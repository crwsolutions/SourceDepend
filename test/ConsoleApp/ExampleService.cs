using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

[Inject]
public partial class ExampleService
{
    [Dependency]
    private readonly AnotherService anotherService;

    ////[Dependency("BindingContext")]
    ////AnotherService viewModel => BindingContext as AnotherService;

    partial void Construct() => anotherService.ConstructValue = "Hello from construct!";

    public string GetValue() => anotherService.Value;
    public string GetConstructValue() => anotherService.ConstructValue;
}

public interface IAnotherService
{
    string Value { get; }
}

[Inject(ServiceLifetime.Singleton)]
public class AnotherService : IAnotherService
{
    public string Value => "Hello World!";
    public string? ConstructValue { get; set; }
}

interface IGeneric { }
interface IGeneric<T> : IGeneric { }
[InjectTransient]
class C : IGeneric<string> { }
