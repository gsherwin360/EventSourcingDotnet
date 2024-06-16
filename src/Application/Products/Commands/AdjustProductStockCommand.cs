using Application.Products.Errors;
using Domain.Entities;
using Domain.EventSourcing;
using Domain.Primitives;
using MediatR;

namespace Application.Products.Commands;

public record AdjustProductStockCommand(Guid ProductId, int Adjustment) : IRequest<Result<bool>>;

public sealed class AdjustProductStockCommandHandler : IRequestHandler<AdjustProductStockCommand, Result<bool>>
{
	private readonly IPersistence<Product> persistence;

	public AdjustProductStockCommandHandler(IPersistence<Product> persistence)
	{
		this.persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
	}

	public async Task<Result<bool>> Handle(AdjustProductStockCommand command, CancellationToken cancellationToken)
	{
		var existingProduct = await this.persistence.StateStore.ReadAsync(command.ProductId, cancellationToken);

		if (existingProduct is null) return Result<bool>.Failure(ProductErrors.NotFound);
			
		var oldVersion = existingProduct.Version;

		existingProduct.AdjustStocks(command.Adjustment);
		await this.persistence.SaveAsync(existingProduct, oldVersion, cancellationToken);

		return Result<bool>.Success(true);
	}
}
