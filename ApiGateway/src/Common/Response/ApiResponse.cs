//namespace src.Common.Response
//{
//    public class ApiResponse
//    {
//        public int StatusCode { get; set; }
//        public bool Success => StatusCode >= 200 && StatusCode < 300;
//        public string Message { get; set; } = string.Empty;
//        public T? Data { get; set; }

//        public static ApiResponse<T> Ok(T data, string message = "Success")
//            => new() { StatusCode = 200, Message = message, Data = data };

//        public static ApiResponse<T> Fail(int statusCode, string message)
//            => new() { StatusCode = statusCode, Message = message };
//    }
//}
