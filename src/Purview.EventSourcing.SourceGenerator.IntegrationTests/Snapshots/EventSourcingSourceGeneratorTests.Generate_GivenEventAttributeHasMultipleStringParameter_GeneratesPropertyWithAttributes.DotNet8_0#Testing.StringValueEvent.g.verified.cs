﻿//HintName: Testing.StringValueEvent.g.cs
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

namespace Testing
{
	sealed public partial class StringValueEvent : Purview.EventSourcing.Aggregates.Events.IEvent
	{
		public Purview.EventSourcing.Aggregates.Events.EventDetails Details { get; set; } = new Purview.EventSourcing.Aggregates.Events.EventDetails();

		public string? StringValue { get; set; } = default!;

		override public int GetHashCode()
		{
			return System.HashCode.Combine(StringValue);
		}
	}
}
