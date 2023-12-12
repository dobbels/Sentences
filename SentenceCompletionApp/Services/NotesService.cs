using Microsoft.Azure.Cosmos;
using SentenceCompletionApp.Model;

namespace SentenceCompletionApp.Services;

public class NotesService
{
    private readonly CosmosClient _cosmosClient;
    private Database? _database;
    private Container? _container;
    private readonly string _containerId = "Notes";
    private readonly IConfiguration _configuration;

    public NotesService(IConfiguration configuration)
    {
        var endpointUri = configuration["CosmosDb:EndpointUri"] ?? "Empty endpoint URI";
        var primaryKey = configuration["CosmosDb:PrimaryKey"] ?? "Empty primary key";
        _cosmosClient = new CosmosClient(endpointUri, primaryKey, new CosmosClientOptions() { ApplicationName = "SentenceCompletionApp" });
        _configuration = configuration;
    }

    public async Task InitializeAsync()
    {
        _database = await _cosmosClient.CreateDatabaseIfNotExistsAsync(_configuration["CosmosDb:DatabaseId"] ?? "EmptyDatabaseId");
        _container = await _database.CreateContainerIfNotExistsAsync(_containerId, $"/{nameof(NoteSubmission.MonthOfSubmission)}");
    }

    public async Task PersistNotesAsync(string notes)
    {
        var note = notes;
        await AddToContainerAsync(note);
    }

    private async Task AddToContainerAsync(string note)
    {
        if (_container == null)
        {
            throw new InvalidProgramException($"Container should have been initialized before calling the method {nameof(AddToContainerAsync)}");
        }

        var noteSubmission = new NoteSubmission(note);
        
        await _container.CreateItemAsync(noteSubmission, new PartitionKey(noteSubmission.MonthOfSubmission));
    }
}
