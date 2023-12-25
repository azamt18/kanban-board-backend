using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace KanbanBoard.WebApi.RequestModels;

public class RegisterListRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonProperty("title")]
    public string Title { get; set; }
}

public class UpdateListRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonProperty("title")]
    public string? Title { get; set; }
}