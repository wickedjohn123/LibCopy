using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LibCopy
{
    /// <summary>
    /// Random Util functions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Verifies a file exists based on the path given though the string.
        /// </summary>
        /// <param name="location">the path to the file.</param>
        /// <returns>a boolean value based on if the file is valid or not.</returns>
        public static Tuple<bool, Exception> VerifyFile(string location, Checkconditions checkconditions = Checkconditions.None)
        {
            if ((checkconditions & Checkconditions.Null) != 0)
                if (location == null)
                    return new Tuple<bool, Exception>(false, new ArgumentNullException(nameof(location)));
            if ((checkconditions & Checkconditions.EmptyString) != 0)
                if (location == String.Empty)
                    return new Tuple<bool, Exception>(false, new InvalidOperationException(nameof(location)));
            if ((checkconditions & Checkconditions.WhiteSpaceString) != Checkconditions.WhiteSpaceString)
                if (location == " ")
                    return new Tuple<bool, Exception>(false, new ArgumentException(nameof(location)));
            return new Tuple<bool, Exception>(File.Exists(location), null);
        }

        /// <summary>
        /// Verifies a directory based on the path given though the string.
        /// </summary>
        /// <param name="directory">the path to the directory.</param>
        /// <returns>a boolean value based on if the directory is valid or not.</returns>
        public static Tuple<bool, Exception> VerifyDirectory(string directory, Checkconditions checkconditions = Checkconditions.None)
        {
            if ((checkconditions & Checkconditions.Null) != 0)
                if (directory == null)
                    return new Tuple<bool, Exception>(false, new ArgumentNullException(nameof(directory)));
            if ((checkconditions & Checkconditions.EmptyString) != 0)
                if (directory == String.Empty)
                    return new Tuple<bool, Exception>(false, new InvalidOperationException(nameof(directory)));
            if ((checkconditions & Checkconditions.WhiteSpaceString) != Checkconditions.WhiteSpaceString)
                if (directory == " ")
                    return new Tuple<bool, Exception>(false, new ArgumentException(nameof(directory)));
            return new Tuple<bool, Exception>(Directory.Exists(directory), null);
        }

        /// <summary>
        /// Gets the file's name from the filepath.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FileName(string filePath)
        {
            var ex = VerifyFile(filePath, Checkconditions.Null);
            if (ex.Item1 == false)
                throw ex.Item2;

            // Todo: Find a less hacky method of getting the file's name.
            string[] name = filePath.Split(Path.DirectorySeparatorChar);
            return name[name.Length - 1];
        }

        /// <summary>
        /// Gets the size of all the files passed to it and returns the size in bytes.
        /// </summary>
        /// <param name="files">all of the files' filepath.</param>
        /// <returns>total size of all the files in bytes.</returns>
        public static float FileSize(params string[] files)
        {
            if (files == null)
                throw new ArgumentNullException();

            float size = 0;
            foreach (var file in files)
            {
                if (file == null)
                    throw new ArgumentNullException();

                size += new FileInfo(file).Length;
            }

            return size;
        }

        /// <summary>
        /// Copies the files into the specified directory.
        /// </summary>
        /// <param name="files">File Paths.</param>
        /// <param name="directory">Directory Path</param>
        /// <param name="verbose">specifies if console output is requires.</param>
        public static void Copy(string[] files, string directory, bool verbose)
        {
            if (!VerifyDirectory(directory).Item1)
                Environment.Exit(1);

            int badFiles = 0;

            if (verbose)
                Console.WriteLine($"Size of files in bytes: {FileSize(files)}");

            Parallel.ForEach(files, (x) =>
            {
                var ex = VerifyFile(x);

                if (!ex.Item1)
                {
                    if (verbose)
                    {
                        //Todo: Better Logging.
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"File {x} has errored for some reason; " +
                                      $"most likely because the filepath is incorrect.");
                        Console.ResetColor();
                    }

                    Interlocked.Increment(ref badFiles);
                    throw ex.Item2;
                }
                else
                {
                    File.Copy(x, $"{directory}\\{FileName(x)}");
                }
            });

            if (files.Length == badFiles)
                Environment.Exit(2);
            else
                Environment.Exit(0);
        }

        [Flags]
        public enum Checkconditions
        {
            None,
            Null,
            EmptyString,
            WhiteSpaceString
        }

    }
}