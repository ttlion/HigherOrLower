namespace HigherOrLower.Utils.Wrappers
{
    public class ResultWithStatus<T, U> : IResultWithStatus<T, U> where T : new()
    {
        public bool IsError { get; }

        public T Result { get; }

        public U Status { get; }

        private ResultWithStatus(bool isError, T result, U status)
        {
            IsError = isError;
            Result = result;
            Status = status;
        }

        public static ResultWithStatus<T, U> Error(U errorStatus)
        {
            return new ResultWithStatus<T, U>(true, new T(), errorStatus);
        }

        public static ResultWithStatus<T, U> Success(T result, U status)
        {
            return new ResultWithStatus<T, U>(false, result, status);
        }
    }
}
