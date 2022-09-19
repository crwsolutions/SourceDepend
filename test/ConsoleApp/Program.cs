using Microsoft.Extensions.DependencyInjection;
using static System.Console;

var services = new ServiceCollection();
services.Discover();
var serviceProvider = services.BuildServiceProvider();
var exampleService = serviceProvider.GetRequiredService<ConsoleApp.ExampleService>();
WriteLine(exampleService.GetValue());
WriteLine(exampleService.GetConstructValue());
