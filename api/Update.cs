using System.IO;
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
using Newtonsoft.Json;

namespace azfunc_cosmosdb
{
  public struct UserUpdate
  {
    public string Name { get; set; }
    public string Profession { get; set; }
    public int Age { get; set; }
  }

  public partial class Program
  {
    [FunctionName("Update")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "user" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **id** parameter")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(UserUpdate), Required = true, Description = "The **UserUpdate** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Update(
      [HttpTrigger(AuthorizationLevel.Function, "put", Route = "users/{id}")] HttpRequest req,
      string id)
    {
      var filterBuilder = Builders<BsonDocument>.Filter;
      var filter = filterBuilder.Eq("_id", new ObjectId(id));

      var user = JsonConvert.DeserializeObject<UserInsert>(await new StreamReader(req.Body).ReadToEndAsync());

      var updateBuilder = Builders<BsonDocument>.Update;
      var update = updateBuilder
        .Set("name", user.Name)
        .Set("profession", user.Profession)
        .Set("age", user.Age);

      var result = await users_collection.UpdateOneAsync(filter, update);

      if (result.MatchedCount == 0)
      {
        return new NotFoundResult();
      }

      return new OkObjectResult(new {
        id = id,
        name = user.Name,
        profession = user.Profession,
        age = user.Age,
      });
    }
  }
}
