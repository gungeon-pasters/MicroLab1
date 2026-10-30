using Prometheus;

namespace MicroLab1.Metrics
{
    public static class TrainingMetrics
    {
        public static readonly Counter OperationsTotal =
            Prometheus.Metrics.CreateCounter(
                "training_operations_total",
                "Количество операций с тренировками.",
                new CounterConfiguration
                {
                    LabelNames = new[] { "operation", "result" }
                });
    }
}