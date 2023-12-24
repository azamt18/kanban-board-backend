using KanbanBoard.Core.Enums;
using KanbanBoard.Database.Entities;

namespace KanbanBoard.Service.Card;

public class GetAllCardsModel
{
    public string? DateStart { get; set; }
    public string? DateEnd { get; set; }
    public CardPriority? CardPriority { get; set; }
    public int? Skip { get; set; }
    public int? Limit { get; set; }
}

public class GetAllCardsResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public IEnumerable<CardEntity> Cards { get; set; }
}

public class GetAllCardsCountModel
{
    public string? DateStart { get; set; }
    public string? DateEnd { get; set; }
    public CardPriority? CardPriority { get; set; }
}

public class GetAllCardsCountResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public int Count { get; set; }
}

public record struct RegisterCardModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public CardPriority CardPriority { get; set; }
    public int ListId { get; set; }
}
public record struct RegisterCardResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public CardEntity CardEntity { get; set; }
    public bool ListNotFound { get; set; }
}

public record struct UpdateCardResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public CardEntity CardEntity { get; set; }
    public bool CardNotExists { get; set; }
}

public record struct MoveCardToListResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public CardEntity CardEntity { get; set; }
    public bool CardNotExists { get; set; }
    public bool SourceListNotExists { get; set; }
    public bool TargetListNotExists { get; set; }
}

public record struct DeleteCardResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public bool CardNotExists { get; set; }
}