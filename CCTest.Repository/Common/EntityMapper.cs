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

        #region Constructor
        public EntityMapper(IMapper mapper)
        {
            Configure();
            Create();
        }

        #endregion

        /// <summary>
        /// Configure mappings with entity to Dto
        /// </summary>
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

        /// <summary>
        /// Common map method
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TSource, TDestination>(source);
        }
    }
}
