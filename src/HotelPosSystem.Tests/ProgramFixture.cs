using FlaUI.Core;

namespace HotelPosSystem.Tests {
    public class ProgramFixture : IDisposable {
        public readonly Application Application;

        private readonly string _testDatabaseCopyPath = string.Empty;

        public ProgramFixture() {
            string srcDirectoryPath = "../../../../";
            string executablePath = "HotelPosSystem/bin/Release/net8.0-windows/win-x64/HotelPosSystem.exe";
            string path = Path.Combine(srcDirectoryPath, executablePath);
            string testDatabasePath = Path.Combine(srcDirectoryPath, "HotelPosSystem.Tests/testData.db");

            // Run tests on a copy of the database to ensure that the data stays the same
            // The copy is discarded after the tests have finished running
            _testDatabaseCopyPath = testDatabasePath + ".temp";
            File.Copy(testDatabasePath, _testDatabaseCopyPath);

            Application = Application.Launch(path, _testDatabaseCopyPath);
        }

        public void Dispose() {
            Application.Close();
            Application.Dispose();
            File.Delete(_testDatabaseCopyPath);
        }
    }
}
