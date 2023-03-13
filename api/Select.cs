using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace azfunc_cosmosdb
{
  public partial class Program
  {
    [FunctionName("Select")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "user" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = false, Type = typeof(string), Description = "The **id** parameter")]
    [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The **name** parameter")]
    [OpenApiParameter(name: "profession", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The **profession** parameter")]
    [OpenApiParameter(name: "age", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "The **age** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Select(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
    {
      string id = req.Query["id"];
      string name = req.Query["name"];
      string profession = req.Query["profession"];
      string age = req.Query["age"];

      var filterBuilder = Builders<BsonDocument>.Filter;
      var filter = filterBuilder.Empty;

      if (id != null)
      {
        filter = filter & filterBuilder.Eq("_id", new ObjectId(id));
      }
      if (name != null)
      {
        filter = filter & filterBuilder.Eq("name", name);
      }
      if (profession != null)
      {
        filter = filter & filterBuilder.Eq("profession", profession);
      }
      if (age != null)
      {
        filter = filter & filterBuilder.Eq("age", int.Parse(age));
      }

      List<BsonDocument> documents = await users_collection.Find(filter).ToListAsync();

      List<User> users = documents.Select(document => new User
      {
        Id = document["_id"].ToString(),
        Name = document["name"].ToString(),
        Profession = document["profession"].ToString(),
        Age = int.Parse(document["age"].ToString())
      }).ToList();

      return new OkObjectResult(users);
    }
  }
}
