/*
AUTHOR: Sam Maxwell
DATE CREATED: 23/03/2024
DESCRIPTION: The Kafka consumer class for the user-registration-requested consumer.
*/

using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MTT.Configuration.Kafka;

namespace MTT.UserRegistrationRequested
{
    /// <summary>
    /// The Kafka consumer class for the user-registration-requested consumer.
    /// </summary>
    public class UserRegistrationRequestedConsumer
    {
        // PROPERTIES
        Topics kafkaTopics;
        Guid containerGuid = Guid.NewGuid();

        // Kafka Consumer Configuration
        ConsumerConfig consumerConfig;

        // Kafka Producer Configuration
        ProducerConfig producerConfig;

        // METHODS
        /// <summary>
        /// Constructor. Performs dependency injection activities.
        /// </summary>
        public UserRegistrationRequestedConsumer(IOptions<Topics> topicsConfigContainer, IOptions<ConsumerConfiguration> consumerConfigContainer, IOptions<ProducerConfiguration> producerConfigContainer) {
            kafkaTopics = topicsConfigContainer.Value;      // Topic Configuration Setup
            
            // Consumer Config Setup
            consumerConfig = KafkaConfigurationHelper.BuildConsumerConfig(consumerConfigContainer.Value);

            // Producer Config Setup
            producerConfig = KafkaConfigurationHelper.BuildProducerConfig(producerConfigContainer.Value);
        }

        /// <summary>
        /// Starts the Kafka consumption loop and executes the relevant code each time a message is available.
        /// </summary>
        public async Task Start() {
            Console.WriteLine("Version 0.4.1");
            Console.WriteLine($"Starting application with container GUID {containerGuid}");

            Console.WriteLine("\nConsumer configuration:");
            Console.WriteLine($"Bootstrap Server: {consumerConfig.BootstrapServers}");
            Console.WriteLine($"Group ID: {consumerConfig.GroupId}");
            Console.WriteLine($"Auto Offset Reset: {consumerConfig.AutoOffsetReset}");
            Console.WriteLine($"Enable Auto Commit: {consumerConfig.EnableAutoCommit}");
            Console.WriteLine("----------------------\n");

            // Creating the Kafka Consumer builder and configuring it
            ConsumerBuilder<Ignore, string> consumerBuilder = new ConsumerBuilder<Ignore, string>(consumerConfig);
            consumerBuilder.SetPartitionsAssignedHandler((c, assignments) =>
            {
                Console.WriteLine($"Assigned partitions {string.Join(", ", assignments.Select(x => x.Partition.Value))}");
            });

            // Building the consumer with the "using" structure. Ensures correct disposal of consumer resources at the end of execution.
            using (IConsumer<Ignore, string> consumer = consumerBuilder.Build()) {
                using (IProducer<Null, string> producer = new ProducerBuilder<Null, string>(producerConfig).Build()) {
                    consumer.Subscribe(kafkaTopics.ConsumerTopic);      // Subscribing to the configured topic
                    CancellationToken cancellationToken = new CancellationToken();      // Creating the cancellation token for the consumer

                    Console.WriteLine($"Consumer with GUID {containerGuid} has subscribed to topic {kafkaTopics.ConsumerTopic}");

                    while(!cancellationToken.IsCancellationRequested) {
                        Console.WriteLine("Consuming next message, or waiting for new message.");
                        ConsumeResult<Ignore, string> consumeResult = consumer.Consume(cancellationToken);

                        // HANDLE THE CONSUMED MESSAGE BETWEEN THESE COMMENTS
                        
                        Console.WriteLine($"Consumer: {containerGuid}, Message received: {consumeResult.Message}");
                        await Task.Delay(3000);

                        // Creating a message object
                        Message<Null, string> produceMessage = new Message<Null, string> {
                            Value = $"Message from Container {containerGuid}: hello there!"
                        };
                        await producer.ProduceAsync(kafkaTopics.ProducerTopic, produceMessage);     // Producing an output message to the configured topic

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

            Console.WriteLine("Consumer exiting...");

            return;
        }
    }
}


