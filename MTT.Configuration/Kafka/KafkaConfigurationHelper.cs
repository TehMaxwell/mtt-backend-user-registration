/*
AUTHOR: Sam Maxwell
DATE CREATED: 01/06/2024
DESCRIPTION: A static helper class for building Kafka Configuration objects.
*/

using Confluent.Kafka;

namespace MTT.Configuration.Kafka
{
    /// <summary>
    /// A static helper class for building Kafka Configuration objects.
    /// </summary>
    public static class KafkaConfigurationHelper
    {
        // METHODS
        /// <summary>
        /// Generates a Kafka ConsumerConfig object based upon an appsettings.json configuration file object.
        /// </summary>
        /// <param name="baseConsumerConfiguration">The appsettings.json configuration object containing the Kafka Consumer configuration.</param>
        /// <returns>A Kafka ConsumerConfig object with the passed configuration values.</returns>
        public static ConsumerConfig BuildConsumerConfig(ConsumerConfiguration baseConsumerConfiguration) {
            // Generating the AutoOffsetReset value
            AutoOffsetReset autoOffsetReset;
            switch(baseConsumerConfiguration.AutoOffsetReset) {
                case "Earliest":
                    autoOffsetReset = AutoOffsetReset.Earliest;
                    break;
                case "Latest":
                    autoOffsetReset = AutoOffsetReset.Latest;
                    break;
                case "Error":
                    autoOffsetReset = AutoOffsetReset.Error;
                    break;
                default:
                    autoOffsetReset = AutoOffsetReset.Earliest;
                    break;
            }

            // Creating the ConsumerConfig object
            ConsumerConfig consumerConfig = new ConsumerConfig
            {
                BootstrapServers = baseConsumerConfiguration.BootstrapServers,
                GroupId = baseConsumerConfiguration.GroupId,
                AutoOffsetReset = autoOffsetReset,
                EnableAutoCommit = baseConsumerConfiguration.EnableAutoCommit
            };

            return consumerConfig;
        }

        /// <summary>
        /// Generates a Kafka ProducerConfig object based upon an appsettings.json configuration file object.
        /// </summary>
        /// <param name="baseProducerConfiguration">The appsettings.json configuration object containing the Kafka Producer configuration.</param>
        /// <returns>A Kafka ProducerConfig object with the passed configuration values.</returns>
        public static ProducerConfig BuildProducerConfig(ProducerConfiguration baseProducerConfiguration)
        {
            // Creating the ProducerConfig object
            ProducerConfig producerConfig = new ProducerConfig
            {
                BootstrapServers = baseProducerConfiguration.BootstrapServers
            };

            return producerConfig;
        }
    }
}

