using KanbanBoard.Service.CardHistory;
using KanbanBoard.WebApi.ResponseContracts;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.Controllers.Api;

[ApiController]
[Route("api/cardHistories")]
public class CardHistoryController : ControllerBase
{
    private readonly CardHistoryService _cardHistoryService;

    public CardHistoryController(CardHistoryService cardHistoryService)
    {
        _cardHistoryService = cardHistoryService;
    }

    [HttpGet("{cardId}")]
    public async Task<IActionResult> GetCardHistory(int cardId)
    {
        var result = await _cardHistoryService.GetCardHistory(cardId);

        if (result.Success)
            return Ok(result.CardHistories.Select(CardHistoryContract.ConvertToContract));

        // this approach is also ok - returning NotFound(404) when data is not found with requested params
        if (result.CardNotExists)
            return NotFound("Card not found");

        return BadRequest(result.Error);
    }
}