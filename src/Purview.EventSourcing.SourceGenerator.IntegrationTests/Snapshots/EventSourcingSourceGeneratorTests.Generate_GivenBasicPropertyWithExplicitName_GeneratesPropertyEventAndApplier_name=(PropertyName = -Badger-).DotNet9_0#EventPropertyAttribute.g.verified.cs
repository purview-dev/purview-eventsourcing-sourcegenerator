﻿//HintName: EventPropertyAttribute.g.cs
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

namespace Purview.EventSourcing;

/// <summary>
/// Represents a field that will be converted to a property and an event class
/// used for generating the event and aggregate classes. By default, private setters are generated.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
[System.Diagnostics.Conditional("PURVIEW_EVENTSOURCE_EMBED_ATTRIBUTES")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1019:Define accessors for attribute arguments")]
sealed class EventPropertyAttribute : System.Attribute {
	/// <summary>
	/// Creates a new instance of the <see cref="EventPropertyAttribute"/> class.
	/// </summary>
	public EventPropertyAttribute() {
	}

	/// <summary>
	/// Creates a new instance of the <see cref="EventPropertyAttribute"/> class.
	/// </summary>
	/// <param name="propertyName">Specifies the <see cref="PropertyName"/>.</param>
	/// <param name="privateSetter">Optionally specifies the <see cref="PrivateSetter"/>.</param>
	/// <param name="eventTypeName">Optionally specifies the <see cref="EventTypeName"/>.</param>
	public EventPropertyAttribute(string propertyName, bool privateSetter = true, string? eventTypeName = null) {
		PropertyName = propertyName;
		PrivateSetter = privateSetter;
		EventTypeName = eventTypeName;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="EventPropertyAttribute"/> class.
	/// </summary>
	/// <param name="privateSetter">Specifies the <see cref="PrivateSetter"/>.</param>
	public EventPropertyAttribute(bool privateSetter) {
		PrivateSetter = privateSetter;
	}

	/// <summary>
	/// Determines the name of the property that will be generated.
	/// This is automatically generated if one is not specified.
	/// By default, the name of the field is used
	/// with any leading underscore removed or 'm' followed
	/// by an underscore. Anything remaining is made title case.
	/// For example, '_myValue', 'm_myValue' or 'myValue' becomes 'MyValue'.
	/// </summary>
	public string? PropertyName { get; set; }

	/// <summary>
	/// Determines if the property should have a private setter.
	/// True to generate a private setter; otherwise, false to generate a public one.
	/// </summary>
	public bool PrivateSetter { get; set; } = true;

	/// <summary>
	/// The name of the event to generate.
	/// This is automatically generated if one is not specified.
	/// </summary>
	public string? EventTypeName { get; set; }
}