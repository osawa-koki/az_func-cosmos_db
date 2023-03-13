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
using Newtonsoft.Json;

namespace azfunc_cosmosdb
{
  public partial class Program
  {
    [FunctionName("Insert")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "user" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "The **User** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Insert(
      [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
    {
      var user = JsonConvert.DeserializeObject<User>(await new StreamReader(req.Body).ReadToEndAsync());
      var id = ObjectId.GenerateNewId();

      var validationContext = new ValidationContext(user, serviceProvider: null, items: null);
      var validationResults = new List<ValidationResult>();
      bool isValid = Validator.TryValidateObject(user, validationContext, validationResults, true);
      if (isValid == false)
      {
        return new BadRequestObjectResult(validationResults);
      }

      var document = new BsonDocument
      {
        { "_id", id },
        { "name", user.Name },
        { "profession", user.Profession },
        { "age", user.Age }
      };

      // ドキュメントを挿入
      users_collection.InsertOne(document);

      // レスポンスを返す
      return new OkObjectResult(new User
      {
        Id = id.ToString(),
        Name = user.Name,
        Profession = user.Profession,
        Age = user.Age
      });
    }
  }
}
