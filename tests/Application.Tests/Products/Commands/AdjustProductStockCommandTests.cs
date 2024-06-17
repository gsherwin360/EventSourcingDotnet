using Application.Products.Commands;
using Application.Products.Errors;
using Domain.Entities;
using Domain.Fakes;
using FluentAssertions;

namespace Application.Tests.Products.Commands;

public class AdjustProductStockCommandTests
{
	[Fact]
	public async void Should_Adjust_Product_Stock_When_Command_Is_Handled_Successfully()
	{
		// Arrange
		var existingProduct = Product.Create(
			Guid.NewGuid(),
			ApplicationTests.Faker.Lorem.Word(),
			ApplicationTests.Faker.Random.Number(0, 100),
			ApplicationTests.Faker.Random.Number(1, 10));

		var fakePersistence = new FakePersistence<Product>();
		fakePersistence.Aggregates.Add(existingProduct.Id, existingProduct);

		int adjustment = ApplicationTests.Faker.Random.Number(1, 5);
		int expectedNumberOfStocks = existingProduct.NumberOfStocks + adjustment;

		var command = new AdjustProductStockCommand(existingProduct.Id, adjustment);
		var commandHandler = new AdjustProductStockCommandHandler(fakePersistence);

		// Act
		var commandHandlerResult = await commandHandler.Handle(command, CancellationToken.None);

		// Assert
		commandHandlerResult.Should().NotBeNull();
		commandHandlerResult.IsSuccess.Should().BeTrue();

		fakePersistence.Aggregates.TryGetValue(existingProduct.Id, out var updatedProduct);

		updatedProduct.Should().NotBeNull();
		updatedProduct.NumberOfStocks.Should().Be(expectedNumberOfStocks);

		// TODO: Validate the published event (ProductStocksAdjustedEvent)
	}

	[Fact]
	public async void Should_Return_NotFound_Error_Code_When_Product_Does_Not_Exist()
	{
		// Arrange
		var nonExistentProductId = Guid.NewGuid();

		var fakePersistence = new FakePersistence<Product>();

		var command = new UpdateProductDetailsCommand(nonExistentProductId, string.Empty, 0);
		var commandHandler = new UpdateProductDetailsCommandHandler(fakePersistence);

		// Act
		var commandHandlerResult = await commandHandler.Handle(command, CancellationToken.None);

		// Assert
		commandHandlerResult.Should().NotBeNull();
		commandHandlerResult.IsSuccess.Should().BeFalse();
		commandHandlerResult.Error.Should().BeEquivalentTo(ProductErrors.NotFound);
	}
}