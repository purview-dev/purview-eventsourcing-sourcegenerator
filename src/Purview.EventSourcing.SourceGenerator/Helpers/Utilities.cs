using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Purview.EventSourcing.SourceGenerator.Helpers;

static class Utilities
{
	static readonly Regex TitleCaseSplit = new("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])", RegexOptions.Compiled);
	static readonly SymbolDisplayFormat SymbolDisplayFormat = new(
		typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces
	);

	static readonly Lazy<ImmutableDictionary<Templates.TypeInfo, string>> _typeInfoToSystemTypeMapper = new(GenerateSystemTypeMap);
	static readonly Lazy<ImmutableDictionary<string, string>> _fullTypeNameToSystemTypeMapper = new(() => _typeInfoToSystemTypeMapper.Value.ToImmutableDictionary(x => x.Key.FullName, x => x.Value));

	static ImmutableDictionary<Templates.TypeInfo, string> GenerateSystemTypeMap()
		// Putting this here ensures it's not accessed
		// before the static fields have been initialised.
		=> new Dictionary<Templates.TypeInfo, string>
		{
			{ Constants.Shared.String, Constants.Shared.StringKeyword },
			{ Constants.Shared.Boolean, Constants.Shared.BoolKeyword },
			{ Constants.Shared.Byte, Constants.Shared.ByteKeyword },
			{ Constants.Shared.Int16, Constants.Shared.ShortKeyword },
			{ Constants.Shared.Int32, Constants.Shared.IntKeyword },
			{ Constants.Shared.Int64, Constants.Shared.LongKeyword },
			{ Constants.Shared.Single, Constants.Shared.FloatKeyword },
			{ Constants.Shared.Double, Constants.Shared.DoubleKeyword },
			{ Constants.Shared.Decimal, Constants.Shared.DecimalKeyword }
		}.ToImmutableDictionary();

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
	public static string GetAggregateName(string aggregateType)
	{
		const string aggregate = "Aggregate";

		if (aggregateType.EndsWith(aggregate, StringComparison.InvariantCulture))
		{
			var result = aggregateType.Substring(0, aggregateType.Length - aggregate.Length);
			if (result.Length > 0)
				aggregateType = result;
		}

		return TitleCaseSplit.Replace(aggregateType, "-$1").ToLowerInvariant();
	}

	public static StringBuilder AppendTabs(this StringBuilder builder, int tabs)
	{
		for (var i = 0; i < tabs; i++)
			builder.Append('\t');

		return builder;
	}

	public static StringBuilder Append(this StringBuilder builder, int tabs, char value, bool withNewLine = true)
	{
		builder
			.AppendTabs(tabs)
			.Append(value);

		if (withNewLine)
			builder.AppendLine();

		return builder;
	}

	public static StringBuilder Append(this StringBuilder builder, int tabs, string value, bool withNewLine = true)
	{
		builder
			.AppendTabs(tabs)
			.Append(value);

		if (withNewLine)
			builder.AppendLine();

		return builder;
	}

	//static public StringBuilder AppendLines(this StringBuilder builder, int lineCount = 2) {
	//	for (var i = 0; i < lineCount; i++) {
	//		builder.AppendLine();
	//	}

	//	return builder;
	//}

	public static StringBuilder AppendLine(this StringBuilder builder, char @char)
		=> builder
			.Append(@char)
			.AppendLine();

	//static public StringBuilder AppendWrap(this StringBuilder builder, string value, char c = '"')
	//	=> builder
	//			.Append(c)
	//			.Append(value)
	//			.Append(c);

	public static string Wrap(this string value, char c = '"')
		=> c + value + c;

	public static string Strip(this string value, char c = '"')
	{
		if (value.Length > 1 && value[0] == c)
			value = value.Substring(1);

		if (value.Length > 1 && value[value.Length - 1] == c)
			value = value.Substring(0, value.Length - 1);

		return value;
	}

	//static public string? GetMemberIdentity(MemberDeclarationSyntax memberSyntax) {
	//	if (memberSyntax is MethodDeclarationSyntax method) {
	//		return method.Identifier.ValueText;
	//	}
	//	else if (memberSyntax is PropertyDeclarationSyntax property) {
	//		return property.Identifier.ValueText;
	//	}
	//	else if (memberSyntax is FieldDeclarationSyntax field) {
	//		var variable = field.Declaration.Variables.FirstOrDefault();
	//		return variable?.Identifier.ValueText;
	//	}
	//	else if (memberSyntax is EventFieldDeclarationSyntax @event) {
	//		var variable = @event.Declaration.Variables.FirstOrDefault();
	//		return variable?.Identifier.ValueText;
	//	}
	//	else if (memberSyntax is IndexerDeclarationSyntax indexer) {
	//		return indexer.ToString();
	//	}

	//	return null;
	//}

	public static ClassDeclarationSyntax? GetParentClass(SyntaxNode? node)
	{
		while (node != null)
		{
			if (node.Parent is ClassDeclarationSyntax classNode)
				return classNode;

			node = node.Parent;
		}

		return null;
	}

	public static string[] GetParentClasses(TypeDeclarationSyntax classDeclaration)
	{
		var parentClass = classDeclaration.Parent as ClassDeclarationSyntax;

		List<string> parentClassList = [];
		while (parentClass != null)
		{
			parentClassList.Add(parentClass.Identifier.Text);

			parentClass = parentClass.Parent as ClassDeclarationSyntax;
		}

		return [.. parentClassList];
	}

