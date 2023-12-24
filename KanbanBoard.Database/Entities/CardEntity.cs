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
    [Column("created_on")]
    public DateTime CreatedOn { get; set; }
    
    [Required] 
    [Column("updated_on")]
    public DateTime UpdatedOn { get; set; }

    [Required]
    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_on")]
    public DateTime? DeletedOn { get; set; }

    [Required] 
    [Column("title", TypeName = "text")]
    [StringLength(100)]
    public string Title { get; set; }
    
    [Required] 
    [Column("description", TypeName = "text")]
    [StringLength(100)]
    public string Description { get; set; }

    [Required] 
    [Column("priority")]
    public CardPriority Priority { get; set; }

    [Required] 
    [Column("active_list_id")]
    public int ActiveListId { get; set; }
    
    public ListEntity ActiveList { get; set; }

    public HashSet<CardHistoryEntity> CardHistories { get; set; } = new();
}