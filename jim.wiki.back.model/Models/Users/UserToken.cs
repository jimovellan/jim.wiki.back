using jim.wiki.back.core.Repository.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.model.Models.Users
{
    public class UserToken
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidAt { get; set; }

        #region Navigation Properties
        public virtual User User { get; set; }
        #endregion

    }
}
