using System;
using System.Collections.Generic;
using System.Text;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Collections;

namespace AppMigrate.Classes
{
    class AppImport
    {
        private string filename;
        private string appID;
        public AppPackage package;

        public AppImport(string filename)
        {
            // TODO: Complete member initialization
            this.filename = filename;
        }

        public bool CheckSupport()
        {
            // We're going to open this file and read the comment in the ZIP to identify the package
            try
            {
                using (ZipFile zip = new ZipFile(this.filename))
                {
                    var comment = zip.Comment;
                    Regex findAppID = new Regex(@"^APPID=\[(?<id>[0-9]*?)\]");
                    MatchCollection mc = findAppID.Matches(comment);
                    if (mc.Count == 0) return false;
                    appID = mc[0].Groups["id"].Value;
                    package = AppCatalog.GetAppPackage(appID);
                    return package != null;
                }

            }
            catch
            {
                appID = null;
                return false;
            }
        }

        // Import registry files
        public string ImportRegistry()
        {
            // List registry files from package and import them all
            try
            {
                var extractFolder = package.GetAppTemporaryFolder();
                var registryFolder = extractFolder + "registry";
                if (!Directory.Exists(registryFolder)) return ""; // No registry?

                var files = Directory.GetFiles(registryFolder);
                foreach (string regFile in files)
                {
                    // Import registry file
                    Process regeditProcess = Process.Start("regedit.exe", "/s \""+regFile+"\"");
                    regeditProcess.WaitForExit();
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message + ex.StackTrace;
            }
        }

        // Import user files from package
        public string ImportFiles()
        {
            // 1) Get directories in temp app folder (all directories but "registry")
            // 2) Match their name with the folders listed in the XML App Catalog
            // 3) Figure out their final destination
            // 4) Move them where they should go

            try
            {
                // Step 1: Get list of directories in the temporary app folder
                var allFolders = Directory.GetDirectories(package.GetAppTemporaryFolder());
                foreach (string folderPath in allFolders)
                {
                    var parts = folderPath.Split('\\');
                    if (parts.Length == 0) continue;
                    var uniqueID = parts[parts.Length - 1];
                    if (uniqueID == "registry") continue;

                    var folder = package.GetFolder(uniqueID);
                    if (folder == null) continue;

                    // Make final move
                    folder.RestoreFromTemp(folderPath);
                }
            }
            catch (Exception ex)
            {
                return ex.Message+ex.StackTrace;
            }

            return "";
        }

        // Unzip all package files to temporary folder
        public string UnzipPackage()
        {
            var extractFolder = package.GetAppTemporaryFolder();

            // Attempt to delete it if it already exists
            try
            {
                if (Directory.Exists(extractFolder))
                    Directory.Delete(extractFolder, true);
            }
            catch
            {
                // Do nothing and hope for the best
            }

            try
            {
                using (ZipFile zip = new ZipFile(this.filename))
                {
                    zip.ParallelDeflateThreshold = -1;

                    if (package.AddProgressEvent != null)
                        zip.AddProgress += new EventHandler<AddProgressEventArgs>(package.AddProgressEvent);
                    if (package.ExtractProgressEvent != null)
                        zip.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(package.ExtractProgressEvent);
                    if (package.ReadProgressEvent != null)
                        zip.ReadProgress += new EventHandler<ReadProgressEventArgs>(package.ReadProgressEvent);
                    if (package.SaveProgressEvent != null)
                        zip.SaveProgress += new EventHandler<SaveProgressEventArgs>(package.SaveProgressEvent);

                    zip.ExtractAll(extractFolder, ExtractExistingFileAction.OverwriteSilently);
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Remove temporary files directory
        /// </summary>
        /// <returns>An error or empty string</returns>
        public string RemoveTemporaryFiles()
        {
            try
            {
                package.EraseTempDirectory();
                return "";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
