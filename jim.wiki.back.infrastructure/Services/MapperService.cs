using AutoMapper;
using jim.wiki.back.application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jim.wiki.back.infrastructure.Services
{
    public class MapperService : IMapperServices
    {
        private readonly IMapper _mapper;

        public MapperService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public TDestiny Map<TDestiny, TOrigin>(TOrigin origin)
        {
            throw new NotImplementedException();
        }

        public TDestiny Map<TDestiny>(object origin)
        {
            return _mapper.Map<TDestiny>(origin);
        }
    }
}
