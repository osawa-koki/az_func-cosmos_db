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
    public string name { get; set; }
    public string profession { get; set; }
    public int age { get; set; }
  }

  public partial class Program
  {
    [FunctionName("Insert")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "user" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(User), Required = true, Description = "The **Name** parameter")]
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
        { "name", user.name },
        { "profession", user.profession },
        { "age", user.age }
      };

      // コレクションを取得
      var collection = mongo_database!.GetCollection<BsonDocument>("users");

      // ドキュメントを挿入
      collection.InsertOne(document);

      // レスポンスを返す
      return new OkObjectResult(new {
        id = id.ToString(),
        user.name,
        user.profession,
        user.age,
      });
    }
  }
}
