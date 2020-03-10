using System;
using CommandLine;

//Todo: If Verbose is set, output the total size of the files.

// Exit Codes:
// 0: Success 
// 1: Bad Directory
// 2: Bad files (if all the files are bad for example a bad path)

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
            [Option('f', "File Paths", Required = true, HelpText = "Gives the application the file paths for the files to copy.")]
            public string[] FilesToCopy { get; set; }
            [Option('d', "Directory to copy files into.", Required = true, HelpText = "Gives the application the place where the files will be copied into.")]
            public string Directory { get; set; }
        }
        
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args).WithParsed(o => {
                Utils.Copy(o.FilesToCopy, o.Directory, o.Verbose);
            });
        }
    }
}