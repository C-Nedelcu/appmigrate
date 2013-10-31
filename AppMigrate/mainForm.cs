using AppMigrate.Classes;
using AppMigrate.Properties;
using BrightIdeasSoftware;
using Ionic.Zip;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace AppMigrate
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        static ProgressDialogForm ProgressForm;
        static ArrayList CurrentObjects;
        static string SelectedPath;
        static string OperationMode = "EXPORT";
        static bool UpdateWanted = false;

        /// <summary>
        /// Form is loading; prepare list of apps and load into list
        /// </summary>
        private void mainForm_Load(object sender, EventArgs e)
        {

            // Check if AppCatalog.xml is present. If not, update database
            if (!File.Exists("AppCatalog.xml"))
            {
                MessageBox.Show("Welcome to AppMigrate.\nThe latest application database will now be downloaded from the Internet. Please make sure your computer has an active Internet connection.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateWanted = true; // We want an update as soon as the main form is shown
            }
            else if (Settings.Default.CheckDatabaseUPdateOnStartup)
            {
                UpdateWanted = true;
            }
            else
            {
                InitializeAppList();
            }
        }


        // Load app catalog and prepare main window
        public void InitializeAppList(bool reload = false)
        {
            AppCatalog.SupportedAppCatalog = AppCatalog.GetSupportedApps(reload);

            // Null? Any problem?
            if (AppCatalog.SupportedAppCatalog == null)
            {
                MessageBox.Show("Error: the application catalog XML file seems to be invalid.\nAppMigrate cannot load the list of supported applications.\nTry updating the database; if the problem persists, visit www.appmigrate.com to manually download the latest application catalog.","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            AppCatalog.SystemAppCatalog = AppCatalog.GetInstalledApps(reload);
            // Null? Any problem?
            if (AppCatalog.SystemAppCatalog == null)
            {
                MessageBox.Show("Error: AppMigrate is unable to detect the applications installed on your system.\nTry reloading the application as Administrator?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            appListView.SetObjects(AppCatalog.SystemAppCatalog);
            appListView.ShowGroups = false;
            appListView.Sort(1);

            // Apply filtering
            tsbFilterApps_CheckStateChanged(null, null);
        }

        /// <summary>
        /// Occurs when an item wants to be selected. 
        /// We allow selection only if the application is Supported
        /// </summary>
        private void appListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // Ensure unsupported items cannot be selected
            OLVListItem item = (OLVListItem)e.Item;
            AppInstallInfo app = (AppInstallInfo)item.RowObject;

            if (!app.Supported && e.IsSelected) e.Item.Selected = false;

            // Depending on how many items are selected, we enable/disable the Export Selected button
            tsbExport.Enabled = ( appListView.SelectedItems.Count > 0 );
        }


        private void tsbExport_Click(object sender, EventArgs e)
        {
            // Browse folder
            if (folderBrowserDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel) return;
            SelectedPath = folderBrowserDialog1.SelectedPath;
            OperationMode = "EXPORT";

            // begin the export
            CurrentObjects = (ArrayList) appListView.SelectedObjects;

            // Build form
            ProgressForm = new ProgressDialogForm();
            ProgressForm.Show();
            ProgressForm.pbImage.Image = (Image) tsbExport.Image.Clone();
            ProgressForm.label1.Text = "Exporting...";

            // Launch thread
            appWorker.RunWorkerAsync(CurrentObjects);
        }

        private void tsbFilterApps_CheckStateChanged(object sender, EventArgs e)
        {
            if (tsbSupportedAppsOnly.CheckState == CheckState.Checked)
            {
                appListView.UseFiltering = true;
                appListView.ModelFilter = new ModelFilter(delegate(object x) { return ((AppInstallInfo)x).Supported; });
                var objs = (ArrayList) appListView.FilteredObjects;
                if (objs.Count == 0)
                {
                    MessageBox.Show("AppMigrate did not detect any supported application on your system. Before importing AppMigrate packages, ensure the concerned applications are already installed on the system.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            else
            {
                appListView.UseFiltering = false;
            }
        }


        // Thread code
        private void appWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Export or import?
            if (OperationMode == "EXPORT")
            {
                // Begin exporting
                var objects = (ArrayList)e.Argument;

                float i = 0;
                float total = objects.Count;

                foreach (AppInstallInfo app in objects)
                {
                    // Cancel?
                    if (ProgressForm.WantCancel) break;


                    double percent = i / total * 100;
                    int myInt = (int)Math.Ceiling(percent);
                    i++;

                    appWorker.ReportProgress(myInt, app);
                    app.package.AddProgressEvent = zip_AddProgress;
                    app.package.ExtractProgressEvent = zip_ExtractProgress;
                    app.package.ReadProgressEvent = zip_ReadProgress;
                    app.package.SaveProgressEvent = zip_SaveProgress;

                    app.package.Export(SelectedPath,ProgressForm);
                }
            }

            // Import
            else if (OperationMode == "IMPORT")
            {

                // Begin exporting
                var objects = (ArrayList)e.Argument;

                float i = 0;
                float total = objects.Count;

                foreach (String filename in objects)
                {
                    // Cancel?
                    if (ProgressForm.WantCancel) break;

                    double percent = i / total * 100;
                    int myInt = (int)Math.Ceiling(percent);
                    i++;

                    string justname = Path.GetFileName(filename);
                    appWorker.ReportProgress(myInt, "Opening "+justname);


                    AppImport app = new AppImport(filename);
                    if (!app.CheckSupport())
                    {
                        appWorker.ReportProgress(myInt, "Failed "+justname);
                        MessageBox.Show("The file '" + filename + "' does not contain any supported application package.\nAre you using a recent version of the AppMigrate database?","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string error = "";

                        // Begin package unzipping (show error if failed, then go to next object)
                        appWorker.ReportProgress(myInt, "Reading " + app.package.Name + " package...");
                        ProgressForm.currentApp = app.package;
                        app.package.AddProgressEvent = zip_AddProgress;
                        app.package.ExtractProgressEvent = zip_ExtractProgress;
                        app.package.ReadProgressEvent = zip_ReadProgress;
                        app.package.SaveProgressEvent = zip_SaveProgress;
                        error = app.UnzipPackage();
                        if (error != "")
                        {
                            MessageBox.Show("Could not extract "+app.package.Name+" package data:\nError 1: "+error,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            app.RemoveTemporaryFiles(); // remove temp files anyway
                            continue;
                        }

                        // Cancel?
                        if (ProgressForm.WantCancel) break;

                        // Begin registry data import (show error if failed, then go to next object)
                        appWorker.ReportProgress(myInt, "Importing " + app.package.Name + " registry data...");
                        error = app.ImportRegistry();
                        if (error != "")
                        {
                            MessageBox.Show("Could not import " + app.package.Name + " registry data:\nError 2: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            app.RemoveTemporaryFiles(); // remove temp files anyway
                            continue;
                        }

                        // Cancel?
                        if (ProgressForm.WantCancel) break;

                        // Begin user file import (show error if failed, then go to next object)
                        appWorker.ReportProgress(myInt, "Importing " + app.package.Name + " user files...");
                        error = app.ImportFiles();
                        if (error != "")
                        {
                            MessageBox.Show("Could not extract " + app.package.Name + " user files:\nError 3: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            app.RemoveTemporaryFiles(); // remove temp files anyway
                            continue;
                        }

                        // Cancel?
                        if (ProgressForm.WantCancel) break;

                        // Remove temporary files (show error if failed, then go to next object)
                        appWorker.ReportProgress(myInt, "Removing temporary files...");
                        error = app.RemoveTemporaryFiles();
                        if (error != "")
                        {
                            MessageBox.Show("Could not delete " + app.package.Name + " temp files:\nError 4: " + error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

                    }
                }

            }
        }


        // When the progress changes
        private void appWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Cancelling?
            if (ProgressForm.WantCancel) return;

            // If it's just for updating the detail label, we dont care
            if (e.ProgressPercentage < 0)
            {
                ProgressForm.lbFilename.Text = (string)e.UserState;
                if (OperationMode == "EXPORT")
                    ProgressForm.lbDetail.Text = "Exporting " + ProgressForm.currentApp.Name + "... (" + Math.Abs(e.ProgressPercentage) + "%)";
                else if (OperationMode == "IMPORT")
                    ProgressForm.lbDetail.Text = "Importing " + ProgressForm.currentApp.Name + "... (" + Math.Abs(e.ProgressPercentage) + "%)";
                
                return;
            }

            ProgressForm.pbProgress.Value = e.ProgressPercentage;
            if (e.UserState is string)
            {
                ProgressForm.lbDetail.Text = (string)e.UserState;
            }
            else
            {
                AppInstallInfo app = (AppInstallInfo)e.UserState;
                ProgressForm.currentApp = app.package;
                ProgressForm.lbDetail.Text = "Exporting " + app.package.Name + "... (" + e.ProgressPercentage + "%)";
            }
        }


        // When the thread is complete
        private void appWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressForm.pbProgress.Value = 100;
            ProgressForm.lbDetail.Text = "Complete";
            ProgressForm.lbFilename.Text = "";
            MessageBox.Show("Operation complete.","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
            ProgressForm.Close();
            ProgressForm.Dispose();
        }



        // Begin importing some files
        private void tsbImport_Click(object sender, EventArgs e)
        {
            // File selection
            if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel) return;
            OperationMode = "IMPORT";

            // Build form
            ProgressForm = new ProgressDialogForm();
            ProgressForm.Show();
            ProgressForm.pbImage.Image = (Image)tsbImport.Image.Clone();
            ProgressForm.label1.Text = "Importing...";
            CurrentObjects = new ArrayList();

            // For each file...
            foreach (string filename in openFileDialog1.FileNames)
            {
                CurrentObjects.Add(filename);
            }

            appWorker.RunWorkerAsync(CurrentObjects);
        }

        private void tsbAbout_Click(object sender, EventArgs e)
        {
            AboutAppMigrate form = new AboutAppMigrate();
            form.ShowDialog();
        }

        private void tsbUpdate_Click(object sender, EventArgs e)
        {
            // Show progress dialog
            ProgressDialogForm form = new ProgressDialogForm();
            form.BeginDBUpdate(this);
            form.ShowDialog();
        }

        private void mainForm_Shown(object sender, EventArgs e)
        {
            // An update of the app database is required
            if (UpdateWanted)
            {
                UpdateWanted = false;
                tsbUpdate_Click(sender, e);
            }

            // Are we importing a package from command-line?
            if (Program.FileImportWanted != null)
            {
                // We need a confirmation dialog in case the file was double clicked by mistake...
                if (MessageBox.Show("Are you sure you want to import this package?\n\""+Program.FileImportWanted+"\"\n\nSettings for this application will be replaced by the ones contained in this package.","Confirm",MessageBoxButtons.OKCancel,MessageBoxIcon.Question) != DialogResult.OK) {
                    Program.FileImportWanted = null;
                    return;
                }

                OperationMode = "IMPORT";
                CurrentObjects = new ArrayList();

                // For each file...
                CurrentObjects.Add(Program.FileImportWanted);
                Program.FileImportWanted = null;

                // Build form
                ProgressForm = new ProgressDialogForm();
                ProgressForm.Show();
                ProgressForm.pbImage.Image = (Image)tsbImport.Image.Clone();
                ProgressForm.label1.Text = "Importing...";

                // Run worker
                appWorker.RunWorkerAsync(CurrentObjects);
            }
        }




        //////////////////////////////
        /// The events below occur 
        /// when there is a change 
        /// in the ZIP processing
        //////////////////////////////

        void zip_AddProgress(object sender, AddProgressEventArgs e)
        {
            // Do we want to cancel?
            if (ProgressForm.WantCancel)
            {
                e.Cancel = true;
                return;
            }

            // Occurs when a file was added to the zip
            if (e.EventType == ZipProgressEventType.Adding_AfterAddEntry)
            {
                appWorker.ReportProgress(-1, "Adding " + e.CurrentEntry.FileName);
            }
        }

        void zip_ExtractProgress(object sender, ExtractProgressEventArgs e)
        {
            // Do we want to cancel?
            if (ProgressForm.WantCancel)
            {
                e.Cancel = true;
                return;
            }

            // Occurs when a file was extracted from the zip
            if (e.EventType == ZipProgressEventType.Extracting_BeforeExtractEntry)
            {
                float fpercent = ((float)e.EntriesExtracted) / ((float)e.EntriesTotal) * ((float)100.0);
                int percent = (int)Math.Floor(fpercent);
                percent++;
                appWorker.ReportProgress(percent * -1, "Extracting " + e.CurrentEntry.FileName);
            }
        }

        void zip_SaveProgress(object sender, SaveProgressEventArgs e)
        {
            // Do we want to cancel?
            if (ProgressForm.WantCancel)
            {
                e.Cancel = true;
                return;
            }

            // Occurs when a file is being written to the zip
            if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
            {
                float fpercent = ((float)e.EntriesSaved) / ((float)e.EntriesTotal) * ((float)100.0);
                int percent = (int)Math.Floor(fpercent);
                percent++;
                appWorker.ReportProgress(percent * -1, "Saving " + e.CurrentEntry.FileName);
            }
        }

        void zip_ReadProgress(object sender, ReadProgressEventArgs e)
        {
            // Do we want to cancel?
            if (ProgressForm.WantCancel)
            {
                e.Cancel = true;
                return;
            }

            // Occurs when a file is being read from the zip
            if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
                appWorker.ReportProgress(-1, "Reading " + e.CurrentEntry.FileName);
        }

        private void mainForm_Activated(object sender, EventArgs e)
        {
            // If the selection is empty, disable the Save button
            tsbExport.Enabled = appListView.SelectedItems.Count > 0;
        }

        private void tsbSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

    }
}
