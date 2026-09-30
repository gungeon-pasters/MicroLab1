using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MicroLab1.Models
{
    public class MongoTraining
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string Title { get; set; } = string.Empty;          // название
        public string Type { get; set; } = string.Empty;           // тип (Кардио/Силовая/Гибкость)
        public int DurationMinutes { get; set; }                   // длительность
        public DateTime TrainingDate { get; set; }                 // дата
        public int Approaches { get; set; }                        // подходы
        public int Weights { get; set; }                           // вес
        public string TrainerName { get; set; } = string.Empty;    // тренер
        public string Description { get; set; } = string.Empty;    // описание
    }
}