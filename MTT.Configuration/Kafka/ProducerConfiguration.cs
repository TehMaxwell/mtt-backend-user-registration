/*
AUTHOR: Sam Maxwell
DATE CREATED: 01/06/2024
DESCRIPTION: A configuration class for the Kafka Producer section.
*/

namespace MTT.Configuration.Kafka {
    /// <summary>
    /// A configuration class for the Kafka Producer section.
    /// </summary>
    public class ProducerConfiguration
    {
        /// <summary>
        /// The connection string of the Kafka Server: {domain_name}:{port}.
        /// </summary>
        public string BootstrapServers { get; set; }
    }
}
