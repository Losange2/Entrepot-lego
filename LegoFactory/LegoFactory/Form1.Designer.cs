namespace LegoFactory
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
            // --- Panels ---
            panelLeft = new Panel();
            panelRight = new Panel();
            panelLoginBox = new Panel();

            // --- Controls ---
            pblogo = new PictureBox();
            lblWelcome = new Label();
            lblSubtitle = new Label();
            ltitre = new Label();
            lblLoginTitle = new Label();
            iconUser = new Label();
            iconLock = new Label();
            tbutil = new TextBox();
            tbmdp = new TextBox();
            panelUserLine = new Panel();
            panelMdpLine = new Panel();
            cbShowPassword = new CheckBox();
            btnconnect = new Button();
            lblError = new Label();

            ((System.ComponentModel.ISupportInitialize)pblogo).BeginInit();
            SuspendLayout();

            // =====================
            // panelLeft (bandeau bleu)
            // =====================
            panelLeft.BackColor = Color.FromArgb(30, 60, 114);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Width = 420;
            panelLeft.Controls.Add(lblSubtitle);
            panelLeft.Controls.Add(lblWelcome);
            panelLeft.Controls.Add(pblogo);

            // pblogo
            string logoPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "img", "logo.png");
            logoPath = Path.GetFullPath(logoPath);
            pblogo.SizeMode = PictureBoxSizeMode.Zoom;
            pblogo.Size = new Size(180, 180);
            pblogo.Location = new Point(120, 80);
            pblogo.BackColor = Color.Transparent;
            pblogo.TabStop = false;
            if (File.Exists(logoPath))
                pblogo.Image = Image.FromFile(logoPath);

            // lblWelcome
            lblWelcome.Text = "Bienvenue";
            lblWelcome.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.White;
            lblWelcome.AutoSize = false;
            lblWelcome.TextAlign = ContentAlignment.MiddleCenter;
            lblWelcome.Size = new Size(420, 55);
            lblWelcome.Location = new Point(0, 290);
            lblWelcome.BackColor = Color.Transparent;

            // lblSubtitle
            lblSubtitle.Text = "Logiciel de gestion\nLegoFactory";
            lblSubtitle.Font = new Font("Segoe UI", 13F, FontStyle.Regular);
            lblSubtitle.ForeColor = Color.FromArgb(180, 200, 230);
            lblSubtitle.AutoSize = false;
            lblSubtitle.TextAlign = ContentAlignment.TopCenter;
            lblSubtitle.Size = new Size(420, 60);
            lblSubtitle.Location = new Point(0, 355);
            lblSubtitle.BackColor = Color.Transparent;

            // =====================
            // panelRight (fond clair)
            // =====================
            panelRight.BackColor = Color.FromArgb(245, 247, 251);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Controls.Add(panelLoginBox);

            // =====================
            // panelLoginBox (carte de connexion)
            // =====================
            panelLoginBox.BackColor = Color.White;
            panelLoginBox.Size = new Size(400, 420);
            panelLoginBox.Location = new Point(80, 100);
            panelLoginBox.Anchor = AnchorStyles.None;
            panelLoginBox.Controls.Add(lblLoginTitle);
            panelLoginBox.Controls.Add(iconUser);
            panelLoginBox.Controls.Add(tbutil);
            panelLoginBox.Controls.Add(panelUserLine);
            panelLoginBox.Controls.Add(iconLock);
            panelLoginBox.Controls.Add(tbmdp);
            panelLoginBox.Controls.Add(panelMdpLine);
            panelLoginBox.Controls.Add(cbShowPassword);
            panelLoginBox.Controls.Add(btnconnect);
            panelLoginBox.Controls.Add(lblError);

            // lblLoginTitle
            lblLoginTitle.Text = "Connexion";
            lblLoginTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lblLoginTitle.ForeColor = Color.FromArgb(30, 60, 114);
            lblLoginTitle.AutoSize = true;
            lblLoginTitle.Location = new Point(30, 30);

            // --- Champ utilisateur ---
            iconUser.Text = "Util.";
            iconUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            iconUser.ForeColor = Color.FromArgb(30, 60, 114);
            iconUser.AutoSize = true;
            iconUser.Location = new Point(32, 112);

            tbutil.Font = new Font("Segoe UI", 12F);
            tbutil.Location = new Point(75, 110);
            tbutil.Size = new Size(285, 28);
            tbutil.BorderStyle = BorderStyle.None;
            tbutil.BackColor = Color.White;
            tbutil.PlaceholderText = "Nom d'utilisateur";
            tbutil.TabIndex = 1;

            panelUserLine.BackColor = Color.FromArgb(200, 210, 230);
            panelUserLine.Size = new Size(335, 2);
            panelUserLine.Location = new Point(30, 142);

            // --- Champ mot de passe ---
            iconLock.Text = "MDP";
            iconLock.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            iconLock.ForeColor = Color.FromArgb(30, 60, 114);
            iconLock.AutoSize = true;
            iconLock.Location = new Point(32, 177);

            tbmdp.Font = new Font("Segoe UI", 12F);
            tbmdp.Location = new Point(75, 175);
            tbmdp.Size = new Size(285, 28);
            tbmdp.BorderStyle = BorderStyle.None;
            tbmdp.BackColor = Color.White;
            tbmdp.PasswordChar = '*';
            tbmdp.PlaceholderText = "Mot de passe";
            tbmdp.TabIndex = 2;

            panelMdpLine.BackColor = Color.FromArgb(200, 210, 230);
            panelMdpLine.Size = new Size(335, 2);
            panelMdpLine.Location = new Point(30, 207);

            // cbShowPassword
            cbShowPassword.Text = "Afficher le mot de passe";
            cbShowPassword.Font = new Font("Segoe UI", 9F);
            cbShowPassword.ForeColor = Color.Gray;
            cbShowPassword.Location = new Point(65, 218);
            cbShowPassword.AutoSize = true;
            cbShowPassword.TabIndex = 3;
            cbShowPassword.FlatStyle = FlatStyle.Flat;
            cbShowPassword.CheckedChanged += cbShowPassword_CheckedChanged;

            // btnconnect
            btnconnect.Text = "Se connecter";
            btnconnect.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnconnect.Size = new Size(335, 50);
            btnconnect.Location = new Point(30, 270);
            btnconnect.BackColor = Color.FromArgb(30, 60, 114);
            btnconnect.ForeColor = Color.White;
            btnconnect.FlatStyle = FlatStyle.Flat;
            btnconnect.FlatAppearance.BorderSize = 0;
            btnconnect.Cursor = Cursors.Hand;
            btnconnect.TabIndex = 4;
            btnconnect.Click += btnconnect_Click;

            // lblError (message d'erreur inline)
            lblError.Text = "";
            lblError.Font = new Font("Segoe UI", 10F);
            lblError.ForeColor = Color.FromArgb(220, 50, 50);
            lblError.AutoSize = false;
            lblError.Size = new Size(335, 50);
            lblError.Location = new Point(30, 330);
            lblError.TextAlign = ContentAlignment.TopCenter;
            lblError.Visible = false;

            // =====================
            // Form1
            // =====================
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(980, 580);
            AcceptButton = btnconnect;
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            Font = new Font("Segoe UI", 9F);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Connexion � LegoFactory";
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(245, 247, 251);
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pblogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelLeft;
        private Panel panelRight;
        private Panel panelLoginBox;
        private PictureBox pblogo;
        private Label lblWelcome;
        private Label lblSubtitle;
        private Label ltitre;
        private Label lblLoginTitle;
        private Label iconUser;
        private Label iconLock;
        private TextBox tbutil;
        private TextBox tbmdp;
        private Panel panelUserLine;
        private Panel panelMdpLine;
        private CheckBox cbShowPassword;
        private Button btnconnect;
        private Label lblError;
    }
}
