using FlaUI.Core;

namespace HotelPosSystem.Tests {
    internal class ProgramWithTestDatabase : IDisposable {
        public Application Application;

        private readonly string _temporaryTestDatabasePath;
        private readonly string _executablePath;

        internal ProgramWithTestDatabase() {
            string srcDirectoryPath = "../../../../";
            string executablePathFromSrc = "HotelPosSystem/bin/Release/net8.0-windows/win-x64/HotelPosSystem.exe";
            _executablePath = Path.Combine(srcDirectoryPath, executablePathFromSrc);
            string testDatabasePath = Path.Combine(srcDirectoryPath, "HotelPosSystem.Tests/testData.db");

            // Run tests on a copy of the database to ensure that the data stays the same
            // The copy is discarded after the tests have finished running
            _temporaryTestDatabasePath = testDatabasePath + ".temp";
            File.Copy(testDatabasePath, _temporaryTestDatabasePath, true);

            StartProgram();
        }

        public void RestartProgram() {
            CloseProgram();
            StartProgram();
        }

        private void StartProgram() {
            Application = Application.Launch(_executablePath, _temporaryTestDatabasePath);
        }

        private void CloseProgram() {
            Application.Close();
            Application.Dispose();
        }

        public void Dispose() {
            CloseProgram();
            File.Delete(_temporaryTestDatabasePath);
        }
    }
}
