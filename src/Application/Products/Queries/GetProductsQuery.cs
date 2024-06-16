using Domain.Entities;
using Domain.EventSourcing;
using MediatR;

namespace Application.Products.Queries;

public record GetProductsQuery : IRequest<IEnumerable<Product>>;

public sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
	private readonly IPersistence<Product> persistence;

	public GetProductsQueryHandler(IPersistence<Product> persistence)
	{
		this.persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
	}

	public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
	{
		var products = await this.persistence.StateStore.GetAll(cancellationToken);
		return products.ToList();
	}
}