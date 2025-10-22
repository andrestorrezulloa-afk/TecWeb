namespace TecWeb.Core.Services
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }

        private ServiceResult() { }

        public static ServiceResult<T> Success(T? data, string? message = null)
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data, Message = message };
        }

        public static ServiceResult<T> Failure(string message)
        {
            return new ServiceResult<T> { IsSuccess = false, Message = message, Data = default };
        }
    }
}
