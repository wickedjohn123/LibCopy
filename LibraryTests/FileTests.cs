using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using UnitTests;

namespace LibraryTests
{
    [TestClass]
    public class FileTests
    {
        private string basePath = null;
        private Util utilObj = null;

        [TestInitialize]
        public void Initialize()
        {
            this.basePath = Path.Combine(new FileInfo(typeof(FileTests).Assembly.Location).Directory.FullName, "filesystem_tests");

            this.utilObj = new Util(this.basePath);

            if (Directory.Exists(this.basePath))
                Directory.Delete(this.basePath);

            Directory.CreateDirectory(this.basePath);
        }


        [TestMethod]
        public void EnsureFileExists()
        {
            for (int i = 0; i < 10; i++)
            {
                
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(this.basePath))
                Directory.Delete(this.basePath);
        }
    }
}
