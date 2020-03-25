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
        public static bool VerifyFile(string location)
        {
            bool valid = false;
            if (!string.IsNullOrWhiteSpace(location))
                valid = File.Exists(location);
            return valid;
        }

        /// <summary>
        /// Verifies a directory based on the path given though the string.
        /// </summary>
        /// <param name="directory">the path to the directory.</param>
        /// <returns>a boolean value based on if the directory is valid or not.</returns>
        public static bool VerifyDirectory(string directory)
        {
            bool valid = false;
            if (!string.IsNullOrWhiteSpace(directory))
                valid = Directory.Exists(directory);
            return valid;
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
                if (!VerifyFile(x))
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
                }
                else
                {
                    File.Copy(x, $"{directory}\\{FileName(x)}");
                }
            });
        }

        /// <summary>
        /// Copies the files into the specified directory.
        /// </summary>
        /// <param name="files">File Paths.</param>
        /// <param name="directory">Directory Path</param>
        /// <param name="filter">The filter argument for copying files.</param>
        /// <param name="verbose">specifies if console output is requires.</param>
        public static void Copy(string[] files, string directory, string filter, bool verbose)
        {
            if (!VerifyDirectory(directory))
                throw new DirectoryNotFoundException();
            
            if (string.IsNullOrWhiteSpace(filter))
                throw new ArgumentException("the Filter argument is incorrect.");
            
            if (verbose)
                Console.WriteLine($"Size of files in bytes: {FileSize(files)}");

            Parallel.ForEach(files, (x) =>
            {
                if (!VerifyFile(x))
                {
                    if (verbose)
                    {
                        //Todo: Better Logging.
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write($"File {x} has errored for some reason; " +
                                      $"most likely because the filepath is incorrect.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    string Fn = FileName(x);
                    
                    // Todo: make this more safe!
                    if (Fn.Split('.')[1] == filter)
                    {
                        File.Copy(x, $"{directory}\\{FileName(x)}");
                    }
                }
            });
        }
    }
}