	public static string? GetParentClassesAsNamespace(TypeDeclarationSyntax classDeclaration)
	{
		var parentClass = classDeclaration.Parent as ClassDeclarationSyntax;

		List<string> parentClasses = [];
		while (parentClass != null)
		{
			parentClasses.Insert(0, parentClass.Identifier.Text);

			parentClass = parentClass.Parent as ClassDeclarationSyntax;
		}

		return parentClasses.Count == 0
			? null
			: string.Join(".", parentClasses);
	}

	public static string GetNamespace(TypeDeclarationSyntax typeSymbol)
	{
		// Determine the namespace the type is declared in, if any
		var potentialNamespaceParent = typeSymbol.Parent;
		while (potentialNamespaceParent != null &&
			   potentialNamespaceParent is not NamespaceDeclarationSyntax
			   && potentialNamespaceParent is not FileScopedNamespaceDeclarationSyntax)
		{
			potentialNamespaceParent = potentialNamespaceParent.Parent;
		}

		if (potentialNamespaceParent is BaseNamespaceDeclarationSyntax namespaceParent)
		{
			var @namespace = namespaceParent.Name.ToString();
			while (true)
			{
				if (namespaceParent.Parent is not NamespaceDeclarationSyntax namespaceParentParent)
					break;

				namespaceParent = namespaceParentParent;
				@namespace = $"{namespaceParent.Name}.{@namespace}";
			}

			return @namespace;
		}

		return string.Empty;
	}

	public static string GetFullyQualifiedOrSystemName(ITypeSymbol namedType, bool trimNullableAnnotation = true)
	{
		var result = namedType.ToDisplayString(SymbolDisplayFormat) ?? namedType.ToString();
		if (namedType as INamedTypeSymbol is { IsGenericType: true, IsValueType: false } genericType)
		{
			List<string> typeArguments = [];
			foreach (var typeArgument in genericType.TypeArguments)
				typeArguments.Add(GetFullyQualifiedOrSystemName(typeArgument));

			result += $"<{string.Join(", ", typeArguments)}>";
		}

		if (trimNullableAnnotation && namedType.NullableAnnotation == NullableAnnotation.Annotated)
			result = result.TrimEnd('?');

		return result.Convert();
	}

	static public string Convert(this Templates.TypeInfo type)
		=> _typeInfoToSystemTypeMapper.Value.GetValueOrDefault(type, type.FullName);

	static public string Convert(this string type)
		=> _fullTypeNameToSystemTypeMapper.Value.GetValueOrDefault(type, type);

	//static public string GetFullyQualifiedName(TypeDeclarationSyntax type)
	//	=> GetFullNamespace(type, true) + type.Identifier.Text;

	public static string? GetFullNamespace(TypeDeclarationSyntax type, bool includeTrailingSeparator)
	{
		var typeNamespace = GetNamespace(type);
		var parentClasses = GetParentClassesAsNamespace(type);

		string? fullNamespace = null;
		if (typeNamespace != null)
			fullNamespace = typeNamespace;

		if (parentClasses != null)
		{
			if (fullNamespace != null)
				fullNamespace += ".";

			fullNamespace += parentClasses;

			if (includeTrailingSeparator)
				fullNamespace += ".";
		}
		else if (includeTrailingSeparator && fullNamespace != null)
			fullNamespace += ".";

		return fullNamespace;
	}

	public static object? GetTypedConstantValue(TypedConstant arg)
		=> arg.Kind == TypedConstantKind.Array
			? arg.Values
			: arg.Value;

	public static IncrementalValuesProvider<TSource> WhereNotNull<TSource>(this IncrementalValuesProvider<TSource> source)
		=> source.Where(static m => m is not null);

	//static public bool IsEnumerableOrArray(string parameterType, string fullTypeName)
	//	=> IsArray(parameterType, fullTypeName)
	//		|| IsEnumerable(parameterType, fullTypeName);

	//static public bool IsArray(string parameterType, string fullTypeName)
	//	=> parameterType == fullTypeName + "[]";

	//static public bool IsEnumerable(string parameterType, string fullTypeName)
	//	=> parameterType == Constants.Shared.IEnumerable.FullName + "<" + fullTypeName + ">"
	//	|| parameterType.StartsWith(Constants.Shared.IEnumerable.FullName + "<" + fullTypeName, StringComparison.Ordinal);

	//static public bool IsString(string type)
	//	=> type == Constants.Shared.String.Name
	//		|| type == Constants.Shared.String.FullName
	//		|| type == Constants.Shared.StringKeyword;

	//static public bool IsExceptionType(ITypeSymbol? typeSymbol) {
	//	if (typeSymbol == null) {
	//		return false;
	//	}

	//	if (Constants.Shared.Exception.Equals(typeSymbol)) {
	//		return true;
	//	}

	//	return IsExceptionType(typeSymbol.BaseType);
	//}

	public static string Flatten(this SyntaxNode syntax)
		=> syntax.WithoutTrivia()
			.ToString()
			.Flatten();

	//static public string Flatten(this SyntaxToken syntax)
	//	=> syntax.WithoutTrivia()
	//		.ToString()
	//		.Flatten();

	public static string Flatten(this string value)
		=> Regex.Replace(value, @"\s+", " ", RegexOptions.None, TimeSpan.FromMilliseconds(1000));
}
