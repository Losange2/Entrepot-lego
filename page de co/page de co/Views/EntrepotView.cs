using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace page_de_co
{
    public class EntrepotView : UserControl
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();
        private TreeView treeEntrepot;
        private DataGridView gridContenu;

        public EntrepotView()
        {
            var title = new Label { Text = "Consulter l'entrepôt", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            
            var lblTree = new Label { Text = "Structure:", Location = new Point(20, 60), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            treeEntrepot = new TreeView { Location = new Point(20, 90), Width = 400, Height = 570 };
            
            var lblContenu = new Label { Text = "Contenu de l'emplacement sélectionné:", Location = new Point(440, 60), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold) };
            gridContenu = new DataGridView { Location = new Point(440, 90), Width = 630, Height = 570, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

            Controls.Add(title);
            Controls.Add(lblTree);
            Controls.Add(treeEntrepot);
            Controls.Add(lblContenu);
            Controls.Add(gridContenu);

            treeEntrepot.AfterSelect += TreeEntrepot_AfterSelect;
            Load += EntrepotView_Load;
        }

        private void EntrepotView_Load(object? sender, System.EventArgs e)
        {
            LoadTreeStructure();
        }

        private void LoadTreeStructure()
        {
            try
            {
                treeEntrepot.Nodes.Clear();
                using var conn = _db.GetConnection();
                
                var rootNode = new TreeNode("Entrepôt LegoFactory");
                treeEntrepot.Nodes.Add(rootNode);
                
                using var cmdZones = new MySqlCommand("SELECT id, nom FROM Zone ORDER BY nom", conn);
                using var readerZones = cmdZones.ExecuteReader();
                var zones = new System.Collections.Generic.List<(int id, string nom)>();
                while (readerZones.Read()) zones.Add(((int)readerZones["id"], readerZones["nom"].ToString() ?? ""));
                readerZones.Close();
                
                foreach (var zone in zones)
                {
                    var zoneNode = new TreeNode(zone.nom) { Tag = $"Zone_{zone.id}" };
                    rootNode.Nodes.Add(zoneNode);
                    
                    using var cmdEmpl = new MySqlCommand("SELECT id, code FROM Emplacement WHERE zone_id = @zid ORDER BY code", conn);
                    cmdEmpl.Parameters.AddWithValue("@zid", zone.id);
                    using var readerEmpl = cmdEmpl.ExecuteReader();
                    while (readerEmpl.Read())
                    {
                        var emplNode = new TreeNode(readerEmpl["code"].ToString() ?? "") { Tag = $"Empl_{readerEmpl["id"]}" };
                        zoneNode.Nodes.Add(emplNode);
                    }
                }
                rootNode.Expand();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement structure: {ex.Message}");
            }
        }

        private void TreeEntrepot_AfterSelect(object? sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag == null) { gridContenu.DataSource = null; return; }
            var tag = e.Node.Tag.ToString() ?? "";
            if (tag.StartsWith("Empl_"))
            {
                int emplId = int.Parse(tag.Replace("Empl_", ""));
                LoadContenu(emplId);
            }
            else
            {
                gridContenu.DataSource = null;
            }
        }

        private void LoadContenu(int emplId)
        {
            try
            {
                using var conn = _db.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT ls.Reference AS Référence, ls.nom AS 'Nom Set', s.quantiter AS Quantité " +
                    "FROM stocker s " +
                    "JOIN LegoSet ls ON ls.id = s.legoset_id " +
                    "WHERE s.emplacement_id = @eid " +
                    "ORDER BY ls.Reference", conn);
                cmd.Parameters.AddWithValue("@eid", emplId);
                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                gridContenu.DataSource = table;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement contenu: {ex.Message}");
            }
        }
    }
}