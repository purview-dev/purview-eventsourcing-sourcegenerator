namespace Purview.EventSourcing.SourceGenerator;

partial class EventSourcingSourceGeneratorTests
{
	[Fact]
	public async Task Generate_GivenAggregateIsNestedClass_GeneratesWithinNestedClass()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;

namespace Testing;

public partial class ParentClass
{
	[GenerateAggregate]
	public partial class NestedTestAggregate : IAggregate
	{
		[EventProperty(EventTypeName = ""TheBadger___Event"")]
		string? _stringValue;
	}
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenAggregateIsNestedClasses_GeneratesWithinNestedClasses()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;

namespace Testing;

public partial class ParentClass {
	public partial class ParentClass2 {
		[GenerateAggregate]
		public partial class TestAggregate : IAggregate {
			[EventProperty(EventTypeName = ""TheBadger_EVENT"")]
			string? _stringValue;
		}
	}
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
