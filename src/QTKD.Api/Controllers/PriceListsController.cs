using Microsoft.AspNetCore.Mvc;
using QTKD.Api.DTOs;
using QTKD.Api.Services;

namespace QTKD.Api.Controllers;

[ApiController]
[Route("api/pricelists")]
public class PriceListsController(IPriceListService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PriceListResponse>>> GetAll() =>
        Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PriceListResponse>> GetById(int id) =>
        Ok(await service.GetByIdAsync(id));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PriceListResponse>> Create(CreatePriceListRequest request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet("{id:int}/items")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<PriceItemResponse>>> GetItems(int id) =>
        Ok(await service.GetItemsAsync(id));

    [HttpPost("{id:int}/items")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PriceItemResponse>> CreateItem(int id, CreatePriceItemRequest request)
    {
        var created = await service.CreateItemAsync(id, request);
        return CreatedAtAction(nameof(GetItems), new { id }, created);
    }
}
