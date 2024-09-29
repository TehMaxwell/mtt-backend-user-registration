/*
AUTHOR: Sam Maxwell
DATE CREATED: 01/06/2024
DESCRIPTION: A configuration class for the Kafka Consumer section.
*/

namespace MTT.Configuration.Kafka {
    /// <summary>
    /// A configuration class for the Kafka Consumer section.
    /// </summary>
    public class ConsumerConfiguration
    {
        /// <summary>
        /// The connection string of the Kafka Server: {domain_name}:{port}.
        /// </summary>
        public string BootstrapServers { get; set; }

        /// <summary>
        /// The Kafka Group ID of the consumer.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Sets the offset that the consumer should start reading for in the event that there are no commited offsets for the partition.
        /// </summary>
        public string AutoOffsetReset { get; set; }

        /// <summary>
        /// Enables (true) auto commit of offsets. Disabling allows for "at-least once" functionality in the consumer
        /// </summary>
        public bool EnableAutoCommit { get; set; }
    }
}
