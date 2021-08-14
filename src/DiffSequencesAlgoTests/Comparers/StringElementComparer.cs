namespace DiffSequencesAlgo.Tests.Comparers
{
    public class StringElementComparer : IElementComparer<string>
    {
        public bool IsEqual(string itemLeft, string itemRight)
        {
            return string.Equals(itemLeft, itemRight);
        }
    }
}
