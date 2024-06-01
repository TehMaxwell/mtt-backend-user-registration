/*
AUTHOR: Sam Maxwell
DATE CREATED: 23/03/2024
DESCRIPTION: Top level execution file for the user-registration-requested-consumer.
*/

using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTT.Configuration.Kafka;
using MTT.UserRegistrationRequested;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// ADD SERVICE REGISTRATION CODE BETWEEN COMMENTS

// Registration of Configuration Services
builder.Services.Configure<Topics>(builder.Configuration.GetSection("Topics"));
builder.Services.Configure<ConsumerConfiguration>(builder.Configuration.GetSection("ConsumerConfiguration"));
builder.Services.Configure<ProducerConfiguration>(builder.Configuration.GetSection("ProducerConfiguration"));

// Registration of Kafka Services
builder.Services.AddScoped<UserRegistrationRequestedConsumer>();

// ADD SERVICE REGISTRATION CODE BETWEEN COMMENTS

IHost host = builder.Build();

UserRegistrationRequestedConsumer consumer = host.Services.GetRequiredService<UserRegistrationRequestedConsumer>();
await consumer.Start();


await host.RunAsync();      // Running the application code
