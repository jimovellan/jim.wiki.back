using jim.wiki.back.core.Repository.Abstractions;

namespace jim.wiki.back.model.Models.Users
{
    public class User:LogicalEntity
    {
        public User()
        {
            RolesUser ??= new List<UserRole>();
        }
        
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public virtual ICollection<UserRole> RolesUser { get; set; }
    }
}
