using KanbanBoard.Database.Entities;

namespace KanbanBoard.Service.CardHistory;

public record struct CardHistoryResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public bool CardNotExists { get; set; }
    public HashSet<CardHistoryEntity> CardHistories { get; set; }
}