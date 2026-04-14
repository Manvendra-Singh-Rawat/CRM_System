namespace ClientManagement.Application.DataTemplate
{
    public class Result<T>
    {
        public bool IsSuccess {  get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public static Result<T> Success(T retrievedData, string retrievedMessage = "")
            => new()
            {
                IsSuccess = true,
                Data = retrievedData,
                Message = retrievedMessage
            };

        public static Result<T> Failure(string errorMessage)
            => new()
            {
                IsSuccess = false,
                Message = errorMessage
            };
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Errors { get; set; }

        public static ApiResponse<T> SuccessResponse(T data, string message)
            => new()
            {
                Success = true,
                Data = data,
                Message = message
            };

        public static ApiResponse<T> FailureResponse(string message, object? errors = null)
            => new()
                {
                    Success = false,
                    Message = message,
                    Errors = errors
                };
    }
}
