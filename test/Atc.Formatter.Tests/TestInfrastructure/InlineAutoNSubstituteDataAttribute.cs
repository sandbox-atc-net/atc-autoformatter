using AutoFixture.Xunit2;

namespace Atc.AutoFormatter.Tests.TestInfrastructure
{
    public class InlineAutoNSubstituteDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoNSubstituteDataAttribute(params object[] values)
            : base(new AutoNSubstituteDataAttribute(), values) { }
    }
}