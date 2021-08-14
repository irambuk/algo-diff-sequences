namespace DiffSequencesAlgo
{
    public interface IElementComparer<T>
    {
        bool IsEqual(T itemLeft, T itemRight);
    }
}
