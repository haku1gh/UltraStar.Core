// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0018:Inline variable declaration", Justification = "Does not always make it more readable.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0034:Simplify 'default' expression", Justification = "Does not always make it more readable. Though it could be considered in the future.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0016:Use 'throw' expression (Null check can be simplified)", Justification = "Does not always make it more readable. Though it could be considered in the future.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0039:Use local function", Justification = "Does not always make it more readable.")]

// Disable warnings from NGettext
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>", Scope = "member", Target = "~F:UltraStar.Core.ThirdParty.NGettext.Loaders.BigEndianBinaryReader._Buffer")]

// Disable warnings from Serilog
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "External code.", Scope = "member", Target = "~M:UltraStar.Core.ThirdParty.Serilog.Policies.ByteArrayScalarConversionPolicy.TryConvertToScalar(System.Object,UltraStar.Core.ThirdParty.Serilog.Events.ScalarValue@)~System.Boolean")]

