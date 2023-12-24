using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KanbanBoard.WebApi.RequestModels;

public class RegisterListRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("title")]
    public string Title { get; set; }
}

public class UpdateListRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("title")]
    public string Title { get; set; }
}