using System;
using System.Collections.Generic;
using System.Text;

namespace Afonsoft.ActiveDirectory
{
    public class ActiveDirectoryException : Exception
    {
        public ActiveDirectoryException()
        {
        }

        public ActiveDirectoryException(string message)
            : base(message)
        {
        }

        public ActiveDirectoryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected ActiveDirectoryException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}