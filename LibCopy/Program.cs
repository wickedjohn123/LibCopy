﻿using CommandLine;
using System;
using System.IO;

// Exit Codes:
// 0: Success 
// 1: Bad Directory
// 2: Bad files (if all the files are bad for example a bad path)
// 3: Invalid command line data.
// 4: Unknown exception killed the application.

namespace LibCopy
{
    class Program
    {
        /// <summary>
        /// Information required from the command line parser.
        /// </summary>
        public class Options
        {
            [Option('v', "verbose", Required = false, HelpText = "Outputs current state of the application.", Default = false)]
            public bool Verbose { get; set; }

            [Option('s', "source", Required = false, HelpText = "copies everything inside of directory; just a bit more convenient than writing down a hundred file locations.")]
            public string Source { get; set; }

            [Option('d', "destination", Required = true, HelpText = "Filepath where the files will be copied into.")]
            public string Destination { get; set; }

            [Option('f', "filter", Required = false, HelpText = "Filters the files that will be copied.", Default = null)]
            public string Filter { get; set; }

            [Option('o', "overwrite", Required = false, HelpText = "Enables the application to overwrite files.", Default = false)]
            public bool Overwrite { get; set; }
        }

        static int Main(string[] args)
        {
            try
            {
                Parser.Default.ParseArguments<Options>(args).WithParsed(o =>
                {
                    if (Directory.Exists(o.Source))
                    {
                        if (o.Filter != null)
                        {
                            string[] filitersplit = o.Filter.Split('.');
                            if (filitersplit.Length > 0)
                            {
                                Tools.FilteredCopy(Directory.GetFiles(o.Source), o.Destination, filitersplit[1], o.Verbose, o.Overwrite);
                            }
                            else
                            {
                                Tools.FilteredCopy(Directory.GetFiles(o.Source), o.Destination, filitersplit[0], o.Verbose, o.Overwrite);
                            }
                        }
                        else
                        {
                            Tools.Copy(Directory.GetFiles(o.Source), o.Destination, o.Verbose, o.Overwrite);
                        }
                    }
                    else
                    {
                        throw new DirectoryNotFoundException();
                    }
                });
            }
            catch (Exception)
            {
                return 4;
            }

            return 0;
        }
    }
}