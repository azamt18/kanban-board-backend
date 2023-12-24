namespace KanbanBoard.Core.Enums;

[Flags]
public enum CardHistoryType
{
    Created = 0,            /// 0
    Deleted = 1,            /// 1
    UpdatedTitle = 2,       /// 2
    UpdatedDescription = 3, /// 4
    UpdatedPriority = 4,    /// 16
    MovedToList = 5         /// 32
}