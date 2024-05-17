namespace Purview.EventSourcing.SourceGenerator;

partial class EventSourcingSourceGeneratorTests
{
	[Fact]
	public async Task Generate_GivenEventAttributeHasValidationAttribute_GeneratesPropertyWithAttributes()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Testing;

[GenerateAggregate]
public partial class TestAggregate : IAggregate
{
	[EventProperty]
	[Required]
	string? _stringValue;
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenEventAttributeHasValidationAttributeWithCtorArgs_GeneratesPropertyWithAttributes()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Testing;

[GenerateAggregate]
public partial class TestAggregate : IAggregate
{
	[EventProperty]
	[Range(1, 100)]
	int _intValue;
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenEventAttributeHasValidationAttributeWithNamedCtorArgs_GeneratesPropertyWithAttributes()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Testing;

[GenerateAggregate]
public partial class TestAggregate : IAggregate
{
	[EventProperty]
	[Range(1, maximum: 100)]
	int _intValue;
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenEventAttributeHasValidationAttributeWithNamedCtorArgsInTheWrongOrder_GeneratesPropertyWithAttributes()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Testing;

[GenerateAggregate]
public partial class TestAggregate : IAggregate
{
	[EventProperty]
	[Range(maximum: 100, minimum: 1)]
	int _intValue;
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenEventAttributeHasValidationAttributeWithCtorAndNamedArgs_GeneratesPropertyWithAttributes()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Testing;

[GenerateAggregate]
public partial class TestAggregate : IAggregate
{
	[EventProperty]
	[StringLength(100, MinimumLength = 10)]
	string? _stringValue;
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenEventAttributeHasValidationAttributeWithNamedArgs_GeneratesPropertyWithAttributes()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Testing;

[GenerateAggregate]
public partial class TestAggregate : IAggregate
{
	[EventProperty]
	[Base64String(ErrorMessage = ""asd"")]
	string? _stringValue;
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenEventAttributeHasMultipleValidationAttributes_GeneratesPropertyWithAttributes()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Testing;

[GenerateAggregate]
public partial class TestAggregate : IAggregate
{
	[EventProperty]
	[Base64String(ErrorMessage = ""asd"")]
	[StringLength(100, MinimumLength = 10)]
	[Range(1, 100)]
	[Required]
	string? _stringValue;
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}

	[Fact]
	public async Task Generate_GivenEventAttributeHasMultipleStringParameters_GeneratesPropertyWithAttributes()
	{
		// Arrange
		var basicAggregate = @"
using Purview.EventSourcing;
using Purview.EventSourcing.Aggregates;
using System.ComponentModel.DataAnnotations;

namespace Testing;

[GenerateAggregate]
public partial class TestAggregate : IAggregate
{
	[EventProperty]
	[DeniedValues(""\n"", ""\r"", ""\t"")]
	[AllowedValues(values: [""12"", 123], ErrorMessage = ""Hello"")]
	string? _stringValue;
}
";

		// Act
		var generationResult = await GenerateAsync(basicAggregate);

		// Assert
		await TestHelpers.Verify(generationResult);
	}
}
