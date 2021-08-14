using DiffSequencesAlgo.Tests.Comparers;
using DiffSequencesAlgo.Tests.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DiffSequencesAlgo.Tests
{
    [TestFixture]
    public class CompareSequencesIntegerTests
    {
        private ICompareSequences<int> _compareSequences;
        private IElementComparer<int> _elementComparer;

        [SetUp]
        public void TestInitialize()
        {
            _compareSequences = new CompareSequences<int>();
            _elementComparer = new IntegerElementComparer();
        }

        [Test]
        public void GivenNumberSequence_WhenCompare_ThenReturnsComparedSections()
        {
            var sequenceLeft = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var sequenceRight = new List<int>() { 1, 5, 16, 17, 18, 19 };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(4, result.Count());
            AssertHelper.AssertComparisonSection<int>(result[0], true, new int[] { 1 }, new int[] { 1 });
            AssertHelper.AssertComparisonSection<int>(result[1], false, new int[] { 2, 3, 4 }, new int[] { });
            AssertHelper.AssertComparisonSection<int>(result[2], true, new int[] { 5 }, new int[] { 5 });
            AssertHelper.AssertComparisonSection<int>(result[3], false, new int[] { 6, 7, 8, 9, 10 }, new int[] { 16, 17, 18, 19 });
        }

        [Test]
        public void GivenNumberSequence2_WhenCompare_ThenReturnsComparedSections()
        {
            var sequenceLeft = new List<int>() { 1, 5, 16, 17, 18, 19 };
            var sequenceRight = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(4, result.Count());
            AssertHelper.AssertComparisonSection<int>(result[0], true, new int[] { 1 }, new int[] { 1 });
            AssertHelper.AssertComparisonSection<int>(result[1], false, new int[] { }, new int[] { 2, 3, 4 });
            AssertHelper.AssertComparisonSection<int>(result[2], true, new int[] { 5 }, new int[] { 5 });
            AssertHelper.AssertComparisonSection<int>(result[3], false, new int[] { 16, 17, 18, 19 }, new int[] { 6, 7, 8, 9, 10 });
        }
    }
}
