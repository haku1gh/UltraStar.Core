using System;

namespace UltraStar.Core.ThirdParty.NGettext.Plural.Ast
{
    /// <summary>
    /// 
    /// </summary>
    public class ParserException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
		public ParserException() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
		public ParserException(string message) : base(message) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
		public ParserException(string message, Exception innerException) : base(message, innerException) { }
	}
}
