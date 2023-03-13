using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace azfunc_cosmosdb
{
  public class User
  {
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    [Required(ErrorMessage = "The name field is required.")]
    public string Name { get; set; }

    [JsonProperty("profession")]
    [Required(ErrorMessage = "The profession field is required.")]
    public string Profession { get; set; }

    [JsonProperty("age")]
    [Required(ErrorMessage = "The age field is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "The age must be zero or a positive integer.")]
    public int Age { get; set; }
  }
}
