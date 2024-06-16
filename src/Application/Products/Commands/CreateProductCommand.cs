using Domain.Entities;
using Domain.EventSourcing;
using Domain.Primitives;
using MediatR;

namespace Application.Products.Commands;

public record CreateProductCommand : IRequest<Result<Guid>>
{
	public CreateProductCommand(string name, decimal price, int numberOfStocks)
	{
		this.ProductId = Guid.NewGuid();
		this.Name = name;
		this.Price = price;
		this.NumberOfStocks = numberOfStocks;
	}

	public Guid ProductId { get; }
	public string Name { get; }
	public decimal Price { get; }
	public int NumberOfStocks { get; }
}

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
	private readonly IPersistence<Product> persistence;

    public CreateProductCommandHandler(IPersistence<Product> persistence)
    {
		this.persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
	}

	public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
	{
		var product = Product.Create(command.ProductId, command.Name, command.Price, command.NumberOfStocks);

		await this.persistence.SaveAsync(product, ct: cancellationToken);

		return Result<Guid>.Success(product.Id);
	}
}