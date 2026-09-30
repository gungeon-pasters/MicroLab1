using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MicroLab1.Models;
using MicroLab1.Services;

namespace MicroLab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly IMongoCollection<MongoTraining> _trainings;

        public TrainingsController(MongoDbService service)
        {
            _trainings = service.Trainings;
        }

        // GET: api/Trainings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingDto>>> GetTrainings()
        {
            var count = await _trainings.CountDocumentsAsync(_ => true);
            List<MongoTraining> trainings;

            if (count > 1000)
            {
                // Случайная выборка 1000 документов через агрегатор $sample
                trainings = await _trainings.Aggregate()
                    .Sample(1000)
                    .ToListAsync();
            }
            else
            {
                trainings = await _trainings.Find(_ => true).ToListAsync();
            }

            return Ok(trainings.Select(ToDto));
        }

        // GET: api/Trainings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingDto>> GetTraining(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Некорректный формат идентификатора.");

            var training = await _trainings.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (training == null) return NotFound();

            return Ok(ToDto(training));
        }

        // GET: api/Trainings/filter?type=Кардио&trainerName=Иванов&year=2025
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<TrainingDto>>> FilterTrainings(
            [FromQuery] string? type,
            [FromQuery] string? trainerName,
            [FromQuery] int? year)
        {
            var fb = Builders<MongoTraining>.Filter;
            var filters = new List<FilterDefinition<MongoTraining>>();

            if (!string.IsNullOrEmpty(type))
                filters.Add(fb.Eq(t => t.Type, type));
            if (!string.IsNullOrEmpty(trainerName))
                filters.Add(fb.Eq(t => t.TrainerName, trainerName));
            if (year.HasValue)
                filters.Add(fb.Gte(t => t.TrainingDate, new DateTime(year.Value, 1, 1)) &
                            fb.Lt(t => t.TrainingDate, new DateTime(year.Value + 1, 1, 1)));

            var final = filters.Any() ? fb.And(filters) : fb.Empty;
            var trainings = await _trainings.Find(final).ToListAsync();
            return Ok(trainings.Select(ToDto));
        }

        // POST: api/Trainings
        [HttpPost]
        public async Task<IActionResult> PostTraining(CreateTrainingDto dto)
        {
            var training = new MongoTraining
            {
                Title = dto.Title,
                Type = dto.Type,
                DurationMinutes = dto.DurationMinutes,
                TrainingDate = dto.TrainingDate,
                Approaches = dto.Approaches,
                Weights = dto.Weights,
                TrainerName = dto.TrainerName,
                Description = dto.Description
            };

            await _trainings.InsertOneAsync(training);
            return CreatedAtAction(nameof(GetTraining), new { id = training.Id }, ToDto(training));
        }

        // PUT: api/Trainings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTraining(string id, CreateTrainingDto dto)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Некорректный формат идентификатора.");

            var existing = await _trainings.Find(t => t.Id == id).FirstOrDefaultAsync();
            if (existing == null) return NotFound();

            var updated = new MongoTraining
            {
                Id = id,
                Title = dto.Title,
                Type = dto.Type,
                DurationMinutes = dto.DurationMinutes,
                TrainingDate = dto.TrainingDate,
                Approaches = dto.Approaches,
                Weights = dto.Weights,
                TrainerName = dto.TrainerName,
                Description = dto.Description
            };

            await _trainings.ReplaceOneAsync(t => t.Id == id, updated);
            return NoContent();
        }

        // DELETE: api/Trainings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("Некорректный формат идентификатора.");

            var result = await _trainings.DeleteOneAsync(t => t.Id == id);
            if (result.DeletedCount == 0) return NotFound();

            return NoContent();
        }

        // DELETE: api/Trainings/all
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllTrainings()
        {
            var result = await _trainings.DeleteManyAsync(_ => true);
            if (result.DeletedCount == 0)
                return NotFound("В базе нет тренировок для удаления.");

            return NoContent();
        }

        private static TrainingDto ToDto(MongoTraining t) => new TrainingDto
        {
            Id = t.Id!,
            Title = t.Title,
            Type = t.Type,
            DurationMinutes = t.DurationMinutes,
            TrainingDate = t.TrainingDate,
            Approaches = t.Approaches,
            Weights = t.Weights,
            TrainerName = t.TrainerName,
            Description = t.Description
        };
    }
}