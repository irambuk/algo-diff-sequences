using System.Collections.Generic;
using System.Linq;

namespace DiffSequencesAlgo
{
    public class ComparisonResult<T>
    {
        public bool IsMatching { get; private set; }
        public IEnumerable<T> SectionLeft { get; private set; }
        public IEnumerable<T> SectionRight { get; private set; }

        public ComparisonResult(IEnumerable<T> sectionLeft, IEnumerable<T> sectionRight)
        {
            IsMatching = AreSequencesEqual(sectionLeft, sectionRight);
            SectionLeft = sectionLeft;
            SectionRight = sectionRight;
        }

        private bool AreSequencesEqual(IEnumerable<T> sequenceLeft, IEnumerable<T> sequenceRight)
        {
            if (sequenceLeft == null || sequenceRight == null)
            {
                return false;
            }

            return (sequenceLeft.Count() == sequenceRight.Count()) && sequenceLeft.All(a => sequenceRight.Contains(a));
        }
    }
}
