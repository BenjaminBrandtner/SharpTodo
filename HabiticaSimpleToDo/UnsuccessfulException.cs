﻿using System;
using System.Runtime.Serialization;

namespace HabiticaSimpleToDo
{
    [Serializable]
    internal class UnsuccessfulException : Exception
    {
        public UnsuccessfulException()
        {
        }

        public UnsuccessfulException(string message) : base(message)
        {
        }

        public UnsuccessfulException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnsuccessfulException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}