using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace AppMigrate
{
    static class Program
    {
        public static string FileImportWanted = null;


        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set current directory
            string exeDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Directory.SetCurrentDirectory(exeDir);

            // If the first argument is a file name, we store it, because we're going to want to import it
            if (args.Length > 0) {
                FileImportWanted = args[0];
                if (!File.Exists(FileImportWanted)) FileImportWanted = "";
            }

            Application.Run(new mainForm());
        }
    }
}
