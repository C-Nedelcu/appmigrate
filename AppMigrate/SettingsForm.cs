using AppMigrate.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AppMigrate
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();

            this.cbCheckDatabaseUpdateOnStartup.Checked = Settings.Default.CheckDatabaseUPdateOnStartup;
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            Settings.Default.CheckDatabaseUPdateOnStartup = this.cbCheckDatabaseUpdateOnStartup.Checked;
            Settings.Default.Password = this.txtPassword.Text;
            Settings.Default.Save();
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbPassword_CheckedChanged(object sender, EventArgs e)
        {
            this.txtPassword.Enabled = this.cbPassword.Checked;
            if(!this.cbPassword.Checked)
            {
                this.txtPassword.Text = string.Empty;
            }
        }
    }
}
