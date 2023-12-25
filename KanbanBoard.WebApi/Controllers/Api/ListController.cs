using KanbanBoard.Service.List;
using KanbanBoard.WebApi.RequestModels;
using KanbanBoard.WebApi.ResponseContracts;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.Controllers.Api;

[ApiController]
[Route("api/lists")]
public class ListController : ControllerBase
{
    private readonly ListService _listService;

    public ListController(ListService listService)
    {
        _listService = listService;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAllLists()
    {
        var lists = await _listService.GetAllLists();
        return Ok(lists.Select(ListContract.ConvertToContract));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetListById(int id)
    {
        var listEntity = await _listService.GetListById(id);
        if (listEntity != null)
            return Ok(ListContract.ConvertToContract(listEntity));

        return NotFound();
    }

    [HttpPost("post")]
    public async Task<IActionResult> RegisterList([FromBody] RegisterListRequestModel requestModel)
    {
        var result = await _listService.RegisterList(new()
        {
            Title = requestModel.Title
        });

        if (result.Success)
            return Ok(result.ListEntity);

        return BadRequest(result.Error);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateList(int id, [FromBody] UpdateListRequestModel requestModel)
    {
        var result = await _listService.UpdateList(id, new()
        {
            Title = requestModel.Title
        });

        if (result.Success)
            return Ok(ListContract.ConvertToContract(result.ListEntity));

        if (result.ListNotExists)
        {
            return Conflict(new
            {
                ListNotExists = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CloseList(int id)
    {
        var result = await _listService.CloseList(id);

        if (result.Success)
            return Ok();
        
        if (result.ListNotExists)
        {
            return Conflict(new
            {
                ListNotExists = true
            });
        }

        return BadRequest(result.Error);
    }
}