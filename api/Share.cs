
using System;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace azfunc_cosmosdb
{
  public partial class Program
  {
    private readonly MongoClient mongo_client = new(MongoClientSettings.FromUrl(
      new MongoUrl(Environment.GetEnvironmentVariable("COSMOSDB_CONNECTION_STRING"))
    ));

    private readonly ILogger<Program> _logger;
    public Program(ILogger<Program> log)
    {
      _logger = log;
    }
  }
}
