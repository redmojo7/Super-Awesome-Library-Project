using System;

namespace BusinessWebAPI.CustomException
{
    [Serializable]
    public class InvalidIndexException: Exception
    {
        public InvalidIndexException() : base() { }
        public InvalidIndexException(string message) : base(message) { }
        public InvalidIndexException(string message, Exception inner) : base(message, inner) { }
    }
}