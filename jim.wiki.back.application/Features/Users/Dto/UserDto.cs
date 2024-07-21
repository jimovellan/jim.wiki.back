using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.application.Features.Users.Dto
{
    public record UserDto(long Id, Guid Guid, string Name, string Email, long RolId, string RolName);
}
