using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;

namespace LegoFactory
{
    public class EntrepotView : UserControl
    {
        private static readonly Color PrimaryColor = Color.FromArgb(30, 60, 114);
        private static readonly Color SecondaryColor = Color.FromArgb(99, 102, 241);
        private static readonly Color AccentColor = Color.FromArgb(59, 130, 246);
        private static readonly Color BgColor = Color.FromArgb(245, 247, 251);
        private static readonly Color CardBgColor = Color.White;
        private static readonly Color SuccessColor = Color.FromArgb(16, 185, 129);
        private static readonly Color WarningColor = Color.FromArgb(245, 158, 11);
        private static readonly Color DangerColor = Color.FromArgb(239, 68, 68);

        private readonly DatabaseConnection _db = new DatabaseConnection();
        private TreeView treeEntrepot;
        private DataGridView gridContenu;
        private TextBox searchBox;
        private Label lblTotalZones;
        private Label lblTotalEmplacements;
        private Label lblTotalSets;

        public EntrepotView()
        {
            BackColor = BgColor;
            Dock = DockStyle.Fill;
            Padding = new Padding(24);
            AutoScroll = true;

            // Header simple
            var panelHeader = CreateHeaderPanel();

            // Panneau de statistiques
            var statsPanel = CreateStatsPanel();

            // Conteneur principal (SplitContainer pour responsive)
            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 300,
                SplitterWidth = 8,
                BackColor = BgColor,
                BorderStyle = BorderStyle.None,
                Orientation = Orientation.Horizontal,
                IsSplitterFixed = false
            };

            // Ajuster dynamiquement le splitter pour un ratio équilibré 50/50
            bool userHasMovedSplitter = false;
            split.SplitterMoved += (s, ev) => { userHasMovedSplitter = true; /* L'utilisateur a ajusté manuellement */ };
            SizeChanged += (s, ev) =>
            {
                // Ne pas ajuster automatiquement si l'utilisateur a déjà déplacé le splitter
                if (!userHasMovedSplitter && split.Height > 100)
                {
                    // 50% pour chaque partie (minimum 200px pour le TreeView)
                    int calculatedDistance = (int)(split.Height * 0.50);
                    calculatedDistance = Math.Max(200, calculatedDistance);
                    if (Math.Abs(split.SplitterDistance - calculatedDistance) > 20)
                    {
                        split.SplitterDistance = calculatedDistance;
                    }
                }
            };

            // Partie haute - TreeView avec recherche
            var topContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BgColor,
                Padding = new Padding(0, 0, 0, 10)
            };

            var leftCard = CreateCardPanel();
            leftCard.Dock = DockStyle.Fill;

