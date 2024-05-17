using System.Text;
using Microsoft.CodeAnalysis;
using Purview.EventSourcing.SourceGenerator.Records;

namespace Purview.EventSourcing.SourceGenerator.Emitters;

partial class EventTargetClassEmitter
{
	static int EmitEventProperty(EventPropertyGenerationTarget target, StringBuilder builder, int indent,
		SourceProductionContext context,
		IGenerationLogger? logger)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		logger?.Debug($"Generating properties for: {target.FullyQualifiedName}");

		builder
			.Append(indent + 1, "public ", withNewLine: false)
			.Append(Constants.EventStore.EventDetails)
			.Append(" Details { get; set; } = new ")
			.Append(Constants.EventStore.EventDetails)
			.AppendLine("();")
			.AppendLine()
		;

		builder
			.Append(indent + 1, "public ", withNewLine: false)
			.Append(target.PropertyType);

		if (target.IsNullable)
			builder.Append('?');

		builder
			.Append(' ')
			.Append(target.PropertyName)
			.AppendLine(" { get; set; } = default!;")
		;

		return indent;
	}

	static int EmitAggregateProperty(EventPropertyGenerationTarget target, StringBuilder builder, int indent,
		SourceProductionContext context,
		IGenerationLogger? logger)
	{
		context.CancellationToken.ThrowIfCancellationRequested();

		logger?.Debug($"Generating aggregate properties for: {target.FullyQualifiedName}");

		builder
			.Append(indent + 1, "// Property for field: ", withNewLine: false)
			.Append(target.FieldName)
			.AppendLine();
		;

		if (!target.ValidationAttributes.IsEmpty)
			EmitValidationPropertyAttributes(target, builder, indent + 1);

		builder
			.Append(indent + 1, "public ", withNewLine: false)
			.Append(target.PropertyType);

		if (target.IsNullable)
			builder.Append('?');

		builder
			.Append(' ')
			.Append(target.PropertyName)
			.AppendLine()
			.Append(indent + 1, '{')
		;

		// Getter
		builder
			.Append(indent + 2, "get => ", withNewLine: false)
			.Append(target.FieldName)
			.Append(';')
			.AppendLine()
		;

		// Setter

		// Start of setter
		builder.AppendTabs(indent + 2);

		if (target.PrivateSetter)
			builder.Append("private ");

		builder
			.Append("set")
			.AppendLine()
			.Append(indent + 2, '{');

		// Output:
		// if (Comparer<T>.Default.Compare(value, _field) != 0)
		builder
			.Append(indent + 3, "if (", withNewLine: false)
			.Append(Constants.Shared.Comparer)
			.Append('<')
			.Append(target.PropertyType)
			.Append('>')
			.Append(".Default.Compare(value, ")
			.Append(target.FieldName)
			.Append(") != 0)")
			.AppendLine()
		;

		// Output:
		// {
		//		RecordAndApplyEvent(new EVENTNAME_GOES_HERE() { FIELDNAME_GOES_HERE = value });
		// }
		builder
			.Append(indent + 3, '{')
			.Append(indent + 4, "RecordAndApplyEvent(new ", withNewLine: false)
			.Append(target.FullyQualifiedName)
			.Append("() { ")
			.Append(target.PropertyName)
			.Append(" = value });")
			.AppendLine()
			.Append(indent + 3, '}')
		;

		// End of setter.
		builder.Append(indent + 2, '}');

		// End of the property.
		builder
			.Append(indent + 1, '}')
		;

		return indent;
	}

	static void EmitValidationPropertyAttributes(EventPropertyGenerationTarget target, StringBuilder builder, int indent)
	{
		foreach (var attrib in target.ValidationAttributes)
		{
			builder
				.Append(indent, '[', withNewLine: false)
				.Append(attrib.AttributeDetails);

			if (attrib.HasArguments)
			{
				builder.Append('(');

				if (!attrib.CtorArgs.IsEmpty)
				{
					for (var i = 0; i < attrib.CtorArgs.Length; i++)
					{
						builder.Append(attrib.CtorArgs[i]);
						if (i < attrib.CtorArgs.Length - 1 || !attrib.NamedArgs.IsEmpty)
							builder.Append(", ");
					}
				}

				if (!attrib.NamedArgs.IsEmpty)
				{
					var i = 0;
					foreach (var kvp in attrib.NamedArgs)
					{
						builder
							.Append(kvp.Key)
							.Append(" = ")
							.Append(kvp.Value)
						;

						if (i < attrib.NamedArgs.Count - 1)
							builder.Append(", ");

						i++;
					}
				}

				builder.Append(')');
			}

			builder.AppendLine(']');

		}
	}
}
