using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using UnitTests;

namespace LibraryTests
{
    [TestClass]
    public class FileTests
    {
        private string basePath = null;
        private TestUtils utilObj = null;

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
            Directory.CreateDirectory(Path.Combine(basePath, "fileexiststest"));

            for (int i = 0; i < 10; i++)
            {
                string tempFilePath = Path.Combine(this.basePath, "fileexiststest", this.utilObj.GetTempFileName());

                if (i > 4) // use an extension
                {
                    tempFilePath += ".txt";
                }

                Assert.IsFalse(LibCopy.Utils.VerifyFile(tempFilePath));
                File.WriteAllText(tempFilePath, "This is a test file.", Encoding.Default);
                Assert.IsTrue(LibCopy.Utils.VerifyFile(tempFilePath));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(this.basePath))
                Directory.Delete(this.basePath, true);
        }
    }
}
