using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LibraryTests
{
    [TestClass]
    public class NullTests
    {
        [TestMethod]
        public void NullCheckFileExists()
        {
            try
            {
                var res = LibCopy.Utils.VerifyFile(null);
                Assert.IsFalse(res);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void NullCheckDirectoryExists()
        {
            try
            {
                var res = LibCopy.Utils.VerifyDirectory(null);
                Assert.IsFalse(res);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void NullCheckFilename()
        {
            Assert.ThrowsException<ArgumentNullException>(() => LibCopy.Utils.FileName(null));
        }
    }
}
