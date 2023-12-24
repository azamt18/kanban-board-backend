using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KanbanBoard.Core.Enums;

namespace KanbanBoard.Database.Entities;

[Table("card_histories")]
public class CardHistoryEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] 
    [Column("card_id")]
    public int CardId { get; set; }

    public virtual CardEntity Card { get; set; }

    [Required] 
    [Column("created_on")]
    public DateTime CreatedOn { get; set; }

    [Required] 
    [Column("type")]
    public CardHistoryType Type { get; set; }

    /// <summary>
    /// if the card was moved from one board to another
    /// why nullable? because, in the future it is possible to add other actions inside card itself, like adding comments, assigning to board members, etc.
    /// </summary>
    [Column("moved_source_list_id")]
    public int? MovedSourceListId { get; set; }

    [Column("moved_target_list_id")]
    public int? MovedTargetListId { get; set; }
}