using System;
using UltraStar.Core.ThirdParty.NGettext.Plural;

namespace UltraStar.Core.ThirdParty.NGettext.PluralCompile
{
    /// <summary>
    /// 
    /// </summary>
    public class CompiledPluralRule : PluralRule
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CompiledPluralRule"/> class using specified maximum plural
		/// forms value and an evaluation delegate.
		/// </summary>
		/// <param name="numPlurals"></param>
		/// <param name="evaluatorDelegate"></param>
		public CompiledPluralRule(int numPlurals, PluralRuleEvaluatorDelegate evaluatorDelegate)
			: base(numPlurals, evaluatorDelegate)
		{
		}
	}
}
