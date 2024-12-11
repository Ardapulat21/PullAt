namespace PullAt.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static Result Success(object data = null, string message = "Operation succeeded.")
        {
            return new Result { IsSuccess = true, Data = data, Message = message };
        }
        public static Result Failure(object data = null,string message = "Operation failed.")
        {
            return new Result { IsSuccess = false, Data = data, Message = message };
        }
    }
}