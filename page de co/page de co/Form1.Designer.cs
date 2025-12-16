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
            tbmdp = new TextBox();
            lutil = new Label();
            lmdp = new Label();
            btnconnect = new Button();
            cbShowPassword = new CheckBox();
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
            tbutil.Location = new Point(1090, 358);
            tbutil.Name = "tbutil";
            tbutil.Size = new Size(440, 34);
            tbutil.TabIndex = 2;
            tbutil.PlaceholderText = "Nom d'utilisateur";
            tbutil.TextChanged += tbutil_TextChanged;
            // 
            // tbmdp
            // 
            tbmdp.Font = new Font("Segoe UI", 15F);
            tbmdp.Location = new Point(1090, 642);
            tbmdp.Name = "tbmdp";
            tbmdp.PasswordChar = '*';
            tbmdp.Size = new Size(457, 34);
            tbmdp.TabIndex = 3;
            tbmdp.PlaceholderText = "Mot de passe";
            // 
            // lutil
            // 
            lutil.AutoSize = true;
            lutil.Font = new Font("Segoe UI", 20F);
            lutil.Location = new Point(1190, 274);
            lutil.Name = "lutil";
            lutil.Size = new Size(235, 37);
            lutil.TabIndex = 4;
            lutil.Text = "Nom d'utilisateur :";
            // 
            // lmdp
            // 
            lmdp.AutoSize = true;
            lmdp.Font = new Font("Segoe UI", 20F);
            lmdp.Location = new Point(1215, 567);
            lmdp.Name = "lmdp";
            lmdp.Size = new Size(189, 37);
            lmdp.TabIndex = 5;
            lmdp.Text = "Mot de passe :";
            // 
            // btnconnect
            // 
            btnconnect.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnconnect.Location = new Point(1190, 820);
            btnconnect.Name = "btnconnect";
            btnconnect.Size = new Size(250, 60);
            btnconnect.TabIndex = 6;
            btnconnect.Text = "Se connecter";
            btnconnect.BackColor = Color.FromArgb(52, 152, 219);
            btnconnect.ForeColor = Color.White;
            btnconnect.FlatStyle = FlatStyle.Flat;
            btnconnect.FlatAppearance.BorderSize = 0;
            btnconnect.UseVisualStyleBackColor = false;
            btnconnect.Click += btnconnect_Click;
            // 
            // cbShowPassword
            // 
            cbShowPassword.AutoSize = true;
            cbShowPassword.Font = new Font("Segoe UI", 10F);
            cbShowPassword.Location = new Point(1090, 690);
            cbShowPassword.Name = "cbShowPassword";
            cbShowPassword.Size = new Size(171, 23);
            cbShowPassword.TabIndex = 7;
            cbShowPassword.Text = "Afficher le mot de passe";
            cbShowPassword.UseVisualStyleBackColor = true;
            cbShowPassword.CheckedChanged += cbShowPassword_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1580, 1025);
            AcceptButton = btnconnect;
            Controls.Add(btnconnect);
            Controls.Add(cbShowPassword);
            Controls.Add(lmdp);
            Controls.Add(lutil);
            Controls.Add(tbmdp);
            Controls.Add(tbutil);
            Controls.Add(ltitre);
            Controls.Add(pblogo);
            Font = new Font("Segoe UI", 9F);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Connexion - LegoFactory";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pblogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pblogo;
        private Label ltitre;
        private TextBox tbutil;
        private TextBox tbmdp;
        private Label lutil;
        private Label lmdp;
        private Button btnconnect;
        private CheckBox cbShowPassword;
    }
}
