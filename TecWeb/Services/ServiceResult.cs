﻿namespace TecWeb.Services
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static ServiceResult<T> Success(T data, string message = "") =>
            new ServiceResult<T> { IsSuccess = true, Data = data, Message = message };

        public static ServiceResult<T> Failure(string message) =>
            new ServiceResult<T> { IsSuccess = false, Message = message };
    }
}