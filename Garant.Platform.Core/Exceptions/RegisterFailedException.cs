using System;

namespace Garant.Platform.Core.Exceptions
{
    public class RegisterFailedException : Exception
    {
        public RegisterFailedException(string message) : base(message)
        {
        }
    }
}
