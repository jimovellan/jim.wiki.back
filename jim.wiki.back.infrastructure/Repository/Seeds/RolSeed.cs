using jim.wiki.back.model.Models.Users;
using jim.wiki.back.model.Models.Users.Enums;
using jim.wiki.core.Utils;

namespace jim.wiki.back.infrastructure.Repository.Seeds
{
    public class RolSeed
    {
        public static IEnumerable<Rol> Data()
        {
            return EnumUtil.GetEnumData<RolEnum>().Select(s => new Rol()
            {
                Id = s.Id,
                Name = s.Name,
                CreateadAt = DateTime.UtcNow,
                CreatedBy = "_System",
                Guid = Guid.NewGuid(),
                ModifiedAt = DateTime.UtcNow,
                ModifiedBy = "_System",
                Description = s.Name,
                LastAction = s.Name,

            });
        }
    }
}
