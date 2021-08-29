using System;

namespace TecWi_Web.Domain.Validators
{
    public class DomainValidadorException : Exception
    {
        public DomainValidadorException(string errorMessage) : base(errorMessage)
        {

        }

        public static void Whem(bool errorExists, string errorMessage)
        {
            if (errorExists)
            {
                throw new DomainValidadorException(errorMessage);
            }
        }
    }
}
