namespace KanbanBoard.Core.Enums;

[Flags]
public enum CardHistoryType
{
    Created,            /// 0
    Deleted,            /// 1
    UpdatedTitle,       /// 2
    UpdatedDescription, /// 4
    UpdatedPriority,    /// 16
    MovedToList         /// 32
}