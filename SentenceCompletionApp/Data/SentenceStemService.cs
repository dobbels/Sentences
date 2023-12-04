using Microsoft.Azure.Cosmos;
using System.Net;

namespace SentenceCompletionApp.Data;

public class SentenceStemService
{
    private readonly string _endpointUri;
    private readonly string _primaryKey;
    private readonly CosmosClient _cosmosClient;
    private Database? _database;
    private Container? _container;
    private readonly string _databaseId = "ThoughtOutput";
    private readonly string _containerId = "Sentences";

    public SentenceStemService(IConfiguration configuration)
    {
        _endpointUri = configuration["CosmosDb:EndpointUri"] ?? "Empty endpoint URI";
        _primaryKey = configuration["CosmosDb:PrimaryKey"] ?? "Empty primary key";
        _cosmosClient = new CosmosClient(_endpointUri, _primaryKey, new CosmosClientOptions() { ApplicationName = "SentenceCompletionApp" });
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
        await CreateDatabaseAsync();
        await CreateContainerAsync();

        var sentenceSubmission = new SentenceSubmission(sentenceSubmissionDto);
        await AddToContainerAsync(sentenceSubmission);
    }

    private async Task CreateDatabaseAsync()
    {
        if (_database == null)
        {
            _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId);
            Console.WriteLine("Created Database: {0}\n", _database.Id);
        }
    }

    private async Task CreateContainerAsync()
    {
        if (_container == null && _database != null)
        {
            _container = await _database.CreateContainerIfNotExistsAsync(_containerId, $"/{nameof(SentenceSubmission.SentenceStemText)}");
            Console.WriteLine("Created Container: {0}\n", _container.Id);
        }
    }

    private async Task AddToContainerAsync(SentenceSubmission sentenceSubmission)
    {
        if (_container == null)
        {
            throw new InvalidProgramException($"Container should have been initialized before calling the method {nameof(AddToContainerAsync)}");
        }

        try
        {
            var sentenceSubmissionResponse = await _container.ReadItemAsync<SentenceSubmission>(sentenceSubmission.Id, new PartitionKey(sentenceSubmission.SentenceStemText));
            Console.WriteLine("Item in database with id: {0} already exists\n", sentenceSubmissionResponse.Resource.Id);
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            await _container.CreateItemAsync(sentenceSubmission, new PartitionKey(sentenceSubmission.SentenceStemText));
        }
    }
}
