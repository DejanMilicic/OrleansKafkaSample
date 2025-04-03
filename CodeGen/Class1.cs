// Orleans generates C# code for serialization so need a C# project to compile the generated code
[assembly: GenerateCodeForDeclaringAssembly(typeof(Grains.HelloGrain))]

// https://learn.microsoft.com/en-us/samples/dotnet/samples/orleans-fsharp-sample/