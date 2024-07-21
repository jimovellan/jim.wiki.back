using jim.wiki.back.model.Models.Users;
using jim.wiki.back.model.Models.Users.Enums;
using jim.wiki.back.model.Services;

namespace jim.wiki.back.infrastructure.Repository.Seeds
{
    public static class UserSeed
    {
        public static IEnumerable<User> Data(IPasswordService passwordService)
        {

            var pass = passwordService.GenerateHash("123456");
               
            return new List<User>(){

                new User(){ Id = 1,
                            Name = "Admin",
                           CreateadAt = DateTime.UtcNow,
                           CreatedBy = "_Systenm",
                           LastAction = "Added",
                           ModifiedAt = DateTime.UtcNow,
                           ModifiedBy = "_System",
                           Hash = pass,
                           Email = "email@mail.com", 
                           RolId = (int)RolEnum.Admin},
            };
        }
    }
}
