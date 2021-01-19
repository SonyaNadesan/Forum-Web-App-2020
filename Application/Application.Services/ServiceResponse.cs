using System;

namespace Application.Services
{
    public class ServiceResponse<TResult>
    {
        private string error;

        public TResult Result { get; set; }

        public string ErrorMessage
        {
            get
            {
                return error;
            }

            set
            {
                error = value;
                IsValid = false;
            }
        }

        public bool IsValid { get; set; }

        public ServiceResponse()
        {
            IsValid = true;
        }

        public ServiceResponse(TResult result)
        {
            Result = result;
            IsValid = true;
        }

        public object Skip()
        {
            throw new NotImplementedException();
        }
    }
}
