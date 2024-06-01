/*
AUTHOR: Sam Maxwell
DATE CREATED: 01/06/2024
DESCRIPTION: A model for the Kafka Topic configuration object in the appsettings.json file.
*/

namespace MTT.Configuration.Kafka
{
    /// <summary>
    /// A model for the Kafka Topic configuration object in the appsettings.json file.
    /// </summary>
    public class Topics
    {
        /// <summary>
        /// The topic that the Kafka Consumer is listening to.
        /// </summary>
        public string ConsumerTopic { get; set; }

        /// <summary>
        /// A list of topics that the Kafka Producer publishes to.
        /// </summary>
        public string[] ProducerTopics { get; set; }
    }
}
