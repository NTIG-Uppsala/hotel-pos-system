namespace HotelPosSystem {
    internal static class Program {
        internal static string? DatabasePath;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args) {
            if (args.Length > 0) {
                DatabasePath = args[0];
            } else {
                Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
                string path = Environment.GetFolderPath(folder);
                DatabasePath = Path.Join(path, "hotelPosSystem.db");
            }

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
