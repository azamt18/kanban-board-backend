using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using KanbanBoard.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.WebApi.RequestModels;

public class GetAllCardsRequestModel
{
    [FromQuery(Name = "dateStart")] public string? DateStart { get; set; }
    [FromQuery(Name = "dateEnd")] public string? DateEnd { get; set; }
    [FromQuery(Name = "priority")] public CardPriority? CardPriority { get; set; }
    [FromQuery(Name = "skip")] public int? Skip { get; set; }
    [FromQuery(Name = "skip")] public int? Limit { get; set; }
}

public class GetAllCardsCountRequestModel
{
    [FromQuery(Name = "dateStart")] public string? DateStart { get; set; }
    [FromQuery(Name = "dateEnd")] public string? DateEnd { get; set; }
    [FromQuery(Name = "priority")] public CardPriority? CardPriority { get; set; }
}

public class RegisterCardRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [Required]
    [JsonPropertyName("priority")]
    public CardPriority Priority { get; set; }

    [Required]
    [JsonPropertyName("listId")]
    public int ListId { get; set; }
}

public class UpdateTitleRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("title")]
    public string Title { get; set; }
}
public class UpdateDescriptionRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonPropertyName("description")]
    public string Description { get; set; }
}
public class UpdatePriorityRequestModel
{
    [Required]
    [JsonPropertyName("priority")]
    public CardPriority CardPriority { get; set; }
}

public class MoveCardRequestModel
{
    [Required]
    [JsonPropertyName("sourceListId")]
    public int SourceListId { get; set; }
    
    [Required]
    [JsonPropertyName("targetListId")]
    public int TargetListId { get; set; }
}
