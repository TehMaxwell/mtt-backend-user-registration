/*
AUTHOR: Sam Maxwell
DATE CREATED: 23/03/2024
DESCRIPTION: The Kafka consumer class for the user-registration-requested consumer.
*/

using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace MTT.UserRegistrationRequested
{
    /// <summary>
    /// The Kafka consumer class for the user-registration-requested consumer.
    /// </summary>
    public class UserRegistrationRequestedConsumer
    {
        // PROPERTIES
        string consumerTopic = "user-registration-requested";
        string producerTopic = "user-registration-completed";
        Guid containerGuid = Guid.NewGuid();

        // Kafka Consumer Configuration
        // ConsumerConfig consumerConfig = new ConsumerConfig
        // {
        //     BootstrapServers = "Kafka.Kafka:9092",
        //     GroupId = "user-registration-requested-group",
        //     AutoOffsetReset = AutoOffsetReset.Earliest,
        //     EnableAutoCommit = false        // Disables auto commiting, allowing for "at-least once" functionality in the consumer
        // };
        ConsumerConfig consumerConfig;

        // Kafka Producer Configuration
        // ProducerConfig producerConfig = new ProducerConfig 
        // {
        //     BootstrapServers = "Kafka.Kafka:9092"
        // };
        ProducerConfig producerConfig;

        // METHODS
        /// <summary>
        /// Constructor. Performs dependency injection activities.
        /// </summary>
        public UserRegistrationRequestedConsumer(IOptions<ConsumerConfig> consumerConfigContainer, IOptions<ProducerConfig> producerConfigContainer) {
            consumerConfig = consumerConfigContainer.Value;
            producerConfig = producerConfigContainer.Value;
        }

        /// <summary>
        /// Starts the Kafka consumption loop and executes the relevant code each time a message is available.
        /// </summary>
        public async Task Start() {
            // Building the consumer with the "using" structure. Ensures correct disposal of consumer resources at the end of execution.
            using (IConsumer<Ignore, string> consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build()) {
                using (IProducer<Null, string> producer = new ProducerBuilder<Null, string>(producerConfig).Build()) {
                    consumer.Subscribe(consumerTopic);      // Subscribing to the configured topic
                    CancellationToken cancellationToken = new CancellationToken();      // Creating the cancellation token for the consumer

                    Console.WriteLine($"Consumer with GUID {containerGuid} has subscribed to topic {consumerTopic}");

                    while(!cancellationToken.IsCancellationRequested) {
                        ConsumeResult<Ignore, string> consumeResult = consumer.Consume(cancellationToken);

                        // HANDLE THE CONSUMED MESSAGE BETWEEN THESE COMMENTS
                        
                        Console.WriteLine($"Consumer: {containerGuid}, Message received: {consumeResult.Message}");
                        await Task.Delay(3000);

                        // Producing a new message
                        Message<Null, string> produceMessage = new Message<Null, string> {
                            Value = $"Message from Container {containerGuid}: hello there!"
                        };
                        await producer.ProduceAsync(producerTopic, produceMessage);

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

            return;
        }
    }
}


