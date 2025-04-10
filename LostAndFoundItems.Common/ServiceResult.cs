namespace LostAndFoundItems.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Enums.Status Code { get; set; }

        public static ServiceResult Ok(string message = "Operation successful" )
        {
            return new ServiceResult { Success = true, Message = message };
        }

        public static ServiceResult Fail(string message, Enums.Status code = Enums.Status.NA)
        {
            return new ServiceResult { Success = false, Message = message, Code = code };
        }
    }
}
