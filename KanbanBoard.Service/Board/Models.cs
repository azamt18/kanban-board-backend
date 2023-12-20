using KanbanBoard.Database.Entities;

namespace KanbanBoard.Service.Board;

public record struct RegisterBoardModel
{
    public string Name { get; set; }
}
public record struct RegisterBoardResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public BoardEntity BoardEntity { get; set; }
}

public record struct UpdateBoardModel
{
    public string Name { get; set; }
}
public record struct UpdateBoardResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public BoardEntity BoardEntity { get; set; }
    public bool BoardExists { get; set; }
}

public record struct DeleteBoardResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public bool BoardExists { get; set; }
}