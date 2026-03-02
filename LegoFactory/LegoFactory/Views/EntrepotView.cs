using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class EntrepotView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);

        private readonly DatabaseConnection _db = new DatabaseConnection();
        private TreeView treeEntrepot;
        private DataGridView gridContenu;

        public EntrepotView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);

            // Header
            var panelHeader = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = BgColor };
            var title = new Label
            {
                Text = "📦  Consulter l'entrepôt",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            panelHeader.Controls.Add(title);

            // Conteneur principal (SplitContainer pour responsive)
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 320,
                SplitterWidth = 6,
                BackColor = BgColor,
                BorderStyle = BorderStyle.None
            };

            // Panel gauche — Tree
            var lblTree = new Label
            {
                Text = "Structure",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Dock = DockStyle.Top,
                Height = 30
            };
            treeEntrepot = new TreeView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ItemHeight = 26
            };
            split.Panel1.BackColor = Color.White;
            split.Panel1.Padding = new Padding(10);
            split.Panel1.Controls.Add(treeEntrepot);
            split.Panel1.Controls.Add(lblTree);

            // Panel droit — Grid
            var lblContenu = new Label
            {
                Text = "Contenu de l'emplacement sélectionné",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Dock = DockStyle.Top,
                Height = 30
            };
            gridContenu = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                RowHeadersVisible = false
            };
            StyleGrid(gridContenu);
            split.Panel2.BackColor = Color.White;
            split.Panel2.Padding = new Padding(10);
            split.Panel2.Controls.Add(gridContenu);
            split.Panel2.Controls.Add(lblContenu);

            Controls.Add(split);
            Controls.Add(panelHeader);

            treeEntrepot.AfterSelect += TreeEntrepot_AfterSelect;
            Load += EntrepotView_Load;
        }

        private void EntrepotView_Load(object? sender, System.EventArgs e) => LoadTreeStructure();

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
            else gridContenu.DataSource = null;
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

        private static void StyleGrid(DataGridView g)
        {
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 60, 114);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6);
            g.ColumnHeadersHeight = 38;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            g.DefaultCellStyle.Padding = new Padding(4);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 220, 255);
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 253);
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.GridColor = Color.FromArgb(230, 235, 242);
        }
    }
}