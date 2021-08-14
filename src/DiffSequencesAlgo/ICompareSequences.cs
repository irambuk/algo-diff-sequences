using System.Collections.Generic;

namespace DiffSequencesAlgo
{
    public interface ICompareSequences<T>
    {
        IEnumerable<ComparisonResult<T>> Compare(IElementComparer<T> comparer, IEnumerable<T> sequenceLeft, IEnumerable<T> sequenceRight);
    }
}
