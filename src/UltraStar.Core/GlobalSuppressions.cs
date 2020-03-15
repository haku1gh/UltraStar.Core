// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018:Inline variable declaration", Justification = "Does not always make it more readable.")]

// Disable warnings from NGettext
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.Loaders.CatalogLoadingException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.PluralCompile.CompiledPluralRule")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.PluralCompile.Compiler.PluralRuleCompiler")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.Plural.AstPluralRule")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.Plural.Ast.AstTokenParser")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.Plural.Ast.ParserException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.Plural.Ast.Token")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.Plural.Ast.TokenDefinition")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "CS1591:Missing XML comment for publibly visible type or member", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.Plural.Ast.TokenType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Stype", "CS3021:Type or member does not need a CLSCompliant attribute because the assembly does not have a CLSCompliant attribute", Justification = "External code.", Scope = "type", Target = "UltraStar.Core.ThirdParty.NGettext.Loaders.BigEndianBinaryReader")]
