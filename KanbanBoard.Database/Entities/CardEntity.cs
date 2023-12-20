using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KanbanBoard.Core.Enums;

namespace KanbanBoard.Database.Entities;

[Table("cards")]
public class CardEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required] 
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Required] 
    [Column("title", TypeName = "text")]
    [StringLength(100)]
    public string Title { get; set; }
    
    [Required] 
    [Column("description", TypeName = "text")]
    [StringLength(100)]
    public string Description { get; set; }

    [Required] 
    [Column("active_status")]
    public CardStatus ActiveStatus { get; set; }

    [Required] 
    [Column("board_id")]
    public int BoardId { get; set; }
    
    public BoardEntity Board { get; set; }
    
    public List<CardHistoryEntity> History { get; set; }
}