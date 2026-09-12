using Microsoft.EntityFrameworkCore;
using MicroLab1.Models;

namespace MicroLab1.Models
{
    public class TrainingContext : DbContext
    {
        public TrainingContext(DbContextOptions<TrainingContext> options) : base(options) { }
        public DbSet<MicroLab1.Models.Training> Training { get; set; } = default!;
    }
}
