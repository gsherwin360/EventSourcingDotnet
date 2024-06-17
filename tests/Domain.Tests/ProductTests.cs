using Bogus;
using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests;

public class ProductTests
{
	[Fact]
	public void Should_Create_Product_Basic_Details_For_Valid_Params()
	{
		// Arrange
		var productId = Guid.NewGuid();
		string name = Faker.Lorem.Word();
		decimal price = Faker.Random.Number(0, 100);
		int numberOfStocks = Faker.Random.Number(0, 10);

		// Act
		var product = Product.Create(productId, name, price, numberOfStocks);

		// Assert
		product.Should().NotBeNull();
		product.Id.Should().Be(productId);
		product.Name.Should().Be(name);
		product.Price.Should().Be(price);
		product.NumberOfStocks.Should().Be(numberOfStocks);

		// TODO: Validate the published event (ProductCreatedEvent)
	}

	[Fact]
	public void Should_Update_Product_Basic_Details_For_Valid_Params()
	{
		// Arrange
		var existingProduct = Product.Create(
			Guid.NewGuid(),
			Faker.Lorem.Word(),
			Faker.Random.Number(0, 100),
			Faker.Random.Number(1, 10));

		string newName = Faker.Lorem.Word();
		decimal newPrice = Faker.Random.Number(101, 199);

		// Act
		existingProduct.UpdateDetails(newName, newPrice);

		// Assert
		existingProduct.Should().NotBeNull();
		existingProduct.Name.Should().Be(newName);
		existingProduct.Price.Should().Be(newPrice);

		// TODO: Validate the published event (ProductDetailsUpdatedEvent)
	}

	[Fact]
	public void Should_Adjust_Product_Stock_For_Valid_Params()
	{
		// Arrange
		var existingProduct = Product.Create(
			Guid.NewGuid(),
			Faker.Lorem.Word(),
			Faker.Random.Number(0, 100),
			Faker.Random.Number(1, 10));

		int adjustment = Faker.Random.Number(1, 5);
		int expectedNumberOfStocks = existingProduct.NumberOfStocks + adjustment;

		// Act
		existingProduct.AdjustStock(adjustment);

		// Assert
		existingProduct.Should().NotBeNull();
		existingProduct.NumberOfStocks.Should().Be(expectedNumberOfStocks);

		// TODO: Validate the published event (ProductStocksAdjustedEvent)
	}
	public static Faker Faker => new Faker();
}