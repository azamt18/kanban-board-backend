using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using KanbanBoard.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KanbanBoard.WebApi.RequestModels;

public class GetAllCardsRequestModel
{
    [FromQuery(Name = "dateStart")] public string? DateStart { get; set; }
    [FromQuery(Name = "dateEnd")] public string? DateEnd { get; set; }
    [FromQuery(Name = "priority")] public CardPriority? CardPriority { get; set; }
    [FromQuery(Name = "skip")] public int? Skip { get; set; }
    [FromQuery(Name = "limit")] public int? Limit { get; set; }
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
    [JsonProperty("title")]
    public string Title { get; set; }

    [Required(AllowEmptyStrings = false)]
    [JsonProperty("description")]
    public string Description { get; set; }
    
    [Required]
    [JsonProperty("priority")]
    public CardPriority Priority { get; set; }

    [Required]
    [JsonProperty("listId")]
    public int ListId { get; set; }
}

public class UpdateTitleRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonProperty("title")]
    public string Title { get; set; }
}
public class UpdateDescriptionRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonProperty("description")]
    public string Description { get; set; }
}
public class UpdatePriorityRequestModel
{
    [Required]
    [JsonProperty("priority")]
    public CardPriority CardPriority { get; set; }
}

public class MoveCardRequestModel
{
    [Required]
    [JsonProperty("sourceListId")]
    public int SourceListId { get; set; }
    
    [Required]
    [JsonProperty("targetListId")]
    public int TargetListId { get; set; }
}
