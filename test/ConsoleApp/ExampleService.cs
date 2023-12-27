using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp;

[Inject]
internal sealed partial class ExampleService
{
    [Dependency]
    private readonly IAnotherService anotherService;

    protected object BindingContext;

    [Dependency]
    AnotherService Prop { get; }

    [Dependency(nameof(BindingContext))]
    AnotherService ViewModel => BindingContext as AnotherService;

    partial void PreConstruct()
    {
        if (anotherService == null)
        {
            Console.WriteLine("Hello from pre-construct! anotherSerice == null");
        }
    }
    partial void PostConstruct() => anotherService.ConstructValue = "Hello from post-construct!";

    public string GetValue() => anotherService.Value;
    public string? GetConstructValue() => anotherService.ConstructValue;
}

public interface IAnotherService
{
    string Value { get; }

    string? ConstructValue { get; set; }
}

[Inject(ServiceLifetime.Singleton)]
public class AnotherService : IAnotherService
{
    public string Value => "Hello World!";
    public string? ConstructValue { get; set; }
}

