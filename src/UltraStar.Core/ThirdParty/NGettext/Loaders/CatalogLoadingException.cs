using System;

namespace UltraStar.Core.ThirdParty.NGettext.Loaders
{
    /// <summary>
    /// 
    /// </summary>
    public class CatalogLoadingException : Exception
	{
        /// <summary>
        /// 
        /// </summary>
		public CatalogLoadingException() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
		public CatalogLoadingException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
		public CatalogLoadingException(string message, Exception innerException) : base(message, innerException) { }
	}
}
