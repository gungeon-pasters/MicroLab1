namespace MicroLab1.Models
{
    public class CreateTrainingDto
    {
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public DateTime TrainingDate { get; set; }
        public int Approaches { get; set; }
        public int Weights { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}