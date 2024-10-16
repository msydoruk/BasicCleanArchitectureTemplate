using AutoMapper;
using AutoFixture;
using BasicCleanArchitectureTemplate.Web.Mapping;

namespace BasicCleanArchitectureTemplate.Tests.Base
{
    public class TestBase
    {
        protected Fixture Fixture { get; set; }

        protected IMapper Mapper { get; set; }

        public TestBase()
        {
            Fixture = new Fixture();

            Mapper = new Mapper(new MapperConfiguration(options =>
                options.AddProfiles(new List<Profile>
                {
                    new MappingProfile()
                })));
        }
    }
}
