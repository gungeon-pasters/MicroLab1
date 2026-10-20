using System.ComponentModel.DataAnnotations;

namespace MicroLab1.Models
{
    public class CreateTrainingDto
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;

        [Range(1, 600, ErrorMessage = "Длительность должна быть от 1 до 600 минут.")]
        public int DurationMinutes { get; set; }
        public DateTime TrainingDate { get; set; }
        [Range(0, 100, ErrorMessage = "Число подходов должно быть от 0 до 100.")]
        public int Approaches { get; set; }

        [Range(0, 1000, ErrorMessage = "Вес должен быть от 0 до 1000 кг.")]
        public int Weights { get; set; }


    }
}