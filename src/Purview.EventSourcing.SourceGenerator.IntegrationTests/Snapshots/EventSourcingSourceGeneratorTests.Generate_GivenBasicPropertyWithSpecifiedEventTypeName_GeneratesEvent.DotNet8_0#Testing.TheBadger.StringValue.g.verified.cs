﻿//HintName: Testing.TheBadger.StringValue.g.cs
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Purview.EventSourcing.SourceGenerator
//     on {Scrubbed}.
//
//     Changes to this file may cause incorrect behaviour and will be lost
//     when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 1591 // publicly visible type or member must be documented

#nullable enable

#nullable enable

namespace Testing
{
	partial class TestAggregate
	{
		// Property for field: _stringValue
		public string? StringValue
		{
			get => _stringValue;
			private set
			{
				if (System.Collections.Generic.Comparer<string>.Default.Compare(value, _stringValue) != 0)
				{
					RecordAndApplyEvent(new Testing.TheBadger() { StringValue = value });
				}
			}
		}

		void ApplyStringValue(Testing.TheBadger @event)
		{
			StringValue = @event.StringValue;
		}
	}
}
