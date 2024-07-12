using jim.wiki.back.core.Repository.Abstractions;
using jim.wiki.back.model.Models.ObjectsValue;
using jim.wiki.back.model.Services;
using jim.wiki.core.Repository.Interfaces;

namespace jim.wiki.back.model.Models.Users
{
    public class User:AggregateLogical
    {
        private User()
        {
            RolesUser ??= new List<UserRole>();
        }
        
        public Guid Guid { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Hash { get; private set; }

        public virtual ICollection<UserRole> RolesUser { get; private set; }
        public virtual ICollection<UserToken> Tokens { get; set; }


        public static User Create(string name, Email email, string hash)
        {
            
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(hash);

            var user = new User();
            user.Guid = Guid.NewGuid();
            user.Email = email.Value;
            user.Hash = hash;

            return user; 

        }

        public void UpdateEmail(Email email)
        {
            this.Email = email.Value;
        }

        public void UpdatePassword(string hash)
        {
            this.Hash = hash;
        }

        public bool IsRefreshTokenValid(string token, string refreshToken)
        {
            var userToken = Tokens.Where(x => x.IsActive && x.ValidAt >= DateTime.UtcNow).LastOrDefault();

            if (userToken is null) return false;

            if(userToken.Token != token || userToken.RefreshToken != refreshToken) return false;

            return true;

        }

        public string GenerateNewRefreshToken(string token)
        {
            
            foreach (var t in Tokens)
            {
                t.IsActive = false;
            }
            var refresToken = Guid.NewGuid().ToString();

            var newToken = new UserToken() { IsActive = true, 
                                Token = token,
                              ValidAt = DateTime.UtcNow.AddHours(24),
                              RefreshToken = refresToken
                              
                                };

            Tokens.Add(newToken);

            return refresToken;
        }


    }
}
