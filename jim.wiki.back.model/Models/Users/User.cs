using jim.wiki.back.core.Repository.Abstractions;
using jim.wiki.back.model.Models.ObjectsValue;
using jim.wiki.back.model.Services;
using jim.wiki.core.Repository.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace jim.wiki.back.model.Models.Users
{
    public class User:AggregateLogical
    {
        internal User()
        {
           
        }
        
        public Guid Guid { get; internal set; }

        public string Name { get; internal set; }

        public string Email { get; internal set; }

        public string Hash { get; internal set; }

        public long RolId { get; internal set; }

        
        public virtual Rol Rol { get; internal set; }
        public virtual ICollection<UserToken> Tokens { get; set; }


        public static User Create(string name, Email email, string hash, int rolId)
        {
            
            ArgumentNullException.ThrowIfNullOrWhiteSpace(name);
            ArgumentNullException.ThrowIfNullOrWhiteSpace(hash);

            var user = new User();
            user.Guid = Guid.NewGuid();
            user.Email = email.Value;
            user.Hash = hash;
            user.Name = name;
            user.RolId = rolId;

            return user;

        }

        public void ChangeRol(int rolId)
        {
            this.RolId = rolId;
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
