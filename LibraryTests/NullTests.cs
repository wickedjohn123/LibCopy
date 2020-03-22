﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LibCopy;

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
                var res = LibCopy.Utils.VerifyFile(null, Utils.Checkconditions.Null);
                Assert.IsFalse(res.Item1);
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
                var res = LibCopy.Utils.VerifyDirectory(null, Utils.Checkconditions.Null);
                Assert.IsFalse(res.Item1);
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

        [TestMethod]
        public void NullCheckFilesize()
        {
            Assert.ThrowsException<ArgumentNullException>(() => LibCopy.Utils.FileSize(null));
        }

        [TestMethod]
        public void NullCheckFilesizeArray()
        {
            Assert.ThrowsException<ArgumentNullException>(() => LibCopy.Utils.FileSize(new string[] { null, null }));
        }
    }
}
