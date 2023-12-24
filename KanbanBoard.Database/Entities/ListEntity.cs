using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KanbanBoard.Database.Entities;

[Table("lists")]
public class ListEntity
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
    [Column("is_closed")]
    public bool IsClosed { get; set; }

    [Column("closed_on")]
    public DateTime? ClosedOn { get; set; }
    
    [Required] 
    [Column("title", TypeName = "text")]
    [StringLength(100)]
    public string Title { get; set; }

    public ICollection<CardEntity> Cards { get; set; } = new List<CardEntity>();
}