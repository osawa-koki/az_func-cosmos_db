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
    [FunctionName("Delete")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "user" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **id** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Delete(
      [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "users/{id}")] HttpRequest req,
      string id)
    {
      var filterBuilder = Builders<BsonDocument>.Filter;
      var filter = filterBuilder.Eq("_id", new ObjectId(id));

      var result = await users_collection.DeleteOneAsync(filter);

      if (result.DeletedCount == 0)
      {
        return new NotFoundResult();
      }

      return new OkObjectResult(new {
        id = id,
        message = $"The document with ID '{id}' has been deleted successfully.",
      });
    }
  }
}
