using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AppMigrate.Classes
{

    public class Folder
    {
        public string Root;
        public string Path;
        public string ID;
        public AppPackage App;

        // Obtain full destination/origin path
        public string GetFullPath()
        {
            // This variable will contain the full path of the folder we're backing up
            var path = "";

            // 0
            if (Root == "Roaming") path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // 1
            else if (Root == "Local") path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // 2
            else if (Root == "LocalLow") path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low";

            // 3
            else if (Root == "MyDocuments") path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // 4
            else if (Root == "MyPictures") path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);

            // 5
            else if (Root == "MyMusic") path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

            // 6
            else if (Root == "Templates") path = Environment.GetFolderPath(Environment.SpecialFolder.Templates);

            // 7
            else if (Root == "UserFolder")
            {
                path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    path = Directory.GetParent(path).FullName;
                }
            }

            // 8
            else if (Root == "CommonAppData") path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

            // 9
            else if (Root == "CommonProgramFiles") path = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles);

            // 10
            else if (Root == "ProgramFiles") path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            // 11
            else if (Root == "SystemDrive")
            {
                string windir = Environment.SystemDirectory; // C:\windows\system32
                path = System.IO.Path.GetPathRoot(Environment.SystemDirectory).Substring(0,2); // C:\
            }

            // 12
            else if (Root == "SystemFolder") path = Environment.SystemDirectory;

            // 13
            else if (Root == "WindowsFolder") path = Directory.GetParent(Environment.SystemDirectory).FullName;

            // Add folder path
            path += "\\" + this.Path;

            return path;
        }

        /// <summary>
        /// Restores a directory from package temporary files (given path)
        /// </summary>
        public void RestoreFromTemp(string folderPath)
        {
            // Step 1: rename existing folder to .bak-time
            // Step 2: move files to destination

            var fullPath = GetFullPath();

            // Build backup folder name
            var bakName = fullPath + ".backup ("+DateTime.Now.Ticks+")";
            
            // Rename old directory, if exists
            if (Directory.Exists(fullPath))
                Directory.Move(fullPath, bakName);

            // Now, create parent if it doesn't exist
            var parent = Directory.GetParent(fullPath).FullName;
            if (!Directory.Exists(parent))
                Directory.CreateDirectory(parent);

            // Now move temp folder to new one
            Directory.Move(folderPath, fullPath);
        }
    }
}
