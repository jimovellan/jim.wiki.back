using jim.wiki.back.infrastructure.Repository.Seeds;
using jim.wiki.back.model.Models.Users;
using jim.wiki.back.model.Services;
using jim.wiki.core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace jim.wiki.back.infrastructure.Repository
{
    public class DataBaseSeeder : IDataBaseSeeder<ModelBuilder>
    {
        private readonly IPasswordService passwordService;

        public DataBaseSeeder(IPasswordService passwordService)
        {
            this.passwordService = passwordService;
        }
        public Task SeedData(ModelBuilder seeder)
        {

            seeder.Entity<Rol>().HasData(RolSeed.Data());
            seeder.Entity<User>().HasData(UserSeed.Data(passwordService));
            return Task.CompletedTask;
        }
    }
}
