using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace AppMigrate
{
    public partial class ProgressDialogForm : Form
    {
        public mainForm owner;
        public bool DoUpdate = false;
        public AppPackage currentApp = null;
        public bool WantCancel = false;

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public ProgressDialogForm()
        {
            InitializeComponent();
            DoUpdate = false; 
            WantCancel = false;
        }

        internal void BeginDBUpdate(mainForm form)
        {
            // Download update
            label1.Text = "Updating catalog...";
            lbDetail.Text = "Connecting...";
            pbImage.Image = (Image)form.tsbUpdate.Image.Clone();
            DoUpdate = true;
            owner = form;
        }

        private void ProgressDialogForm_Shown(object sender, EventArgs e)
        {
            if (DoUpdate)
            {
                var savePath = Path.GetDirectoryName(Application.ExecutablePath)+"\\AppCatalog.xml";

                WebClient client = new WebClient();
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileAsync(new Uri("http://www.appmigrate.com/getdb"), savePath);
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Update progress state
            lbDetail.Text = "Downloading (" + e.ProgressPercentage + "%)";
            pbProgress.Value = e.ProgressPercentage;
        }

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // Any error?
            if (e.Error != null)
            {
                File.Delete(Path.GetDirectoryName(Application.ExecutablePath) + "\\AppCatalog.xml");
                MessageBox.Show("The database could not be downloaded.\nSystem error: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
                return;
            }

            // Download complete!
            MessageBox.Show("Database update successful. Click OK to continue.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lbDetail.Text = "Refreshing application list...";
            Application.DoEvents();
            owner.InitializeAppList(true);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.WantCancel = true;
            this.lbDetail.Text = "Cancelling...";
            this.lbFilename.Text = "";
        }

        private void ProgressDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
