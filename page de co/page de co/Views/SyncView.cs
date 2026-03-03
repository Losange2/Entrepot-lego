using System.Windows.Forms;
using System.Drawing;

namespace page_de_co
{
    public class SyncView : UserControl
    {
        public SyncView()
        {
            var title = new Label { Text = "Synchroniser l'outil de stock", Font = new Font("Segoe UI", 16F, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            
            var lblInfo = new Label 
            { 
                Text = "Cette fonctionnalité nécessite la configuration d'un outil de stock externe.\n\n" +
                       "Selon le cahier des charges (section 3.4), la synchronisation peut se faire via :\n" +
                       "• API REST (si l'outil externe expose une API)\n" +
                       "• Import/Export planifié (CSV/Excel)\n" +
                       "• Liaison par clé unique (code set)\n\n" +
                       "Configuration requise :\n" +
                       "1. URL de l'API ou chemin du fichier d'échange\n" +
                       "2. Identifiants d'accès (si API)\n" +
                       "3. Mapping des champs (Reference ↔ code unique)\n" +
                       "4. Fréquence de synchronisation",
                Location = new Point(20, 70),
                AutoSize = false,
                Width = 900,
                Height = 250,
                Font = new Font("Segoe UI", 10F)
            };
            
            var btnConfig = new Button { Text = "Configurer la synchronisation", Location = new Point(20, 340), Width = 220, Enabled = false };
            var btnSync = new Button { Text = "Lancer la synchronisation", Location = new Point(250, 340), Width = 200, Enabled = false };
            
            var lblStatus = new Label 
            { 
                Text = "⚠ Aucun outil de stock configuré. Utilisez Import/Export pour l'instant.",
                Location = new Point(20, 390),
                AutoSize = true,
                ForeColor = Color.DarkOrange,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic)
            };

            Controls.Add(title);
            Controls.Add(lblInfo);
            Controls.Add(btnConfig);
            Controls.Add(btnSync);
            Controls.Add(lblStatus);
        }
    }
}
