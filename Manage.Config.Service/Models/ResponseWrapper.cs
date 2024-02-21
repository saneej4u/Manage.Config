namespace Manage.Config.Service.Models
{
    public class ResponseWrapper
    {
        public bool IsClientError { get; }
        public bool IsNotFound { get; }
        public bool HasError => IsClientError || IsNotFound;

        public string ErrorMessage { get; }

        protected ResponseWrapper()
        {
        }

        protected ResponseWrapper(bool isClientError, bool isNotFound, string erroMessage)
        {
            IsClientError = isClientError;
            IsNotFound = isNotFound;
            ErrorMessage = erroMessage;
        }

        public static ResponseWrapper CreateSuccess()
        {
            return new ResponseWrapper();
        }

        public static ResponseWrapper<T> CreateSuccess<T>(T data)
        {
            return new ResponseWrapper<T>(data);
        }

        public static ResponseWrapper CreateNotFoundError(string errorMessage = null)
        {
            return new ResponseWrapper(false, true, errorMessage);
        }

        public static ResponseWrapper CreateClientError(string errorMessage = null)
        {
            return new ResponseWrapper(true, false, errorMessage);
        }
    }
}
