using System;

namespace UltraStar.Core.ThirdParty.NGettext.Plural.Ast
{
    /// <summary>
    /// 
    /// </summary>
    public enum TokenType : int
    {
        /// <summary>
        /// 
        /// </summary>
		None,

		/// <summary>
		/// ?
		/// </summary>
		TernaryIf,

		/// <summary>
		/// :
		/// </summary>
		TernaryElse,

		/// <summary>
		/// ||
		/// </summary>
		Or,

		/// <summary>
		/// &amp;&amp;
		/// </summary>
		And,

		/// <summary>
		/// ==
		/// </summary>
		Equals,

		/// <summary>
		/// !=
		/// </summary>
		NotEquals,

		/// <summary>
		/// &gt;
		/// </summary>
		GreaterThan,

		/// <summary>
		/// &lt;
		/// </summary>
		LessThan,

		/// <summary>
		/// &gt;=
		/// </summary>
		GreaterOrEquals,

		/// <summary>
		/// &lt;=
		/// </summary>
		LessOrEquals,

		/// <summary>
		/// -
		/// </summary>
		Minus,

		/// <summary>
		/// +
		/// </summary>
		Plus,

		/// <summary>
		/// *
		/// </summary>
		Multiply,

		/// <summary>
		/// /
		/// </summary>
		Divide,

		/// <summary>
		/// %
		/// </summary>
		Modulo,

		/// <summary>
		/// !
		/// </summary>
		Not,

		/// <summary>
		/// n
		/// </summary>
		N,

        /// <summary>
        /// 
        /// </summary>
		Number,

		/// <summary>
		/// (
		/// </summary>
		LeftParenthesis,

		/// <summary>
		/// )
		/// </summary>
		RightParenthesis,

        /// <summary>
        /// 
        /// </summary>
		EOF,
	}
}
