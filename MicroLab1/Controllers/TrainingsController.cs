using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MicroLab1.Models;
using MicroLab1.Metrics;

namespace MicroLab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        private readonly TrainingContext _context;

        public TrainingsController(TrainingContext context)
        {
            _context = context;
        }

        // GET: api/Trainings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingDto>>> GetTraining()
        {
            var trainings = await _context.Training.ToListAsync();
            TrainingMetrics.OperationsTotal.WithLabels("get_all", "success").Inc();

            return Ok(trainings.Select(ToDto));
        }

        // GET: api/Trainings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingDto>> GetTraining(int id)
        {
            var training = await _context.Training.FindAsync(id);

            if (training == null)
            {
                return NotFound();
            }
            TrainingMetrics.OperationsTotal.WithLabels("get_one", "success").Inc();
            return Ok(ToDto(training));
        }

        // PUT: api/Trainings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTraining(int id, CreateTrainingDto trainingDto)
        {
            var training = await _context.Training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }

            training.Title = trainingDto.Title;
            training.Type = trainingDto.Type;
            training.DurationMinutes = trainingDto.DurationMinutes;
            training.TrainingDate = trainingDto.TrainingDate;
            training.Approaches = trainingDto.Approaches;
            training.Weights = trainingDto.Weights;

            if (training == null)
            {
                TrainingMetrics.OperationsTotal
                    .WithLabels("update", "not_found")
                    .Inc();

                return NotFound();
            }

            await _context.SaveChangesAsync();
            TrainingMetrics.OperationsTotal.WithLabels("update", "success").Inc();

            return NoContent();
        }

        // POST: api/Trainings
        [HttpPost]
        public async Task<ActionResult<TrainingDto>> PostTraining(CreateTrainingDto trainingDto)
        {
            var training = new Training
            {
                Title = trainingDto.Title,
                Type = trainingDto.Type,
                DurationMinutes = trainingDto.DurationMinutes,
                TrainingDate = trainingDto.TrainingDate,
                Approaches = trainingDto.Approaches,
                Weights = trainingDto.Weights
            };

            _context.Training.Add(training);
            await _context.SaveChangesAsync();

            TrainingMetrics.OperationsTotal.WithLabels("create", "success").Inc();

            return CreatedAtAction(nameof(GetTraining), new { id = training.Id }, ToDto(training));
        }

        // DELETE: api/Trainings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTraining(int id)
        {
            var training = await _context.Training.FindAsync(id);

            if (training == null)
            {
                TrainingMetrics.OperationsTotal
                    .WithLabels("delete", "not_found")
                    .Inc();

                return NotFound();
            }

            _context.Training.Remove(training);
            await _context.SaveChangesAsync();

            TrainingMetrics.OperationsTotal.WithLabels("delete", "success").Inc();

            return NoContent();
        }

        private static TrainingDto ToDto(Training training)
        {
            return new TrainingDto
            {
                Id = training.Id,
                Title = training.Title,
                Type = training.Type,
                DurationMinutes = training.DurationMinutes,
                TrainingDate = training.TrainingDate,
                Approaches = training.Approaches,
                Weights = training.Weights
            };
        }
    }
}
