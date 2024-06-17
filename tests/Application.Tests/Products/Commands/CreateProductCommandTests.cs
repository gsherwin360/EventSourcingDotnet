using Application.Products.Commands;
using Domain.Entities;
using Domain.Fakes;
using FluentAssertions;

namespace Application.Tests.Products.Commands;

public class CreateProductCommandTests
{
	[Fact]
	public async Task Should_Create_Product_When_Command_Is_Handled_Successfully()
	{
		// Arrange
		string name = ApplicationTests.Faker.Lorem.Word();
		decimal price = ApplicationTests.Faker.Random.Number(0, 100);
		int numberOfStocks = ApplicationTests.Faker.Random.Number(0, 100);

		var fakePersistence = new FakePersistence<Product>();
		var commandHandler = new CreateProductCommandHandler(fakePersistence);

		var command = new CreateProductCommand(name, price, numberOfStocks);

		// Act
		var commandHandlerResult = await commandHandler.Handle(command, CancellationToken.None);

		// Assert
		commandHandlerResult.Should().NotBeNull();
		commandHandlerResult.IsSuccess.Should().BeTrue();

		fakePersistence.Aggregates.TryGetValue(commandHandlerResult.Value, out var product);

		product.Should().NotBeNull();
		product.Id.Should().Be(commandHandlerResult.Value);
		product.Name.Should().Be(name);
		product.Price.Should().Be(price);
		product.NumberOfStocks.Should().Be(numberOfStocks);

		// TODO: Validate the published event (ProductCreatedEvent)
	}
}