using Microsoft.Azure.Cosmos;
using SentenceCompletionApp.Model;

namespace SentenceCompletionApp.Services;

public class SentenceStemService
{
    private readonly CosmosClient _cosmosClient;
    private Database? _database;
    private Container? _container;
    private readonly string _containerId = "Sentences";
    private readonly IConfiguration _configuration;

    public SentenceStemService(IConfiguration configuration)
    {
        var endpointUri = configuration["CosmosDb:EndpointUri"] ?? "Empty endpoint URI";
        var primaryKey = configuration["CosmosDb:PrimaryKey"] ?? "Empty primary key";
        _cosmosClient = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = "SentenceCompletionApp" });
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_configuration["CosmosDb:DatabaseId"] ?? "EmptyDatabaseId");
        _container = await _database.CreateContainerIfNotExistsAsync(_containerId, $"/{nameof(SentenceSubmission.SentenceStemText)}");
    }

    private string[] sentenceStems = new string[] {
        "Today is a great day because...",
        "The best way to start the day is...",
        "I feel happiest when..."
    };

    public Task<SentenceStem> GetNextSentenceStemAsync()
    {
        var random = new Random();
        var currentStem = sentenceStems[random.Next(sentenceStems.Length)];
        return Task.FromResult(new SentenceStem(currentStem));
    }

    public async Task PersistFormedSentenceAsync(SentenceSubmissionDto sentenceSubmissionDto)
    {
        var sentenceSubmission = new SentenceSubmission(sentenceSubmissionDto);
        await AddToContainerAsync(sentenceSubmission);
    }

    private async Task AddToContainerAsync(SentenceSubmission sentenceSubmission)
    {
        if (_container == null)
        {
            throw new InvalidProgramException($"Container should have been initialized before calling the method {nameof(AddToContainerAsync)}");
        }
        
        await _container.CreateItemAsync(sentenceSubmission, new PartitionKey(sentenceSubmission.SentenceStemText));
    }
}