            // En-tête de la structure
            var headerTree = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = CardBgColor,
                Padding = new Padding(20, 15, 20, 15)
            };

            var lblTree = new Label
            {
                Text = "🏗️ Structure de l'Entrepôt",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Dock = DockStyle.Top,
                Height = 35
            };

            // Barre de recherche
            searchBox = new TextBox
            {
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 11F),
                Height = 38,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(243, 244, 246),
                ForeColor = Color.FromArgb(55, 65, 81)
            };
            searchBox.Text = "🔍 Rechercher une zone ou un emplacement...";
            searchBox.ForeColor = Color.Gray;
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "🔍 Rechercher une zone ou un emplacement...")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = Color.FromArgb(55, 65, 81);
                }
            };
            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "🔍 Rechercher une zone ou un emplacement...";
                    searchBox.ForeColor = Color.Gray;
                }
            };
            searchBox.TextChanged += SearchBox_TextChanged;

            headerTree.Controls.Add(searchBox);
            headerTree.Controls.Add(lblTree);

            // TreeView avec style moderne
            treeEntrepot = new TreeView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F),
                BorderStyle = BorderStyle.None,
                BackColor = CardBgColor,
                ItemHeight = 32,
                Indent = 25,
                ShowLines = true,
                ShowPlusMinus = true,
                FullRowSelect = true,
                HotTracking = true
            };

            leftCard.Controls.Add(treeEntrepot);
            leftCard.Controls.Add(headerTree);
            topContainer.Controls.Add(leftCard);
            split.Panel1.Controls.Add(topContainer);
            split.Panel1.BackColor = BgColor;

            // Partie basse - DataGridView avec en-tête amélioré
            var bottomContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = BgColor,
                Padding = new Padding(0, 10, 0, 0)
            };

            var rightCard = CreateCardPanel();
            rightCard.Dock = DockStyle.Fill;

            // En-tête du contenu
            var headerContenu = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = CardBgColor,
                Padding = new Padding(20, 15, 20, 10)
            };

            var lblContenu = new Label
            {
                Text = "📋 Contenu de l'emplacement",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                Dock = DockStyle.Top,
                Height = 35
            };
            headerContenu.Controls.Add(lblContenu);

            // DataGridView moderne
            gridContenu = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                BackgroundColor = CardBgColor,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowTemplate = { Height = 45 }
            };
            StyleGrid(gridContenu);

            rightCard.Controls.Add(gridContenu);
            rightCard.Controls.Add(headerContenu);
            bottomContainer.Controls.Add(rightCard);
            split.Panel2.Controls.Add(bottomContainer);
            split.Panel2.BackColor = BgColor;

            Controls.Add(split);
            Controls.Add(statsPanel);
            Controls.Add(panelHeader);

            treeEntrepot.AfterSelect += TreeEntrepot_AfterSelect;
            Load += EntrepotView_Load;
        }

        private Panel CreateHeaderPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = BgColor
            };

            var title = new Label
            {
                Text = "📦  Consulter l'Entrepôt",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = PrimaryColor,
                AutoSize = true,
                Location = new Point(0, 8)
            };

            panel.Controls.Add(title);
            return panel;
        }

        private Panel CreateStatsPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 125,
                BackColor = BgColor,
                Padding = new Padding(0, 15, 0, 15)
            };

            var statsContainer = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                BackColor = BgColor
            };
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            statsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));

            // Stat 1 - Zones
            var stat1 = CreateStatCard("🗺️ Total Zones", "0", SuccessColor);
            stat1.Dock = DockStyle.Fill;
            stat1.Margin = new Padding(0, 0, 8, 0);
            lblTotalZones = stat1.Controls[1] as Label;

            // Stat 2 - Emplacements
            var stat2 = CreateStatCard("📍 Total Emplacements", "0", AccentColor);
            stat2.Dock = DockStyle.Fill;
            stat2.Margin = new Padding(4, 0, 4, 0);
            lblTotalEmplacements = stat2.Controls[1] as Label;

            // Stat 3 - Sets
            var stat3 = CreateStatCard("🎁 Total Sets Stockés", "0", WarningColor);
            stat3.Dock = DockStyle.Fill;
            stat3.Margin = new Padding(8, 0, 0, 0);
            lblTotalSets = stat3.Controls[1] as Label;

            statsContainer.Controls.Add(stat1, 0, 0);
            statsContainer.Controls.Add(stat2, 1, 0);
            statsContainer.Controls.Add(stat3, 2, 0);

            panel.Controls.Add(statsContainer);
            return panel;
        }

        private Panel CreateStatCard(string title, string value, Color accentColor)
        {
            var card = new Panel
            {
                Height = 90,
                BackColor = CardBgColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(18)
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(75, 85, 99),
                AutoSize = true,
                Location = new Point(18, 15),
                BackColor = Color.Transparent
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = accentColor,
                AutoSize = true,
                Location = new Point(18, 42),
                BackColor = Color.Transparent
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            return card;
        }

        private Panel CreateCardPanel()
        {
            var card = new Panel
            {
                BackColor = CardBgColor,
                BorderStyle = BorderStyle.FixedSingle
            };
            return card;
        }

        private void SearchBox_TextChanged(object? sender, System.EventArgs e)
        {
            if (searchBox.Text == "🔍 Rechercher une zone ou un emplacement..." || string.IsNullOrWhiteSpace(searchBox.Text))
            {
                ExpandAllNodes(treeEntrepot.Nodes);
                return;
            }

            CollapseAllNodes(treeEntrepot.Nodes);
            FilterNodes(treeEntrepot.Nodes, searchBox.Text.ToLower());
        }

        private void FilterNodes(TreeNodeCollection nodes, string searchText)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Text.ToLower().Contains(searchText))
                {
                    node.BackColor = Color.FromArgb(254, 249, 195);
                    node.ForeColor = Color.FromArgb(161, 98, 7);
                    ExpandParentNodes(node);
                }
                else
                {
                    node.BackColor = Color.White;
                    node.ForeColor = Color.Black;
                }

                if (node.Nodes.Count > 0)
                    FilterNodes(node.Nodes, searchText);
            }
        }

        private void ExpandParentNodes(TreeNode node)
        {
            if (node.Parent != null)
            {
                node.Parent.Expand();
                ExpandParentNodes(node.Parent);
            }
        }

        private void CollapseAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Collapse();
                if (node.Nodes.Count > 0)
                    CollapseAllNodes(node.Nodes);
            }
        }

        private void ExpandAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.Expand();
                node.BackColor = Color.White;
                node.ForeColor = Color.Black;
                if (node.Nodes.Count > 0)
                    ExpandAllNodes(node.Nodes);
            }
        }


        private void EntrepotView_Load(object? sender, System.EventArgs e)
        {
            LoadTreeStructure();
            LoadStats();
        }

        private void LoadStats()
        {
            try
            {
                using var conn = _db.GetConnection();

                // Total zones
                using var cmdZones = new MySqlCommand("SELECT COUNT(*) FROM Zone", conn);
                lblTotalZones.Text = cmdZones.ExecuteScalar()?.ToString() ?? "0";

                // Total emplacements
                using var cmdEmpl = new MySqlCommand("SELECT COUNT(*) FROM Emplacement", conn);
                lblTotalEmplacements.Text = cmdEmpl.ExecuteScalar()?.ToString() ?? "0";

                // Total sets stockés
                using var cmdSets = new MySqlCommand("SELECT COALESCE(SUM(quantiter), 0) FROM stocker", conn);
                lblTotalSets.Text = cmdSets.ExecuteScalar()?.ToString() ?? "0";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement statistiques: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void LoadTreeStructure()
        {
            try
            {
                treeEntrepot.Nodes.Clear();
                using var conn = _db.GetConnection();

                var rootNode = new TreeNode("🏭 Entrepôt LegoFactory")
                {
                    NodeFont = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = PrimaryColor
                };
                treeEntrepot.Nodes.Add(rootNode);

                using var cmdZones = new MySqlCommand("SELECT id, nom FROM Zone ORDER BY nom", conn);
                using var readerZones = cmdZones.ExecuteReader();
                var zones = new System.Collections.Generic.List<(int id, string nom)>();
                while (readerZones.Read()) zones.Add(((int)readerZones["id"], readerZones["nom"].ToString() ?? ""));
                readerZones.Close();

                foreach (var zone in zones)
                {
                    // Compter les emplacements dans cette zone
                    using var cmdCount = new MySqlCommand("SELECT COUNT(*) FROM Emplacement WHERE zone_id = @zid", conn);
                    cmdCount.Parameters.AddWithValue("@zid", zone.id);
                    var countEmpl = Convert.ToInt32(cmdCount.ExecuteScalar());

                    var zoneNode = new TreeNode($"🗺️ {zone.nom} ({countEmpl} emplacements)")
                    {
                        Tag = $"Zone_{zone.id}",
                        NodeFont = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                        ForeColor = SecondaryColor
                    };
                    rootNode.Nodes.Add(zoneNode);

                    using var cmdEmpl = new MySqlCommand(
                        "SELECT e.id, e.code, COALESCE(SUM(s.quantiter), 0) as total " +
                        "FROM Emplacement e " +
                        "LEFT JOIN stocker s ON e.id = s.emplacement_id " +
                        "WHERE e.zone_id = @zid " +
                        "GROUP BY e.id, e.code " +
                        "ORDER BY e.code", conn);
                    cmdEmpl.Parameters.AddWithValue("@zid", zone.id);
                    using var readerEmpl = cmdEmpl.ExecuteReader();
                    while (readerEmpl.Read())
                    {
                        var total = Convert.ToInt32(readerEmpl["total"]);
                        var icon = total > 0 ? "📦" : "📭";
                        var emplNode = new TreeNode($"{icon} {readerEmpl["code"]} ({total} sets)")
                        {
                            Tag = $"Empl_{readerEmpl["id"]}",
                            ForeColor = total > 0 ? SuccessColor : Color.FromArgb(156, 163, 175)
                        };
                        zoneNode.Nodes.Add(emplNode);
                    }
                }
                rootNode.Expand();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement structure: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void TreeEntrepot_AfterSelect(object? sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag == null)
            {
                gridContenu.DataSource = null;
                return;
            }
            var tag = e.Node.Tag.ToString() ?? "";
            if (tag.StartsWith("Empl_"))
            {
                int emplId = int.Parse(tag.Replace("Empl_", ""));
                LoadContenu(emplId);
            }
            else if (tag.StartsWith("Zone_"))
            {
                // Afficher tous les sets de la zone
                int zoneId = int.Parse(tag.Replace("Zone_", ""));
                LoadZoneContenu(zoneId);
            }
            else
            {
                gridContenu.DataSource = null;
            }
        }

        private void LoadZoneContenu(int zoneId)
        {
            try
            {
                using var conn = _db.GetConnection();
                using var cmd = new MySqlCommand(
                    "SELECT e.code AS Emplacement, ls.Reference AS Référence, ls.nom AS 'Nom Set', s.quantiter AS Quantité " +
                    "FROM stocker s " +
                    "JOIN LegoSet ls ON ls.id = s.legoset_id " +
                    "JOIN Emplacement e ON e.id = s.emplacement_id " +
                    "WHERE e.zone_id = @zid " +
                    "ORDER BY e.code, ls.Reference", conn);
                cmd.Parameters.AddWithValue("@zid", zoneId);
                using var reader = cmd.ExecuteReader();
                var table = new System.Data.DataTable();
                table.Load(reader);
                gridContenu.DataSource = table;

                // Ajuster automatiquement le splitter pour les grandes listes
                AdjustSplitterForContent(table.Rows.Count);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement contenu zone: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                // Mettre à jour la couleur des cellules selon la quantité
                gridContenu.CellFormatting -= Grid_CellFormatting;
                gridContenu.CellFormatting += Grid_CellFormatting;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Erreur chargement contenu: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Grid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (gridContenu.Columns[e.ColumnIndex].Name == "Quantité" && e.Value != null)
            {
                if (int.TryParse(e.Value.ToString(), out int quantity))
                {
                    if (quantity > 100)
                        e.CellStyle.ForeColor = SuccessColor;
                    else if (quantity > 50)
                        e.CellStyle.ForeColor = AccentColor;
                    else if (quantity > 0)
                        e.CellStyle.ForeColor = WarningColor;
                    else
                        e.CellStyle.ForeColor = DangerColor;

                    e.CellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                }
            }
        }

        private void AdjustSplitterForContent(int rowCount)
        {
            // Trouver le SplitContainer parent
            var split = gridContenu.Parent?.Parent?.Parent?.Parent as SplitContainer;
            if (split == null) return;

            if (rowCount > 10)
            {
                // Beaucoup de contenu : donner 70% au tableau
                int newDistance = (int)(split.Height * 0.30);
                split.SplitterDistance = Math.Max(180, newDistance);
            }
            else if (rowCount > 5)
            {
                // Contenu moyen : donner 60% au tableau
                int newDistance = (int)(split.Height * 0.40);
                split.SplitterDistance = Math.Max(200, newDistance);
            }
            else
            {
                // Peu de contenu : ratio équilibré 50/50
                int newDistance = (int)(split.Height * 0.50);
                split.SplitterDistance = Math.Max(200, newDistance);
            }
        }



        private static void StyleGrid(DataGridView g)
        {
            g.EnableHeadersVisualStyles = false;

            // En-têtes avec gradient moderne
            g.ColumnHeadersDefaultCellStyle.BackColor = PrimaryColor;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            g.ColumnHeadersHeight = 48;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Style des cellules
            g.DefaultCellStyle.Font = new Font("Segoe UI", 10.5F);
            g.DefaultCellStyle.Padding = new Padding(10, 8, 10, 8);
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            g.DefaultCellStyle.SelectionForeColor = Color.FromArgb(30, 64, 175);
            g.DefaultCellStyle.BackColor = CardBgColor;
            g.DefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);

            // Lignes alternées avec couleur subtile
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 250, 251);

            // Bordures et grille moderne
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.GridColor = Color.FromArgb(229, 231, 235);

            // Désactiver les bordures 3D
            g.BorderStyle = BorderStyle.None;
        }
    }
}
