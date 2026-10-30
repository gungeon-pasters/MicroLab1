using Prometheus;

namespace MicroLab1.Metrics
{
    public static class MongoTrainingMetrics
    {
        public static readonly Counter OperationsTotal =
            Prometheus.Metrics.CreateCounter(
                "mongo_training_operations_total",
                "Количество операций с тренировками во втором сервисе.",
                new CounterConfiguration
                {
                    LabelNames = new[] { "operation", "result" }
                });

        public static readonly Counter DatabaseOperationsTotal =
            Prometheus.Metrics.CreateCounter(
                "mongo_database_operations_total",
                "Количество обращений второго сервиса к MongoDB.",
                new CounterConfiguration
                {
                    LabelNames = new[] { "operation", "result" }
                });
    }
}