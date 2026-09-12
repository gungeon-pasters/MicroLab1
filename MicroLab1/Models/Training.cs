using System.ComponentModel.DataAnnotations.Schema;

namespace MicroLab1.Models
{
    public class Training
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int DurationMinutes { get; set; }
        public DateTime TrainingDate { get; set; }
        public int Approaches { get; set; }
        public int Weights { get; set; }
    }
}
