namespace AppMigrate
{
    partial class mainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbSupportedAppsOnly = new System.Windows.Forms.ToolStripButton();
            this.tsbImport = new System.Windows.Forms.ToolStripButton();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbUpdate = new System.Windows.Forms.ToolStripButton();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.appListView = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.appWorker = new System.ComponentModel.BackgroundWorker();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tsbSettings = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appListView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(731, 95);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSupportedAppsOnly,
            this.tsbImport,
            this.tsbExport,
            this.tsbUpdate,
            this.tsbSettings,
            this.tsbAbout});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(731, 92);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbSupportedAppsOnly
            // 
            this.tsbSupportedAppsOnly.AutoSize = false;
            this.tsbSupportedAppsOnly.AutoToolTip = false;
            this.tsbSupportedAppsOnly.Checked = true;
            this.tsbSupportedAppsOnly.CheckOnClick = true;
            this.tsbSupportedAppsOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbSupportedAppsOnly.Image = ((System.Drawing.Image)(resources.GetObject("tsbSupportedAppsOnly.Image")));
            this.tsbSupportedAppsOnly.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSupportedAppsOnly.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSupportedAppsOnly.Name = "tsbSupportedAppsOnly";
            this.tsbSupportedAppsOnly.Size = new System.Drawing.Size(120, 89);
            this.tsbSupportedAppsOnly.Text = "Supported apps only";
            this.tsbSupportedAppsOnly.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSupportedAppsOnly.ToolTipText = "Click this button to view all applications installed on your system, or only the " +
    "applications that are supported by AppMigrate.";
            this.tsbSupportedAppsOnly.CheckStateChanged += new System.EventHandler(this.tsbFilterApps_CheckStateChanged);
            // 
            // tsbImport
            // 
            this.tsbImport.AutoSize = false;
            this.tsbImport.AutoToolTip = false;
            this.tsbImport.Image = ((System.Drawing.Image)(resources.GetObject("tsbImport.Image")));
            this.tsbImport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImport.Name = "tsbImport";
            this.tsbImport.Size = new System.Drawing.Size(120, 89);
            this.tsbImport.Text = "Import package(s)";
            this.tsbImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbImport.ToolTipText = "Import existing AppMigrate packages into your system";
            this.tsbImport.Click += new System.EventHandler(this.tsbImport_Click);
            // 
            // tsbExport
            // 
            this.tsbExport.AutoSize = false;
            this.tsbExport.AutoToolTip = false;
            this.tsbExport.Enabled = false;
            this.tsbExport.Image = ((System.Drawing.Image)(resources.GetObject("tsbExport.Image")));
            this.tsbExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Size = new System.Drawing.Size(120, 89);
            this.tsbExport.Text = "Export selected";
            this.tsbExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbExport.ToolTipText = "Export selected application packages for migration";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // tsbUpdate
            // 
            this.tsbUpdate.AutoSize = false;
            this.tsbUpdate.AutoToolTip = false;
            this.tsbUpdate.Image = ((System.Drawing.Image)(resources.GetObject("tsbUpdate.Image")));
            this.tsbUpdate.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpdate.Name = "tsbUpdate";
            this.tsbUpdate.Size = new System.Drawing.Size(120, 89);
            this.tsbUpdate.Text = "Update database";
            this.tsbUpdate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbUpdate.ToolTipText = "Update AppMigrate database (requires an active Internet connection)";
            this.tsbUpdate.Click += new System.EventHandler(this.tsbUpdate_Click);
            // 
            // tsbAbout
            // 
            this.tsbAbout.AutoSize = false;
            this.tsbAbout.AutoToolTip = false;
            this.tsbAbout.Image = ((System.Drawing.Image)(resources.GetObject("tsbAbout.Image")));
            this.tsbAbout.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(120, 89);
            this.tsbAbout.Text = "About AppMigrate";
            this.tsbAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbAbout.ToolTipText = "More information about AppMigrate...";
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.appListView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 95);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(731, 449);
            this.panel2.TabIndex = 1;
            // 
            // appListView
            // 
            this.appListView.AllColumns.Add(this.olvColumn1);
            this.appListView.AllColumns.Add(this.olvColumn2);
            this.appListView.AllColumns.Add(this.olvColumn3);
            this.appListView.AllColumns.Add(this.olvColumn4);
            this.appListView.CheckedAspectName = "";
            this.appListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.olvColumn3,
            this.olvColumn4});
            this.appListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appListView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.appListView.FullRowSelect = true;
            this.appListView.Location = new System.Drawing.Point(0, 0);
            this.appListView.Name = "appListView";
            this.appListView.RenderNonEditableCheckboxesAsDisabled = true;
            this.appListView.ShowImagesOnSubItems = true;
            this.appListView.Size = new System.Drawing.Size(731, 449);
            this.appListView.TabIndex = 0;
            this.appListView.UseCompatibleStateImageBehavior = false;
            this.appListView.UseSubItemCheckBoxes = true;
            this.appListView.View = System.Windows.Forms.View.Details;
            this.appListView.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.appListView_ItemSelectionChanged);
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Supported";
            this.olvColumn1.AspectToStringFormat = " ";
            this.olvColumn1.AutoCompleteEditor = false;
            this.olvColumn1.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
            this.olvColumn1.CellPadding = null;
            this.olvColumn1.CheckBoxes = true;
            this.olvColumn1.IsEditable = false;
            this.olvColumn1.Text = "Supported";
            this.olvColumn1.Width = 70;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "DisplayName";
            this.olvColumn2.CellPadding = null;
            this.olvColumn2.Groupable = false;
            this.olvColumn2.IsEditable = false;
            this.olvColumn2.Text = "Application";
            this.olvColumn2.Width = 200;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "DisplayVersion";
            this.olvColumn3.CellPadding = null;
            this.olvColumn3.Groupable = false;
            this.olvColumn3.IsEditable = false;
            this.olvColumn3.Text = "Version";
            this.olvColumn3.Width = 100;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "Notes";
            this.olvColumn4.CellPadding = null;
            this.olvColumn4.FillsFreeSpace = true;
            this.olvColumn4.Text = "Notes";
            // 
            // appWorker
            // 
            this.appWorker.WorkerReportsProgress = true;
            this.appWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.appWorker_DoWork);
            this.appWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.appWorker_ProgressChanged);
            this.appWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.appWorker_RunWorkerCompleted);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "WARNING: make sure the applications you are backing up are closed before beginnin" +
    "g the export. Select the folders where AppMigrate should store exported packages" +
    ":";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "appmigrate";
            this.openFileDialog1.Filter = "AppMigrate Packages|*.appmigrate";
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Select packages";
            // 
            // tsbSettings
            // 
            this.tsbSettings.AutoSize = false;
            this.tsbSettings.Image = ((System.Drawing.Image)(resources.GetObject("tsbSettings.Image")));
            this.tsbSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSettings.Name = "tsbSettings";
            this.tsbSettings.Size = new System.Drawing.Size(120, 89);
            this.tsbSettings.Text = "Settings";
            this.tsbSettings.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbSettings.ToolTipText = "Modify settings of AppMigrate";
            this.tsbSettings.Click += new System.EventHandler(this.tsbSettings_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 544);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "mainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AppMigrate: migrate your Windows applications easily";
            this.Activated += new System.EventHandler(this.mainForm_Activated);
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.Shown += new System.EventHandler(this.mainForm_Shown);
            this.panel1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.appListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbImport;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private BrightIdeasSoftware.ObjectListView appListView;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private System.Windows.Forms.ToolStripButton tsbSupportedAppsOnly;
        private System.ComponentModel.BackgroundWorker appWorker;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.ToolStripButton tsbUpdate;
        private System.Windows.Forms.ToolStripButton tsbSettings;
    }
}

