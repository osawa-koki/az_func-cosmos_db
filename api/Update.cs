using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
  public partial class Program
  {
    [FunctionName("Update")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "user" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "The **id** parameter")]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "The **UserUpdate** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Update(
      [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "users/{id}")] HttpRequest req,
      string id)
    {
      var filterBuilder = Builders<BsonDocument>.Filter;
      var filter = filterBuilder.Eq("_id", new ObjectId(id));

      var user = JsonConvert.DeserializeObject<User>(await new StreamReader(req.Body).ReadToEndAsync());

      var validationContext = new ValidationContext(user, serviceProvider: null, items: null);
      var validationResults = new List<ValidationResult>();
      bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);
      if (isValid == false)
      {
        return new BadRequestObjectResult(validationResults);
      }

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
