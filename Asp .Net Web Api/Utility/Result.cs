

namespace Asp_.Net_Web_Api.Utility
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string? Token { get; set; }
        public T? Data { get; set; }

        public static Result<T> Success(T data, string message = null) => new() { IsSuccess = true, Data = data, Message = message };
        public static Result<T> Success(string message, string token, T data) => new() { IsSuccess = true, Message=message, Token = token , Data = data};
        public static Result<T> Failure(string message) => new() { IsSuccess = false, Message = message };
    }
}
