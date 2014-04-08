using System;

namespace outcold.MoMaSy.Data
{
    internal class StorageException : Exception
    {
        public StorageException(string message)
            : base(message)
        {
        }
    }
}
