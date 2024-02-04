namespace HigherOrLower.Utils.Wrappers
{
    public interface IResultWithStatus<T, U> where T : new()
    {
        bool IsError { get; }

        T Result { get; }
        
        U Status { get; }
    }
}