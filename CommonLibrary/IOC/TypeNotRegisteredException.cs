using System;

namespace CommonLibrary.IOC
{
    public class TypeNotRegisteredException : Exception
    {
        public TypeNotRegisteredException(string message)
            : base(message)
        {
        }
    }
}