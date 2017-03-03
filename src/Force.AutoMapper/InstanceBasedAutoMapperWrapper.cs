using System.Linq;
using AutoMapper.QueryableExtensions;
using Force.Common;
using AM = AutoMapper;

namespace Force.AutoMapper
{
    public class InstanceBasedAutoMapperWrapper : Force.Common.IMapper, IProjector
    {
        public AM.IConfigurationProvider Configuration { get; private set; }
        public AM.IMapper Instance { get; private set; }

        public InstanceBasedAutoMapperWrapper(AM.IConfigurationProvider configuration, bool skipValidnessAssertation = false)
        {
            Configuration = configuration;

            if (!skipValidnessAssertation)
            {
                configuration.AssertConfigurationIsValid();
            }

            Instance = configuration.CreateMapper();
        }

        public TReturn Map<TReturn>(object src) => Instance.Map<TReturn>(src);

        public TReturn Map<TReturn>(object src, TReturn dest) => Instance.Map(src, dest);

        public IQueryable<TReturn> Project<TReturn>(IQueryable queryable)
            => queryable.ProjectTo<TReturn>(Configuration);
    }
}
