using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.model.Dtos.Users
{
    public class UserInfoDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
