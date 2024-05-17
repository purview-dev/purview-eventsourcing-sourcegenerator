using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Purview.EventSourcing.SourceGenerator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class EventPropertySurpressor : DiagnosticSuppressor
{
	static readonly SuppressionDescriptor Rule = new(
		"ESS0001",
		"IDE0044",
		"Read only fields aren't required or relevent as Event Properties."
	);

	public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions
		=> [Rule];

	public override void ReportSuppressions(SuppressionAnalysisContext context)
	{
		foreach (var diagnostic in context.ReportedDiagnostics)
		{
			context.CancellationToken.ThrowIfCancellationRequested();

			var location = diagnostic.Location;
			var syntaxTree = location.SourceTree;
			var root = syntaxTree?.GetRoot(context.CancellationToken);
			var textSpan = location.SourceSpan;
			var node = root?.FindNode(textSpan);

			if (syntaxTree == null || node == null)
				continue;

			var semanticModel = context.GetSemanticModel(syntaxTree);
			if (ShouldSuppress(context.Compilation, semanticModel, node, context.CancellationToken))
			{
				var suppression = Suppression.Create(Rule, diagnostic);
				context.ReportSuppression(suppression);
			}
		}
	}

	static bool ShouldSuppress(Compilation _, SemanticModel __, SyntaxNode node, CancellationToken token)
	{
		token.ThrowIfCancellationRequested();

		if (!node.IsKind(SyntaxKind.FieldDeclaration))
			return false;

		var fieldDeclaration = (FieldDeclarationSyntax)node;
		return fieldDeclaration.AttributeLists
			.SelectMany(x => x.Attributes)
			.Any(x => Constants.Core.EventPropertyAttribute.Equals(x.Name));
	}
}
