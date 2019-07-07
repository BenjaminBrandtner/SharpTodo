using System;
using System.Runtime.Serialization;

namespace Backend
{
	[Serializable]
	public class InvalidTodoException : Exception
	{
		public InvalidTodoException()
		{
		}

		public InvalidTodoException(string message) : base(message)
		{
		}

		public InvalidTodoException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected InvalidTodoException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}