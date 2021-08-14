using NUnit.Framework;

namespace DiffSequencesAlgo.Tests
{
    [TestFixture]
    public class ComparisonResultTests
    {
        [Test]
        public void GivenNullCollection_WhenComparisionResultIsCreated_ThenObjectCreatedAccordingly()
        {
            var result = new ComparisonResult<string>(null, null);
            Assert.IsFalse(result.IsMatching);
            Assert.IsNull(result.SectionLeft);
            Assert.IsNull(result.SectionRight);
        }
    }
}
