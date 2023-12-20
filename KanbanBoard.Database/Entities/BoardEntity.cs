using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KanbanBoard.Database.Entities;

[Table("boards")]
public class BoardEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required] 
    [Column("name", TypeName = "text")]
    [StringLength(100)]
    public string Name { get; set; }

    public ICollection<CardEntity> Cards { get; set; } = new List<CardEntity>();
}