using System;

namespace khi.Catalog
{
    public class PageLoadingException : Exception
    {
        public PageLoadingException(string message) : base(message)
        {
        }
    }
}