using System;
using System.IO;
using CommandLine;

//Todo: If Verbose is set, output the total size of the files. 
// testing ci!
// Exit Codes:
// 0: Success 
// 1: Bad Directory
// 2: Bad files (if all the files are bad for example a bad path)
// 3: Invalid command line data.

namespace LibCopy
{
    class Program
    {
        /// <summary>
        /// Information required from the command line parser.
        /// </summary>
        public class Options
        {
            [Option('v', "Verbose", Required = false, HelpText = "Outputs current state of the application.", Default = false)]
            public bool Verbose  { get; set; }
            [Option('f', "File Paths", Required = false, HelpText = "Location of the files that the application will copy.")]
            public string[] FilesToCopy { get; set; }

            [Option('x', "Directory", Required = false, HelpText = "copies everything inside of directory; just a bit more convenient than writing down a hundred file locations.")]
            public string Xdirectory { get; set; }

            [Option('d', "Directory to copy files into.", Required = true, HelpText = "Filepath where the files will be copied into.")]
            public string Directory { get; set; }
        }
        
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => {
                if (Directory.Exists(o.Xdirectory))
                {
                    Utils.Copy(Directory.GetFiles(o.Xdirectory), o.Directory, o.Verbose);
                }
                else if (o.FilesToCopy.Length != 0)
                {
                    Utils.Copy(o.FilesToCopy, o.Directory, o.Verbose);
                }
                else
                {
                    Environment.Exit(3);
                }
            });
        }
    }
}