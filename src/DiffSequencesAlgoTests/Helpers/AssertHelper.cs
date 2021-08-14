using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffSequencesAlgo.Tests.Helpers
{
    public static class AssertHelper
    {
        public static void AssertComparisonSection<T>(ComparisonResult<T> result, bool isMatching, T[] leftItems, T[] rightItems)
        {
            Assert.AreEqual(isMatching, result.IsMatching);

            Assert.AreEqual(leftItems.Length, result.SectionLeft.Count());
            Assert.IsTrue(result.SectionLeft.All(a => leftItems.Contains(a)));

            Assert.AreEqual(rightItems.Length, result.SectionRight.Count());
            Assert.IsTrue(result.SectionRight.All(a => rightItems.Contains(a)));
        }
    }
}
