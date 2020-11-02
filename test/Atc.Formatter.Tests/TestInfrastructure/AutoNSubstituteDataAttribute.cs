using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace Atc.Formatter.Tests.TestInfrastructure
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        public AutoNSubstituteDataAttribute()
            : base(CreateCustomizedFixture)
        { }

        private static IFixture CreateCustomizedFixture()
            => new Fixture().Customize(new AutoNSubstituteCustomization());
    }
}