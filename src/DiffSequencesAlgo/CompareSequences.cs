using System.Collections.Generic;
using System.Linq;

namespace DiffSequencesAlgo
{
    public class CompareSequences<T> : ICompareSequences<T>
    {
        public IEnumerable<ComparisonResult<T>> Compare(IElementComparer<T> comparer, IEnumerable<T> sequenceLeft, IEnumerable<T> sequenceRight)
        {
            if ((sequenceLeft == null || !sequenceLeft.Any()) && (sequenceRight == null || !sequenceRight.Any()))
            {
                return new List<ComparisonResult<T>>();
            }
            if ((sequenceLeft == null || !sequenceLeft.Any()) && (sequenceRight != null && sequenceRight.Any()))
            {
                return new List<ComparisonResult<T>> { new ComparisonResult<T>(new List<T>(), new List<T>(sequenceRight)) };
            }
            if ((sequenceLeft != null && sequenceLeft.Any()) && (sequenceRight == null || !sequenceRight.Any()))
            {
                return new List<ComparisonResult<T>> { new ComparisonResult<T>(new List<T>(sequenceLeft), new List<T>()) };
            }

            var leftArray = sequenceLeft.ToArray();
            var rightArray = sequenceRight.ToArray();

            var matchingItemsInfo = CalculateMatchingItems(comparer, leftArray, rightArray);

            return ProcessMatchingItems(leftArray, rightArray, matchingItemsInfo);
        }

        private bool[,] CalculateMatchingItems(IElementComparer<T> comparer, T[] sequenceLeft, T[] sequenceRight)
        {
            var matchingItemsInfo = new bool[sequenceLeft.Length, sequenceRight.Length];

            for (int i = 0; i < sequenceLeft.Length; i++)
            {
                for (int j = 0; j < sequenceRight.Length; j++)
                {
                    var left = sequenceLeft[i];
                    var right = sequenceRight[j];

                    var isMatching = comparer.IsEqual(left, right);
                    matchingItemsInfo[i, j] = isMatching;
                }
            }

            return matchingItemsInfo;
        }

        private IEnumerable<ComparisonResult<T>> ProcessMatchingItems(T[] sequenceLeft, T[] sequenceRight, bool[,] matchingItemsInfo)
        {
            var results = new List<ComparisonResult<T>>();

            var matchingLeftItems = new List<T>();
            var matchingRightItems = new List<T>();

            var notMatchingLeftItems = new List<T>();
            var lastMatchedRight = -1;

            for (int i = 0; i < sequenceLeft.Length; i++)
            {
                var left = sequenceLeft[i];

                var isNextItemMatch = matchingItemsInfo[i, lastMatchedRight + 1];
                if (isNextItemMatch)
                {
                    if (notMatchingLeftItems.Any())
                    {
                        var result = OnNonMatchingSectionEnds(notMatchingLeftItems, sequenceRight, lastMatchedRight, lastMatchedRight);
                        results.Add(result);
                    }

                    lastMatchedRight++;
                    OnMatchingSectionContinues(matchingLeftItems, matchingRightItems, left, sequenceRight[lastMatchedRight]);

                    continue;
                }

                if (matchingLeftItems.Count > 0)
                {
                    var result = OnMatchingSectionEnds(matchingLeftItems, matchingRightItems);
                    results.Add(result);
                }

                var tempLastMatchedRight = -1;
                for (int j = lastMatchedRight + 1; j < sequenceRight.Length; j++)
                {
                    var isMatch = matchingItemsInfo[i, j];
                    if (isMatch)
                    {
                        tempLastMatchedRight = j;
                        break;
                    }
                }
                if (tempLastMatchedRight > -1)
                {
                    var result = OnNonMatchingSectionEnds(notMatchingLeftItems, sequenceRight, lastMatchedRight, tempLastMatchedRight);
                    results.Add(result);
                    lastMatchedRight = tempLastMatchedRight;

                    OnMatchingSectionContinues(matchingLeftItems, matchingRightItems, left, sequenceRight[lastMatchedRight]);
                    continue;
                }

                notMatchingLeftItems.Add(left);
            }

            if (matchingLeftItems.Count > 0)
            {
                var result = OnMatchingSectionEnds(matchingLeftItems, matchingRightItems);
                results.Add(result);
            }

            if (notMatchingLeftItems.Count > 0)
            {
                var result = OnNonMatchingSectionEnds(notMatchingLeftItems, sequenceRight, lastMatchedRight, sequenceRight.Length);
                results.Add(result);
            }

            return results;
        }

        private void OnMatchingSectionContinues(List<T> matchingLeft, List<T> matchingRight, T left, T right)
        {
            matchingLeft.Add(left);
            matchingRight.Add(right);
        }

        private ComparisonResult<T> OnMatchingSectionEnds(List<T> matchingLeft, List<T> matchingRight)
        {
            var result = new ComparisonResult<T>(
                           new List<T>(matchingLeft),
                           new List<T>(matchingRight));
            matchingLeft.Clear();
            matchingRight.Clear();
            return result;
        }

        private ComparisonResult<T> OnNonMatchingSectionEnds(List<T> notMatchingLeft, T[] sequenceRight, int previousMatchedRight, int newMatchedRight)
        {
            var result =
                        new ComparisonResult<T>(
                            new List<T>(notMatchingLeft),
                            new List<T>(sequenceRight.Skip(previousMatchedRight + 1).Take(newMatchedRight - previousMatchedRight - 1)));
            notMatchingLeft.Clear();
            return result;
        }
    }
}
