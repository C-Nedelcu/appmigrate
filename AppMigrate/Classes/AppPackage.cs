using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Diagnostics;
using Ionic.Zip;
using System.Windows.Forms;
using AppMigrate.Classes;


namespace AppMigrate
{
    
    public class AppPackage
    {
        /// <summary>
        /// Unique Identifier in the app. database
        /// </summary>
        public string UniqueID;

        /// <summary>
        /// Name of the application (informational only)
        /// </summary>
        public string Name;

        /// <summary>
        /// Match installed names to identify application on the system
        /// </summary>
        public string MatchRegexp;

        /// <summary>
        /// Minimum/Maximum supported versions of the application (inclusive)
        /// </summary>
        public string MinimumVersion;
        public string MaximumVersion;

        /// <summary>
        /// Notes about this particular application, displayed at export time
        /// </summary>
        public string Notes;

        /// <summary>
        /// Registry keys to export
        /// </summary>
        public ArrayList RegistryKeys;

        /// <summary>
        /// Folders to backup
        /// </summary>
        public ArrayList Folders;

        /// <summary>
        /// To follow the progress of zip file extraction or file addition
        /// </summary>
        public EventHandler<AddProgressEventArgs> AddProgressEvent;
        public EventHandler<ExtractProgressEventArgs> ExtractProgressEvent;
        public EventHandler<ReadProgressEventArgs> ReadProgressEvent;
        public EventHandler<SaveProgressEventArgs> SaveProgressEvent;

        /// <summary>
        /// Returns temporary filename for package
        /// </summary>
        public string GetTempPackageFilename()
        {
            return GetAppTemporaryFolder() + "\\" + UniqueID + ".zip";
        }
        
        // Export package
        public bool Export(string OutputFilePath, ProgressDialogForm progressForm)
        {
            // 1. create folder to store temporary files
            string error = CreateAppFolder();
            if (error != "")
            {
                MessageBox.Show("Package creation failed for "+this.Name+": AppMigrate could not create a temporary directory.\n\nSystem error: "+error,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                EraseTempDirectory(); 
                return false;
            }

            // Do we want to cancel?
            if (progressForm.WantCancel) return false;

            // 2. export registry keys (if any)
            error = ExportRegistryKeys();
            if (error != "")
            {
                MessageBox.Show("Package creation failed for " + this.Name + ": AppMigrate could not export required registry data.\n\nSystem error: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EraseTempDirectory();
                return false;
            }

            // Do we want to cancel?
            if (progressForm.WantCancel) return false;

            // 3. copy concerned folders (zipped)
            error = GenerateZipPackage();
            if (error != "")
            {
                MessageBox.Show("Package creation failed for " + this.Name + ": AppMigrate could not read one or more of the required data files.\n\nSystem error: " + error+"\n\nSolution: make sure "+this.Name+" is not running while exporting settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EraseTempDirectory();
                return false;
            }

            // Do we want to cancel?
            if (progressForm.WantCancel) return false;

            // 4. Rename file as .appmigrate to ensure it goes to the appropriate location
            error = MoveFileToDesiredLocation(OutputFilePath + "\\"+ Name + ".appmigrate");
            if (error != "")
            {
                MessageBox.Show("Package creation failed for " + this.Name + ": AppMigrate could not move the finalized package to your chosen destination.\n\nSystem error: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                EraseTempDirectory();
                return false;
            }

            // Do we want to cancel?
            if (progressForm.WantCancel) return false;

            // 5. clean directory
            error = EraseTempDirectory();
            if (error != "")
            {
                //MessageBox.Show("Package creation failed for " + this.Name + ": AppMigrate could not remove temporary directory.\n\nSystem error: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Success!
            return true;
        }

        // Rename or move package to its final location
        private string MoveFileToDesiredLocation(string outputPath)
        {
            try
            {
                // Is there already an existing package ?
                // If so, try to rename the old one
                if (File.Exists(outputPath))
                    File.Move(outputPath, outputPath + ".backup (" + DateTime.Now.Ticks + ")");

                // Do the move
                File.Move(GetTempPackageFilename(), outputPath);
                return "";
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error: could not save package in "+outputPath+"\n\nSystem Error: "+e.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return ex.Message;
            }
        }

        /// <summary>
        /// Erase temporary directory
        /// </summary>
        public string EraseTempDirectory()
        {
            try
            {
                Directory.Delete(GetAppTemporaryFolder(), true);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// Zip folders of the application into the temp directory
        /// </summary>
        private string GenerateZipPackage()
        {
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
                    zip.ParallelDeflateThreshold = -1;

                    if (AddProgressEvent != null)
                        zip.AddProgress += new EventHandler<AddProgressEventArgs>(AddProgressEvent);
                    if (ExtractProgressEvent != null)
                        zip.ExtractProgress += new EventHandler<ExtractProgressEventArgs>(ExtractProgressEvent);
                    if (ReadProgressEvent != null)
                        zip.ReadProgress += new EventHandler<ReadProgressEventArgs>(ReadProgressEvent);
                    if (SaveProgressEvent != null)
                        zip.SaveProgress += new EventHandler<SaveProgressEventArgs>(SaveProgressEvent);
                    

                    // Adding app folders
                    foreach (Folder f in Folders)
                    {
                        zip.AddDirectory(f.GetFullPath(), f.ID);
                    }

                    // Adding registry files
                    foreach (RegKey key in RegistryKeys)
                    {
                        zip.AddFile(key.GetSavePath(), "registry");
                    }

                    // Creation of the zip file
                    zip.Comment = "APPID=[" + this.UniqueID + "] AppMigrate Package for " + this.Name + "(Creation date: " + System.DateTime.Now.ToString("G") + ")";
                    zip.Save(GetTempPackageFilename());
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        // Export registry keys of the application
        private string ExportRegistryKeys()
        {
            try
            {

                for (int i = 0; i < RegistryKeys.Count; i++)
                {
                    RegKey key = (RegKey)RegistryKeys[i];
                    string savePath = key.GetSavePath();
                    string fullKey = key.GetFullKey();
                    ExportKey(fullKey, savePath);
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        // Create app folder in the temporary files
        public string CreateAppFolder()
        {
            try
            {
                var folder = GetAppTemporaryFolder();
                Directory.CreateDirectory(folder);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        // Returns temporary folder for app
        public string GetAppTemporaryFolder()
        {
            return System.IO.Path.GetTempPath()+"appmigrate\\"+UniqueID+"\\";
        }


        // Export 1 registry key
        public void ExportKey(string RegKey, string SavePath)
        {
            string path = "\"" + SavePath + "\"";
            string key = "\"" + RegKey + "\"";

            var proc = new Process();
            try
            {
                proc.StartInfo.FileName = "regedit.exe";
                proc.StartInfo.UseShellExecute = false;
                proc = Process.Start("regedit.exe", "/e " + path + " " + key + "");

                if (proc != null) proc.WaitForExit();
            }
            finally
            {
                if (proc != null) proc.Dispose();
            }

        }

        // Find folder by ID
        public Folder GetFolder(string FolderID)
        {
            if (Folders == null) return null;
            foreach (Folder folder in Folders)
            {
                if (folder.ID == FolderID) return folder;
            }
            return null;
        }
    }
}
