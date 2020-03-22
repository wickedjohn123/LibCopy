using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnitTests;

namespace LibraryTests
{
    [TestClass]
    public class FileTests
    {
        private string basePath = null;
        private TestUtils utilObj = null;
        private const string filenamePattern = "testfile(&|%|$|!|-|_|#| |)(a|A)(|.txt|.TXT)(|.exe)";


        [TestInitialize]
        public void Initialize()
        {
            this.basePath = Path.Combine(new FileInfo(typeof(FileTests).Assembly.Location).Directory.FullName, "filesystem_tests");

            this.utilObj = new TestUtils(this.basePath);

            if (Directory.Exists(this.basePath))
                Directory.Delete(this.basePath, true);

            Directory.CreateDirectory(this.basePath);
        }


        [TestMethod]
        public void EnsureFileExists()
        {
            Directory.CreateDirectory(Path.Combine(this.basePath, "fileexiststest"));

            for (int i = 0; i < 10; i++)
            {
                string tempFilePath = Path.Combine(this.basePath, "fileexiststest", this.utilObj.GetTempFileName());

                if (i > 4) // use an extension
                {
                    tempFilePath += ".txt";
                }

                Assert.IsFalse(LibCopy.Utils.VerifyFile(tempFilePath));
                Assert.IsFalse(LibCopy.Utils.VerifyDirectory(tempFilePath));
                File.WriteAllText(tempFilePath, "This is a test file.", Encoding.Default);

                Assert.IsTrue(LibCopy.Utils.VerifyFile(tempFilePath));
                Assert.IsFalse(LibCopy.Utils.VerifyDirectory(tempFilePath));
            }
        }

        [TestMethod]
        public void EnsureDirectoryExists()
        {
            Directory.CreateDirectory(Path.Combine(this.basePath, "direxiststest"));

            for (int i = 0; i < 10; i++)
            {
                string tempDirectoryPath = Path.Combine(this.basePath, "direxiststest", this.utilObj.GetTempDirName());

                if (i > 4) // use a name which contains a dot
                {
                    tempDirectoryPath += ".txt";
                }

                Assert.IsFalse(LibCopy.Utils.VerifyDirectory(tempDirectoryPath));
                Assert.IsFalse(LibCopy.Utils.VerifyFile(tempDirectoryPath));
                Directory.CreateDirectory(tempDirectoryPath);
                Assert.IsTrue(LibCopy.Utils.VerifyDirectory(tempDirectoryPath));
                Assert.IsFalse(LibCopy.Utils.VerifyFile(tempDirectoryPath));
            }
        }

        [TestMethod]
        public void TestGetFilename()
        {
            var expanded = RegexExpander.Expand(filenamePattern);
            var expandedPrefixed = expanded.Select(x => Path.Combine(this.basePath, "testgetfilename", x)).ToList();

            foreach (var filePath in expandedPrefixed)
            {
                var fn = new FileInfo(filePath).Name;
                var libFn = LibCopy.Utils.FileName(filePath);

                Assert.AreEqual(fn, libFn);
            }
        }

        [TestMethod]
        public void TestFilesize()
        {
            var expanded = RegexExpander.Expand(filenamePattern);
            var subdirA = CreateSubDirectory(Path.Combine("testfilesize", "folderA"));
            var subdirB = CreateSubDirectory(Path.Combine("testfilesize", "folderB"));

            var expandedPrefixedA = expanded.Select(x => Path.Combine(subdirA, x)).ToList();

            var size = CreateAndFillFiles(expandedPrefixedA);
            var libSize = LibCopy.Utils.FileSize(expandedPrefixedA.ToArray());
            Assert.AreEqual(size, libSize);
        }

        [TestMethod]
        public void TestCopy()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(this.basePath))
                Directory.Delete(this.basePath, true);
        }

        private string CreateSubDirectory(params string[] dirnames)
        {
            string path = Path.Combine(this.basePath, Path.Combine(dirnames));
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Creates all specified files and returns the total number of bytes written.
        /// </summary>
        /// <param name="files">The filenames of the files that are to be created.</param>
        /// <returns>The total number of bytes written.</returns>
        private int CreateAndFillFiles(List<string> files)
        {
            int totalLength = 0;
            int nextByte = 234;

            for (int i = 0; i < files.Count; i++)
            {
                var path = files[i];
                int fileLength = nextByte * 500 + 512;
                using (var handle = File.OpenWrite(path))
                {
                    for (int j = 0; j < fileLength; j++)
                    {
                        handle.WriteByte((byte)nextByte);
                        nextByte = (nextByte * 9 + 25) % 255;
                        totalLength++;
                    }
                }
            }

            return totalLength;
        }
    }
}
