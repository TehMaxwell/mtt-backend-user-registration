/*
AUTHOR: Sam Maxwell
DATE CREATED: 23/03/2024
DESCRIPTION: The Kafka consumer class for the user-registration-requested consumer.
*/

using Confluent.Kafka;

namespace MTT.UserRegistrationRequested {
    /// <summary>
    /// The Kafka consumer class for the user-registration-requested consumer.
    /// </summary>
    public class UserRegistrationRequestedConsumer
    {
        // PROPERTIES
        string consumerTopic = "user-registration-requested";
        Guid consumerGuid = Guid.NewGuid();

        // Kafka Consumer Configuration
        ConsumerConfig consumerConfig = new ConsumerConfig
        {
            BootstrapServers = "Kafka.Kafka:9092",
            GroupId = "user-registration-requested-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false        // Disables auto commiting, allowing for "at-least once" functionality in the consumer
        };

        // METHODS
        /// <summary>
        /// Starts the Kafka consumption loop and executes the relevant code each time a message is available.
        /// </summary>
        public async Task Start() {
            // Building the consumer with the "using" structure. Ensures correct disposal of consumer resources at the end of execution.
            using (IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build()) {
                consumer.Subscribe(consumerTopic);      // Subscribing to the configured topic
                CancellationToken cancellationToken = new CancellationToken();      // Creating the cancellation token for the consumer

                while(!cancellationToken.IsCancellationRequested) {
                    ConsumeResult<Ignore, string> consumeResult = consumer.Consume(cancellationToken);

                    // HANDLE THE CONSUMED MESSAGE BETWEEN THESE COMMENTS
                    
                    Console.WriteLine($"Consumer: {consumerGuid}, Message received: {consumeResult.Message}");

                    // HANDLE THE CONSUMED MESSAGE BETWEEN THESE COMMENTS

                    // Commiting the new offset to the Kafka Topic, enables the at-least once processing behaviour
                    try {
                        consumer.Commit(consumeResult);
                    }
                    
                    catch (KafkaException exception) {
                        Console.WriteLine($"Commit error: {exception.Error.Reason}");
                    }
                }

                consumer.Close();
            }
        }
    }
}


