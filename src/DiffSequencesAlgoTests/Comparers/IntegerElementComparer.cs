namespace DiffSequencesAlgo.Tests.Comparers
{
    public class IntegerElementComparer : IElementComparer<int>
    {
        public bool IsEqual(int itemLeft, int itemRight)
        {
            return itemLeft == itemRight;
        }
    }
}
