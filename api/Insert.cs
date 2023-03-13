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
  public struct User
  {
    public string Name { get; set; }
    public string Profession { get; set; }
    public int Age { get; set; }
  }

  public partial class Program
  {
    [FunctionName("Insert")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "user" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "The **User** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Insert(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
      // mongodbに挿入

      // ドキュメントを作成
      var user = JsonConvert.DeserializeObject<User>(await new StreamReader(req.Body).ReadToEndAsync());
      var id = ObjectId.GenerateNewId();

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
      return new OkObjectResult(new {
        id = id.ToString(),
        user.Name,
        user.Profession,
        user.Age,
      });
    }
  }
}
