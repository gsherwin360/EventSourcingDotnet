using Application.Products.Queries;
using Domain.Entities;
using Domain.Fakes;
using FluentAssertions;

namespace Application.Tests.Products.Queries;

public class GetProductsQueryTests
{
    [Fact]
    public async Task Should_Return_Product_List_When_Query_Is_Handled_Successfully()
    {
		// Arrange
		int expectedProductCount = ApplicationTests.Faker.Random.Number(1, 10);

		var fakePersistence = new FakePersistence<Product>();

		for (int i = 0; i < expectedProductCount; i++)
		{
			fakePersistence.Aggregates.Add(
				Guid.NewGuid(),
				Product.Create(
					Guid.NewGuid(),
					ApplicationTests.Faker.Lorem.Word(),
					ApplicationTests.Faker.Random.Number(0, 100),
					ApplicationTests.Faker.Random.Number(1, 10)));
		}

		var query = new GetProductsQuery();
		var queryHandler = new GetProductsQueryHandler(fakePersistence);

		// Act
		var queryHandlerResult = await queryHandler.Handle(query, CancellationToken.None);

		// Assert
		queryHandlerResult.Should().NotBeNull().And.HaveCount(expectedProductCount).And.AllBeOfType<Product>();
	}

	[Fact]
	public async Task Should_Return_Empty_Result_When_Product_List_Is_Empty()
	{
		// Arrange
		var fakePersistence = new FakePersistence<Product>();
		var query = new GetProductsQuery();
		var queryHandler = new GetProductsQueryHandler(fakePersistence);

		// Act
		var queryHandlerResult = await queryHandler.Handle(query, CancellationToken.None);

		// Assert
		queryHandlerResult.Should().NotBeNull().And.BeEmpty();
	}
}