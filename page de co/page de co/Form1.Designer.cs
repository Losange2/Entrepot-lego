namespace page_de_co
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pblogo = new PictureBox();
            ltitre = new Label();
            tbutil = new TextBox();
            ((System.ComponentModel.ISupportInitialize)pblogo).BeginInit();
            SuspendLayout();
            // 
            // pblogo
            // 
            pblogo.Image = (Image)resources.GetObject("pblogo.Image");
            pblogo.Location = new Point(1, -1);
            pblogo.Name = "pblogo";
            pblogo.Size = new Size(1048, 1030);
            pblogo.SizeMode = PictureBoxSizeMode.StretchImage;
            pblogo.TabIndex = 0;
            pblogo.TabStop = false;
            pblogo.Click += pblogo_Click;
            // 
            // ltitre
            // 
            ltitre.AutoSize = true;
            ltitre.Font = new Font("Segoe UI", 24F);
            ltitre.Location = new Point(1055, -1);
            ltitre.Name = "ltitre";
            ltitre.Size = new Size(523, 45);
            ltitre.TabIndex = 1;
            ltitre.Text = "Bienvenue sur le logiciel de gestion";
            ltitre.Click += ltitre_Click;
            // 
            // tbutil
            // 
            tbutil.Font = new Font("Segoe UI", 15F);
            tbutil.Location = new Point(1090, 118);
            tbutil.Name = "tbutil";
            tbutil.Size = new Size(440, 34);
            tbutil.TabIndex = 2;
            tbutil.Text = "Nom d'utilisateur";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1580, 1025);
            Controls.Add(tbutil);
            Controls.Add(ltitre);
            Controls.Add(pblogo);
            Font = new Font("Segoe UI", 9F);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pblogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pblogo;
        private Label ltitre;
        private TextBox tbutil;
    }
}
