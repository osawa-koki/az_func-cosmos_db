using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace azfunc_cosmosdb
{
  public partial class Program
  {
    [FunctionName("Insert")]
    [OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
    [OpenApiParameter(name: "profession", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Profession** parameter")]
    [OpenApiParameter(name: "age", In = ParameterLocation.Query, Required = true, Type = typeof(int), Description = "The **Age** parameter")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
    public async Task<IActionResult> Insert(
      [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
    {
      // mongodbに挿入

      // ドキュメントを作成
      var name = req.Query["name"];
      var profession = req.Query["profession"];
      var age = int.Parse(req.Query["age"]);

      Console.WriteLine($"name: {name} profession: {profession} age: {age}");

      var document = new BsonDocument
      {
        { "age", age }
      };


      // コレクションを取得
      var collection = mongo_database!.GetCollection<BsonDocument>("users");

      // ドキュメントを挿入
      collection.InsertOne(document);

      // レスポンスを返す
      return new OkObjectResult("Inserted");
    }
  }
}
