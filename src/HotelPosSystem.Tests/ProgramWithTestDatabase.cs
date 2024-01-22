using FlaUI.Core;

namespace HotelPosSystem.Tests {
    internal class ProgramWithTestDatabase : IDisposable {
        public readonly Application Application;

        private readonly string _temporaryTestDatabasePath = string.Empty;

        internal ProgramWithTestDatabase() {
            string srcDirectoryPath = "../../../../";
            string executablePathFromSrc = "HotelPosSystem/bin/Release/net8.0-windows/win-x64/HotelPosSystem.exe";
            string executablePath = Path.Combine(srcDirectoryPath, executablePathFromSrc);
            string testDatabasePath = Path.Combine(srcDirectoryPath, "HotelPosSystem.Tests/testData.db");

            // Run tests on a copy of the database to ensure that the data stays the same
            // The copy is discarded after the tests have finished running
            _temporaryTestDatabasePath = testDatabasePath + ".temp";
            File.Copy(testDatabasePath, _temporaryTestDatabasePath, true);

            Application = Application.Launch(executablePath, _temporaryTestDatabasePath);
        }

        public void Dispose() {
            Application.Close();
            Application.Dispose();
            File.Delete(_temporaryTestDatabasePath);
        }
    }
}
