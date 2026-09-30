using MicroLab1.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MicroLab1.Services
{
    public class MongoDbService
    {
        private readonly IMongoCollection<MongoTraining> _trainingsCollection;

        public MongoDbService(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _trainingsCollection = database.GetCollection<MongoTraining>(settings.Value.CollectionName);
        }

        public IMongoCollection<MongoTraining> Trainings => _trainingsCollection;
    }
}