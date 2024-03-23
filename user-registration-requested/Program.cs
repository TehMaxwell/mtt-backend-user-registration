/*
AUTHOR: Sam Maxwell
DATE CREATED: 23/03/2024
DESCRIPTION: Top level execution file for the user-registration-requested-consumer.
*/

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// ADD SERVICE REGISTRATION CODE HERE

IHost host = builder.Build();

await host.RunAsync();      // Running the application code
