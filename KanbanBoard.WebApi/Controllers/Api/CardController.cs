using KanbanBoard.Core.Enums;
using KanbanBoard.Service.Card;
using KanbanBoard.WebApi.Controllers.Base;
using KanbanBoard.WebApi.RequestModels;
using KanbanBoard.WebApi.ResponseContracts;
using Microsoft.AspNetCore.Mvc;
using GetAllCardsCountModel = KanbanBoard.Service.Card.GetAllCardsCountModel;
using RegisterCardModel = KanbanBoard.Service.Card.RegisterCardModel;

namespace KanbanBoard.WebApi.Controllers.Api;

public class CardController : BaseController
{
    private readonly CardService _cardService;

    public CardController(CardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAllCards([FromQuery] GetAllCardsRequestModel requestModel)
    {
        var result = await _cardService.GetAllCards(new GetAllCardsModel()
        {
            Limit = requestModel.Limit,
            Skip = requestModel.Skip,
            CardPriority = requestModel.CardPriority,
            DateStart = requestModel.DateStart,
            DateEnd = requestModel.DateEnd
        });

        if (result.Success)
            return Ok(result.Cards.Select(CardContract.ConvertToContract));

        return BadRequest(result.Error);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetAllCardsCount([FromQuery] GetAllCardsCountRequestModel requestModel)
    {
        var result = await _cardService.GetAllCardsCount(new GetAllCardsCountModel()
        {
            CardPriority = requestModel.CardPriority,
            DateStart = requestModel.DateStart,
            DateEnd = requestModel.DateEnd
        });

        if (result.Success)
            return Ok(result.Count);

        return BadRequest(result.Error);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCardById(int id)
    {
        var card = await _cardService.GetCardById(id);
        if (card != null)
            return Ok(CardContract.ConvertToContract(card));

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCard([FromBody] RegisterCardRequestModel requestModel)
    {
        var result = await _cardService.RegisterCard(new RegisterCardModel()
        {
            Title = requestModel.Title,
            Description = requestModel.Description,
            CardPriority = requestModel.Priority,
            ListId = requestModel.ListId
        });

        if (result.Success)
            return Ok(CardContract.ConvertToContract(result.CardEntity));

        if (result.ListNotFound)
        {
            return Conflict(new
            {
                ListNotFound = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpPut("{id}/title")]
    public async Task<IActionResult> UpdateTitle(int id, [FromBody] UpdateTitleRequestModel requestModel)
    {
        var result = await _cardService.UpdateTitle(id, requestModel.Title);
        if (result.Success)
            return Ok(CardContract.ConvertToContract(result.CardEntity));

        if (result.CardNotExists)
        {
            return Conflict(new
            {
                CardNotFound = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpPut("{id}/description")]
    public async Task<IActionResult> UpdateDescription(int id, [FromBody] UpdateDescriptionRequestModel requestModel)
    {
        var result = await _cardService.UpdateDescription(id, requestModel.Description);
        if (result.Success)
            return Ok(CardContract.ConvertToContract(result.CardEntity));

        if (result.CardNotExists)
        {
            return Conflict(new
            {
                CardNotFound = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpPut("{id}/priority")]
    public async Task<IActionResult> UpdatePriority(int id, [FromBody] UpdatePriorityRequestModel requestModel)
    {
        var result = await _cardService.UpdatePriority(id, requestModel.CardPriority);
        if (result.Success)
            return Ok(CardContract.ConvertToContract(result.CardEntity));

        if (result.CardNotExists)
        {
            return Conflict(new
            {
                CardNotFound = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpPut("{id}/move")]
    public async Task<IActionResult> MoveToList(int id, [FromBody] MoveCardRequestModel requestModel)
    {
        var result = await _cardService.MoveToList(id, requestModel.SourceListId, requestModel.TargetListId);
        if (result.Success)
            return Ok(CardContract.ConvertToContract(result.CardEntity));

        if (result.CardNotExists)
        {
            return Conflict(new
            {
                CardNotFound = true
            });
        }

        if (result.SourceListNotExists)
        {
            return Conflict(new
            {
                SourceListNotFound = true
            });
        }

        if (result.TargetListNotExists)
        {
            return Conflict(new
            {
                TargetListNotFound = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(int id)
    {
        var result = await _cardService.DeleteCard(id);
        if (result.Success)
            return Ok();

        if (result.CardNotExists)
        {
            return Conflict(new
            {
                CardNotFound = true
            });
        }

        return BadRequest(result.Error);
    }
}