using Application.Products.Errors;
using Domain.Entities;
using Domain.EventSourcing;
using Domain.Primitives;
using MediatR;

namespace Application.Products.Commands;

public record UpdateProductDetailsCommand(Guid ProductId, string NewName, decimal NewPrice) : IRequest<Result<bool>>;

public sealed class UpdateProductDetailsCommandHandler : IRequestHandler<UpdateProductDetailsCommand, Result<bool>>
{
	private readonly IPersistence<Product> persistence;

	public UpdateProductDetailsCommandHandler(IPersistence<Product> persistence)
	{
		this.persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
	}

	public async Task<Result<bool>> Handle(UpdateProductDetailsCommand command, CancellationToken cancellationToken)
	{
		var existingProduct = await this.persistence.StateStore.FindAsync(x => x.Id == command.ProductId, cancellationToken);

		if (existingProduct is null) return Result<bool>.Failure(ProductErrors.NotFound);

		var oldVersion = existingProduct.Version;

		existingProduct.UpdateDetails(command.NewName, command.NewPrice);
		await this.persistence.SaveAsync(existingProduct, oldVersion, cancellationToken);

		return Result<bool>.Success(true);
	}
}