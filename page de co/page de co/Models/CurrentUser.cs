namespace page_de_co
{
    public enum UserRole
    {
        Employe = 1,        // Employé logistique : lecture seule
        Responsable = 2,    // Responsable entrepôt : modif emplacements/sets
        Admin = 3           // Admin IT/Direction : tous droits
    }

    public class CurrentUser
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Employe;

        public static CurrentUser? Instance { get; set; }

        public bool CanEdit => Role >= UserRole.Responsable;
        public bool CanDelete => Role >= UserRole.Responsable;
        public bool CanManageUsers => Role == UserRole.Admin;
    }
}
