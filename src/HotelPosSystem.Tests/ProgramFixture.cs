using FlaUI.Core;

namespace HotelPosSystem.Tests {
    public class ProgramFixture : IDisposable {
        public readonly Application Application;

        public ProgramFixture() {
            string srcDirectoryPath = "../../../../";
            string executablePath = "HotelPosSystem/bin/Release/net8.0-windows/HotelPosSystem.exe";
            string path = Path.Combine(srcDirectoryPath, executablePath);
            Application = Application.Launch(path);
        }

        public void Dispose() {
            Application.Close();
            Application.Dispose();
        }
    }
}
