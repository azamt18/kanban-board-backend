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
    [Column("status")]
    public CardStatus Status { get; set; }

    [Required] 
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Required] 
    [Column("card_id")]
    public int CardId { get; set; }
    
    public virtual CardEntity Card { get; set; }
}