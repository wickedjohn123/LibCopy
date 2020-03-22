using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    class TestUtils
    {
        public string BasePath { get; }

        private static int GetAndIncrementNextTempFileNumber()
        {
            return nextTempFileNumber++;
        }

        private static int nextTempFileNumber = 0;

        private static int GetAndIncrementNextTempDirNumber()
        {
            return nextTempDirNumber++;
        }

        private static int nextTempDirNumber = 0;


        public TestUtils(string basePath)
        {
            this.BasePath = basePath;
        }

        public string GetTempFileName()
        {
            return "tempfile" + GetAndIncrementNextTempFileNumber();
        } 
        
        public string GetTempDirName()
        {
            return "tempdir" + GetAndIncrementNextTempDirNumber();
        }
    }
}
