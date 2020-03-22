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
        public static Tuple<bool, Exception> VerifyString(string stringcheck, Checkconditions checkconditions = Checkconditions.Null)
        {
            if ((checkconditions & Checkconditions.Null) != 0)
                if (stringcheck == null)
                    return new Tuple<bool, Exception>(false, new ArgumentNullException());
            if ((checkconditions & Checkconditions.EmptyString) != 0)
                if (stringcheck == String.Empty)
                    return new Tuple<bool, Exception>(false, new ArgumentException());
            if ((checkconditions & Checkconditions.WhiteSpaceString) != Checkconditions.WhiteSpaceString)
                if (stringcheck == " ")
                    return new Tuple<bool, Exception>(false, new ArgumentException());
            return new Tuple<bool, Exception>(true, null);
        }


        /// <summary>
        /// Verifies a file exists based on the path given though the string.
        /// </summary>
        /// <param name="location">the path to the file.</param>
        /// <returns>a boolean value based on if the file is valid or not.</returns>
        public static Tuple<bool, Exception> VerifyFile(string location, Nullable<Checkconditions> checkconditions = null)
        {
            if (checkconditions != null)
                return VerifyString(location, checkconditions.Value);
            return new Tuple<bool, Exception>(File.Exists(location), null);
        }

        /// <summary>
        /// Verifies a directory based on the path given though the string.
        /// </summary>
        /// <param name="directory">the path to the directory.</param>
        /// <returns>a boolean value based on if the directory is valid or not.</returns>
        public static Tuple<bool, Exception> VerifyDirectory(string directory, Nullable<Checkconditions> checkconditions = null)
        {
            if (checkconditions != null)
                return VerifyString(directory, checkconditions.Value);
            return new Tuple<bool, Exception>(Directory.Exists(directory), null);
        }

        /// <summary>
        /// Gets the file's name from the filepath.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FileName(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException();

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
            if (!VerifyDirectory(directory))
                throw new DirectoryNotFoundException();

            int badFiles = 0;

            if (verbose)
                Console.WriteLine($"Size of files in bytes: {FileSize(files)}");

            Parallel.ForEach(files, (x) =>
            {
                var xex = VerifyFile(x);

                if (!xex.Item1)
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
                    throw xex.Item2;
                }
                else
                {
                    File.Copy(x, $"{directory}\\{FileName(x)}");
                }
            });

            // wtf is this
            //if (files.Length == badFiles)
            //    Environment.Exit(2);
        }

        [Flags]
        public enum Checkconditions
        {
            None = 1,
            Null = 2,
            EmptyString = 3,
            WhiteSpaceString = 4
        }

    }
}
