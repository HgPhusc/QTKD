using Microsoft.AspNetCore.Mvc;
using QTKD.Api.DTOs;
using QTKD.Api.Entities;
using QTKD.Api.Services;

namespace QTKD.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController(IContractService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ContractResponse>>> GetAll([FromQuery] ContractStatus? status) =>
        Ok(await service.GetAllAsync(status));

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContractResponse>> GetById(int id) =>
        Ok(await service.GetByIdAsync(id));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ContractResponse>> Create(CreateContractRequest request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ContractResponse>> Update(int id, UpdateContractRequest request) =>
        Ok(await service.UpdateAsync(id, request));

    // US03 – gửi hợp đồng trình duyệt
    [HttpPost("{id:int}/submit")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ContractResponse>> Submit(int id) =>
        Ok(await service.SubmitAsync(id));
}
