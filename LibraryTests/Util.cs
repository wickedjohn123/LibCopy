using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    class Util
    {
        public string BasePath { get; }

        private static int GetAndIncrementNextTempFileNumber()
        {
            return nextTempFileNumber++;
        }

        private static int nextTempFileNumber = 0;


        public Util(string basePath)
        {
            this.BasePath = basePath;
        }

        public static string GetTempFileName()
        {
            return "tempfile" + GetAndIncrementNextTempFileNumber();
        }
    }
}
