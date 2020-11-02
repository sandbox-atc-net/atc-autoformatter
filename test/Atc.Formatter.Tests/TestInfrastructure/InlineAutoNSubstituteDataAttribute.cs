using AutoFixture.Xunit2;

namespace Atc.Formatter.Tests.TestInfrastructure
{
    public class InlineAutoNSubstituteDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoNSubstituteDataAttribute(params object[] values)
            : base(new AutoNSubstituteDataAttribute(), values) { }
    }
}