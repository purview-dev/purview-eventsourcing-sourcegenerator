﻿using Purview.EventSourcing.SourceGenerator.Templates;

// This this as the non-SourceGenerator namespace...
namespace Purview.EventSourcing;

static partial class Constants
{
	public static TemplateInfo[] GetAllTemplates()
	{
		return
			[.. Core.GetTemplates()];
	}

	public static class Core
	{
		public static readonly TemplateInfo GenerateAggregateAttribute = TemplateInfo.Create("Purview.EventSourcing.GenerateAggregateAttribute");
		public static readonly TemplateInfo EventPropertyAttribute = TemplateInfo.Create("Purview.EventSourcing.EventPropertyAttribute");

		public static TemplateInfo[] GetTemplates()
			=> [
				EventPropertyAttribute,
				GenerateAggregateAttribute
			];
	}

	public static class EventStore
	{
		public static readonly TypeInfo IAggregate = TypeInfo.Create("Purview.EventSourcing.Aggregates.IAggregate");
		public static readonly TypeInfo AggregateDetails = TypeInfo.Create("Purview.EventSourcing.Aggregates.AggregateDetails");

		public static readonly TypeInfo IEvent = TypeInfo.Create("Purview.EventSourcing.Aggregates.Events.IEvent");
		public static readonly TypeInfo EventBase = TypeInfo.Create("Purview.EventSourcing.Aggregates.Events.EventBase");
		public static readonly TypeInfo EventDetails = TypeInfo.Create("Purview.EventSourcing.Aggregates.Events.EventDetails");

		public static readonly TypeInfo LockedException = TypeInfo.Create("Purview.EventSourcing.Aggregates.Exceptions.LockedException");
	}

	public static class Shared
	{
		public const string StringKeyword = "string";
		public const string BoolKeyword = "bool";
		public const string ByteKeyword = "byte";
		public const string ShortKeyword = "short";
		public const string IntKeyword = "int";
		public const string LongKeyword = "long";
		public const string FloatKeyword = "float";
		public const string DoubleKeyword = "double";
		public const string DecimalKeyword = "decimal";

		public static readonly TypeInfo String = TypeInfo.Create<string>();
		public static readonly TypeInfo Boolean = TypeInfo.Create<bool>();
		public static readonly TypeInfo Byte = TypeInfo.Create<byte>();
		public static readonly TypeInfo Int16 = TypeInfo.Create<short>(); // int16
		public static readonly TypeInfo Int32 = TypeInfo.Create<int>(); // int32
		public static readonly TypeInfo Int64 = TypeInfo.Create<long>(); // int64
		public static readonly TypeInfo Single = TypeInfo.Create<float>(); // single
		public static readonly TypeInfo Double = TypeInfo.Create<double>();
		public static readonly TypeInfo Decimal = TypeInfo.Create<decimal>();

		public static readonly TypeInfo Type = TypeInfo.Create("System.Type");
		public static readonly TypeInfo HashCode = TypeInfo.Create("System.HashCode");

		public static readonly TypeInfo Comparer = TypeInfo.Create("System.Collections.Generic.Comparer"); // <>;

		public static readonly TypeInfo ConcurrentQueue = TypeInfo.Create("System.Collections.Concurrent.ConcurrentQueue"); // <>;
		public static readonly TypeInfo List = TypeInfo.Create("System.Collections.Generic.List"); // <>;
		public static readonly TypeInfo IEnumerable = TypeInfo.Create("System.Collections.Generic.IEnumerable"); // <>;

		public static readonly TypeInfo ValidationAttribute = TypeInfo.Create("System.ComponentModel.DataAnnotations.ValidationAttribute");
	}

	public static class Diagnostics
	{
		public static class Activity
		{
			public const string Usage = nameof(Activity) + "." + nameof(Usage);
		}
	}
}
