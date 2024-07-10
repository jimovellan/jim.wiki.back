using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.application.Features.Users.Dto
{
    public class UserDto
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public ICollection<RolDto> Roles { get; set; }
    }
}
