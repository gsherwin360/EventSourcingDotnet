using Application.Products.Commands;
using Application.Products.Errors;
using Domain.Entities;
using Domain.Fakes;
using FluentAssertions;

namespace Application.Tests.Products.Commands;

public class UpdateProductDetailsCommandTests
{
	[Fact]
	public async void Should_Update_Product_Details_When_Command_Is_Handled_Successfully()
	{
		// Arrange
		var existingProduct = Product.Create(
			Guid.NewGuid(),
			ApplicationTests.Faker.Lorem.Word(),
			ApplicationTests.Faker.Random.Number(0, 100),
			ApplicationTests.Faker.Random.Number(0, 100));

		var fakePersistence = new FakePersistence<Product>();
		fakePersistence.Aggregates.Add(existingProduct.Id, existingProduct);

		string newName = ApplicationTests.Faker.Lorem.Word();
		decimal newPrice = ApplicationTests.Faker.Random.Number(0, 100);

		var command = new UpdateProductDetailsCommand(existingProduct.Id, newName, newPrice);
		var commandHandler = new UpdateProductDetailsCommandHandler(fakePersistence);

		// Act
		var commandHandlerResult = await commandHandler.Handle(command, CancellationToken.None);

		// Assert
		commandHandlerResult.Should().NotBeNull();
		commandHandlerResult.IsSuccess.Should().BeTrue();

		fakePersistence.Aggregates.TryGetValue(existingProduct.Id, out var updatedProduct);

		updatedProduct.Should().NotBeNull();
		updatedProduct.Name.Should().Be(newName);
		updatedProduct.Price.Should().Be(newPrice);

		// TODO: Validate the published event (ProductDetailsUpdatedEvent)
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