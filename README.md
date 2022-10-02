# Source Depend

A source generator for C# that uses [Roslyn](https://github.com/dotnet/roslyn) (the C# compiler) which saves you from writing the DI plumming in your constructor.
These will be written during compile time.

[![NuGet version (sourcedepend)](https://img.shields.io/nuget/v/sourcedepend?color=blue)](https://www.nuget.org/packages/sourcedepend/)
[![License](https://img.shields.io/github/license/crwsolutions/sourcedepend.svg)](https://github.com/crwsolutions/sourcedepend/blob/master/LICENSE.txt)

## How to use it

Install it and add an attribute to the fields and properties you want be set in your constructor, like so:

```csharp
public partial class ExampleService
{
    [Dependency]
    private readonly AnotherService anotherService;

    [Dependency]
    AnotherService Prop { get; }
}
```

It is also possible to assign to an anternative property

```csharp
public partial class ExampleService
{
    [Dependency(nameof(BindingContext))]
    AnotherService ViewModel => BindingContext as AnotherService;
}
```

Because you constructor is highjacked, there are the alternative methods PreConstruct/PostConstruct to do your construction work

```csharp
public partial class ExampleService
{
    [Dependency]
    private readonly AnotherService anotherService;

    ///This method will be called before the generated field assignments
    partial void PreConstruct()
    {
        Initialize()
    }

    ///This method will be called after the generated field assignments
    partial void PostConstruct() => anotherService.ConstructValue = "Hello from post-construct!";
}
```

These samples give combined the following generated code:

```csharp
pnamespace ConsoleApp
{
    public partial class ExampleService
    {
        public ExampleService(ConsoleApp.IAnotherService anotherService, ConsoleApp.AnotherService prop, ConsoleApp.AnotherService viewModel)
        {
            PreConstruct();

            this.anotherService = anotherService;
            Prop = prop;
            BindingContext = viewModel;

            PostConstruct();
        }

        partial void PreConstruct();
        partial void PostConstruct();
    }
}
```

## Installing

The package is available [on NuGet](https://www.nuget.org/packages/sourcedepend).
To install from the command line:

```shell
dotnet add package sourcedepend
```

Or use the Package Manager in Visual Studio.

## Contributing

The main supported IDE for development is Visual Studio 2019.

Questions, comments, bug reports, and pull requests are all welcome.
Bug reports that include steps to reproduce (including code) are
preferred. Even better, make them in the form of pull requests.
Before you start to work on an existing issue, check if it is not assigned
to anyone yet, and if it is, talk to that person.

## Maintainers/Core team

Contributors can be found at the [contributors](https://github.com/crwsolutions/sourcedepend/graphs/contributors) page on Github.

## License

This software is open source, licensed under the MIT License.
See [LICENSE](https://github.com/crwsolutions/sourcedepend/blob/master/LICENSE) for details.
Check out the terms of the license before you contribute, fork, copy or do anything
with the code. If you decide to contribute you agree to grant copyright of all your contribution to this project and agree to
mention clearly if do not agree to these terms. Your work will be licensed with the project at MIT, along the rest of the code.
