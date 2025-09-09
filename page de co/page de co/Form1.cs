namespace page_de_co
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ltitre_Click(object sender, EventArgs e)
        {

        }

        private void pblogo_Click(object sender, EventArgs e)
        {

        }

        private void tbutil_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnconnect_Click(object sender, EventArgs e)
        {
            if (tbutil.Text == "admin" && tbmdp.Text == "admin")
            {
                MessageBox.Show("Connexion réussie !");
                    Form2 form2 = new Form2();
                    form2.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
            }
        }
    }
}
