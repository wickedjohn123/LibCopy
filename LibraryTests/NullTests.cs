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
            Assert.ThrowsException<ArgumentNullException>(() => LibCopy.Utils.VerifyFile(null));
        }

        [TestMethod]
        public void EmptyCheckFileExists()
        {
            Assert.ThrowsException<ArgumentException>(() => LibCopy.Utils.VerifyFile(""));
        }

        [TestMethod]
        public void WhitespaceCheckFileExists()
        {
            Assert.ThrowsException<ArgumentException>(() => LibCopy.Utils.VerifyFile(" "));
        }

        [TestMethod]
        public void NullCheckDirectoryExists()
        {
            Assert.ThrowsException<ArgumentNullException>(() => LibCopy.Utils.VerifyDirectory(null));
        }

        [TestMethod]
        public void EmptyCheckDirectoryExists()
        {
            Assert.ThrowsException<ArgumentException>(() => LibCopy.Utils.VerifyDirectory(""));
        }

        [TestMethod]
        public void WhitespaceCheckDirectoryExists()
        {
            Assert.ThrowsException<ArgumentException>(() => LibCopy.Utils.VerifyDirectory(" "));
        }
    }
}
