using Microsoft.AspNetCore.Mvc;
using QTKD.Api.DTOs;
using QTKD.Api.Services;

namespace QTKD.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController(IContractService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContractResponse>> GetById(int id)
    {
        var contract = await service.GetByIdAsync(id);
        return contract is null ? NotFound() : Ok(contract);
    }
}
