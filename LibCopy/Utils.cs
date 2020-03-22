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
        /// <returns></returns>
        public static bool VerifyFile(string location) => File.Exists(location);
        
        /// <summary>
        /// Verifies a directory based on the path given though the string.
        /// </summary>
        /// <param name="directory">the path to the directory.</param>
        /// <returns></returns>
        public static bool VerifyDirectory(string directory) => Directory.Exists(directory);

        /// <summary>
        /// Gets the file's name from the filepath.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FileName(string filePath)
        {
            // Todo: Find a less hacky method of getting the file's name.
            string[] name = filePath.Split(Path.PathSeparator);
            return name[name.Length];
        }

        /// <summary>
        /// Copies the files into the specified directory.
        /// </summary>
        /// <param name="files">File Paths.</param>
        /// <param name="directory">Directory Path</param>
        /// <param name="verbose">specifies if console output is requires.</param>
        public static void Copy(string[] files, string directory, bool verbose)
        {
            if (VerifyDirectory(directory))
                Environment.Exit(1);

            int badFiles = 0;

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
                
                File.Copy(x, $"{directory}\\{FileName(x)}");
            });
            
            if (files.Length == badFiles)
                Environment.Exit(2);
            else
                Environment.Exit(0);
        }
    }
}