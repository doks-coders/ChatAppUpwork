using ChatUpdater.Models;

namespace ChatUpdater.ApplicationCore.Helpers
{

    public class ApiErrorException : Exception
    {
        public string ErrorCode { get; set; }

        public ApiErrorException(string errorCode)
            : base(errorCode + " : " + new BaseErrorCodes().ErrorMessages[errorCode])
        {
            ErrorCode = errorCode;
        }

        public ApiErrorException(string errorCode, string message)
            : base(errorCode + " : " + message)
        {
            ErrorCode = errorCode;
        }

        public ApiErrorException(string errorCode, Exception innerException)
            : base(errorCode, innerException)
        {
            ErrorCode = errorCode;
        }

        public ApiErrorException(List<FluentValidation.Results.ValidationFailure> validationFailures)
            : base(string.Join(",\n", validationFailures.Select(x => x.ErrorMessage).ToList()))
        {
            ErrorCode = BaseErrorCodes.ValidationError;
        }

    }
}
