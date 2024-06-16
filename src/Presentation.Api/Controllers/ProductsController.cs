using Application.Products.Commands;
using Application.Products.Queries;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Api.Models;
using Presentation.Api.Models.DTOs;

namespace Presentation.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
	private readonly IMediator mediator;

	public ProductsController(IMediator mediator)
	{
		this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
	}

	[HttpPost]
	[AllowAnonymous]
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	public async Task<IActionResult> CreateProduct(CreateProductModel request)
	{
		var command = new CreateProductCommand(request.Name, request.Price, request.NumberOfStocks);
		var result = await this.mediator.Send(command);

		return CreatedAtAction(nameof(this.CreateProduct), new { id = result.Value }, result.Value);
	}

	[HttpGet]
	[AllowAnonymous]
	[ProducesResponseType(typeof(IEnumerable<ProductDTO>), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetProducts()
	{
		var products = await this.mediator.Send(new GetProductsQuery());
		return Ok(ProductDTO.ToProductDTOMapList(products));
	}

	[HttpPut("{productId}")]
	[AllowAnonymous]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateDetails(Guid productId, UpdateProductDetailsModel request)
	{
		var command = new UpdateProductDetailsCommand(productId, request.Name, request.Price);
		var result = await this.mediator.Send(command);

		if (result.IsSuccess) return NoContent();
		return BadRequest(result.Error);
	}

	[HttpPut("AdjustStock/{productId}")]
	[AllowAnonymous]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AdjustStock(Guid productId, AdjustProductStockModel request)
	{
		var command = new AdjustProductStockCommand(productId, request.Adjustment);
		var result = await this.mediator.Send(command);

		if (result.IsSuccess) return NoContent();
		return BadRequest(result.Error);
	}
}
