using KanbanBoard.Database.Entities;

namespace KanbanBoard.Service.List;

public record struct RegisterListModel
{
    public string Title { get; set; }
}
public record struct RegisterListResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public ListEntity ListEntity { get; set; }
}

public record struct UpdateListModel
{
    public string Title { get; set; }
}
public record struct UpdateListResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public ListEntity ListEntity { get; set; }
    public bool ListNotExists { get; set; }
}

public record struct DeleteListResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public bool ListNotExists { get; set; }
}