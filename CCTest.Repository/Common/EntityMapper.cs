using AutoMapper;
using CCTest.Common.DTO;
using CCTest.Database.Entities;
using CCTest.Repository.Contracts;

namespace CCTest.Repository.Common
{
    public class EntityMapper : IEntityMapper
    {
        private MapperConfiguration? _config;

        private IMapper? _mapper;

        public EntityMapper(IMapper mapper)
        {
            Configure();
            Create();
        }

        private void Configure()
        {
            _config ??= new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Team, TeamDto>().ReverseMap();
                    cfg.CreateMap<Agent, AgentDto>().ReverseMap();
                });
        }

        private void Create()
        {
            if (_mapper == null && _config != null)
            {
                _mapper = _config.CreateMapper();
            }
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
