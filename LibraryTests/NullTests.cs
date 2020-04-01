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
                var res = LibCopy.Tools.FileExists(null);
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
                var res = LibCopy.Tools.DirectoryExists(null);
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
            Assert.ThrowsException<ArgumentNullException>(() => LibCopy.Tools.FileName(null));
        }

        [TestMethod]
        public void NullCheckFilesize()
        {
            Assert.ThrowsException<ArgumentNullException>(() => LibCopy.Tools.GetFileSize(null));
        }

        [TestMethod]
        public void NullCheckFilesizeArray()
        {
            Assert.ThrowsException<ArgumentNullException>(() => LibCopy.Tools.GetFileSize(new string[] { null, null }));
        }
    }
}
