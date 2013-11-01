namespace AppMigrate
{
    partial class ProgressDialogForm
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
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.lbDetail = new System.Windows.Forms.Label();
            this.lbFilename = new System.Windows.Forms.Label();
            this.cancelBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(121, 12);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(64, 64);
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Exporting...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbProgress
            // 
            this.pbProgress.Location = new System.Drawing.Point(16, 127);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(284, 23);
            this.pbProgress.TabIndex = 2;
            // 
            // lbDetail
            // 
            this.lbDetail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDetail.Location = new System.Drawing.Point(13, 162);
            this.lbDetail.Name = "lbDetail";
            this.lbDetail.Size = new System.Drawing.Size(287, 23);
            this.lbDetail.TabIndex = 3;
            this.lbDetail.Text = "Operation";
            this.lbDetail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbFilename
            // 
            this.lbFilename.AutoSize = true;
            this.lbFilename.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFilename.Location = new System.Drawing.Point(13, 185);
            this.lbFilename.Name = "lbFilename";
            this.lbFilename.Size = new System.Drawing.Size(0, 15);
            this.lbFilename.TabIndex = 4;
            this.lbFilename.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cancelBtn
            // 
            this.cancelBtn.Location = new System.Drawing.Point(110, 220);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(89, 32);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // ProgressDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 264);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.lbFilename);
            this.Controls.Add(this.lbDetail);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbImage);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProgressDialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operation in progress...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgressDialogForm_FormClosing);
            this.Shown += new System.EventHandler(this.ProgressDialogForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox pbImage;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ProgressBar pbProgress;
        public System.Windows.Forms.Label lbDetail;
        public System.Windows.Forms.Label lbFilename;
        private System.Windows.Forms.Button cancelBtn;

    }
}