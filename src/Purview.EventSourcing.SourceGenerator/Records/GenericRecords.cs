﻿namespace Purview.EventSourcing.SourceGenerator.Records;

record AttributeValue<T>
	where T : struct
{
	public AttributeValue(T? value)
	{
		Value = value;
		IsSet = true;
	}

	public AttributeValue()
	{
		IsSet = false;
	}

	public T? Or(T value)
	{
		return IsSet ? Value : value;
	}

	public T? Value { get; }

	public bool IsSet { get; }

	public static implicit operator AttributeValue<T>(T? value)
		=> new(value);

	public static implicit operator T?(AttributeValue<T> value)
		=> value.Value;
}

record AttributeStringValue
{
	public AttributeStringValue(string? value)
	{
		Value = value;
		IsSet = true;
	}

	public AttributeStringValue()
	{
		IsSet = false;
	}

	public string Or(string value)
	{
		return IsSet && !string.IsNullOrWhiteSpace(Value) ? Value! : value;
	}

	public string? Value { get; }

	public bool IsSet { get; }

	public static implicit operator AttributeStringValue(string? value)
		=> new(value);

	public static implicit operator string?(AttributeStringValue value)
		=> value.Value;
}
