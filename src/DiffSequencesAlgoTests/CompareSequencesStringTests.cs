using DiffSequencesAlgo.Tests.Comparers;
using DiffSequencesAlgo.Tests.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DiffSequencesAlgo.Tests
{
    [TestFixture]
    public class CompareSequencesStringTests
    {
        private ICompareSequences<string> _compareSequences;
        private IElementComparer<string> _elementComparer;

        [SetUp]
        public void TestInitialize()
        {
            _compareSequences = new CompareSequences<string>();
            _elementComparer = new StringElementComparer();
        }

        [Test]
        public void GivenEmptySequences_WhenCompare_ReturnsEmpty()
        {
            var sequenceLeft = new List<string>(); ;
            var sequenceRight = new List<string>();

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GivenLeftSequenceNotEmptyAndRightSequenceEmpty_WhenCompare_ReturnsSingleNotMatchingSection()
        {
            var sequenceLeft = new List<string>() { "test1", "test2" };
            var sequenceRight = new List<string>();

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(1, result.Count());
            AssertHelper.AssertComparisonSection<string>(result[0], false, new string[] { "test1", "test2" }, new string[] { });
        }

        [Test]
        public void GivenLeftSequenceEmptyAndRightSequenceNotEmpty_WhenCompare_ReturnsSingleNotMatchingSection()
        {
            var sequenceLeft = new List<string>();
            var sequenceRight = new List<string>() { "test1", "test2" };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToList();

            Assert.AreEqual(1, result.Count());
            AssertHelper.AssertComparisonSection<string>(result[0], false, new string[] { }, new string[] { "test1", "test2" });
        }

        [Test]
        public void GivenEqualSequences_WhenCompare_ReturnsSingleMatchingSection()
        {
            var sequenceLeft = new List<string>() { "test1", "test2", "test3" };
            var sequenceRight = new List<string>() { "test1", "test2", "test3" };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(1, result.Count());
            AssertHelper.AssertComparisonSection<string>(result[0], true, new string[] { "test1", "test2", "test3" }, new string[] { "test1", "test2", "test3" });
        }

        [Test]
        public void GivenTwoSequencesWithDifferenceAtTheStart_WhenCompare_Returns2MatchingSection()
        {
            var sequenceLeft = new List<string>() { "test1", "test2", "test3", "test4" };
            var sequenceRight = new List<string>() { "test5", "test6", "test3", "test4" };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(2, result.Count());
            AssertHelper.AssertComparisonSection<string>(result[0], false, new string[] { "test1", "test2" }, new string[] { "test5", "test6" });
            AssertHelper.AssertComparisonSection<string>(result[1], true, new string[] { "test3", "test4" }, new string[] { "test3", "test4" });
        }

        [Test]
        public void GivenTwoSequencesWithDifferenceAtTheMiddle_WhenCompare_Returns3MatchingSection()
        {
            var sequenceLeft = new List<string>() { "test1", "test2", "test3", "test4", "test5" };
            var sequenceRight = new List<string>() { "test1", "test2", "test6", "test7", "test5" };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(3, result.Count());
            AssertHelper.AssertComparisonSection<string>(result[0], true, new string[] { "test1", "test2" }, new string[] { "test1", "test2" });
            AssertHelper.AssertComparisonSection<string>(result[1], false, new string[] { "test3", "test4" }, new string[] { "test6", "test7" });
            AssertHelper.AssertComparisonSection<string>(result[2], true, new string[] { "test5" }, new string[] { "test5" });
        }

        [Test]
        public void GivenTwoSequencesWithDifferenceAtTheEnd_WhenCompare_Returns2MatchingSection()
        {
            var sequenceLeft = new List<string>() { "test1", "test2", "test3", "test4" };
            var sequenceRight = new List<string>() { "test1", "test2", "test5", "test6" };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(2, result.Count());
            AssertHelper.AssertComparisonSection<string>(result[0], true, new string[] { "test1", "test2" }, new string[] { "test1", "test2" });
            AssertHelper.AssertComparisonSection<string>(result[1], false, new string[] { "test3", "test4" }, new string[] { "test5", "test6" });
        }

        [Test]
        public void GivenTwoSequencesWithMultipleDifferences_WhenCompare_ReturnsMatchingSections()
        {
            var sequenceLeft = new List<string>() { "test1", "test2", "test3", "test4", "test5", "test6", "test7", "test8" };
            var sequenceRight = new List<string>() { "test1", "test2", "test31", "test41", "test5", "test61", "test7", "test8" };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(5, result.Count());
            AssertHelper.AssertComparisonSection<string>(result[0], true, new string[] { "test1", "test2" }, new string[] { "test1", "test2" });
            AssertHelper.AssertComparisonSection<string>(result[1], false, new string[] { "test3", "test4" }, new string[] { "test31", "test41" });
            AssertHelper.AssertComparisonSection<string>(result[2], true, new string[] { "test5" }, new string[] { "test5" });
            AssertHelper.AssertComparisonSection<string>(result[3], false, new string[] { "test6" }, new string[] { "test61" });
            AssertHelper.AssertComparisonSection<string>(result[4], true, new string[] { "test7", "test8" }, new string[] { "test7", "test8" });
        }

        [Test]
        public void GivenDifferentSizedTwoSequencesWithMultipleDifferences_WhenCompare_ReturnsMatchingSections()
        {
            var sequenceLeft = new List<string>() { "test1", "test2", "test3", "test4", "test5", "test6", "test7", "test8" };
            var sequenceRight = new List<string>() { "test1", "test21", "test3", "test41" };

            var result = _compareSequences.Compare(_elementComparer, sequenceLeft, sequenceRight).ToArray();

            Assert.AreEqual(4, result.Count());
            AssertHelper.AssertComparisonSection<string>(result[0], true, new string[] { "test1" }, new string[] { "test1" });
            AssertHelper.AssertComparisonSection<string>(result[1], false, new string[] { "test2" }, new string[] { "test21" });
            AssertHelper.AssertComparisonSection<string>(result[2], true, new string[] { "test3" }, new string[] { "test3" });
            AssertHelper.AssertComparisonSection<string>(result[3], false, new string[] { "test4", "test5", "test6", "test7", "test8" }, new string[] { "test41" });
        }
    }
}
