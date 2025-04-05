namespace Grains

open System.Threading.Tasks
open Orleans

// following three lines are necessary to make the Orleans codegen work
// "CodeGen" is the name of the C# codegen project
open System.Runtime.CompilerServices
[<assembly: InternalsVisibleTo("CodeGen")>]
do()

[<GenerateSerializer>]
type GreeterMessage =
    | Greeting of string
    | Number of int

type IGreetingGrain =
    inherit IGrainWithStringKey
    abstract member Receive : greeting:GreeterMessage -> Task<string>

type GreetingGrain() = 
    inherit Grain()
    
    interface IGreetingGrain with
        member this.Receive(msg: GreeterMessage) =
            match msg with
            | Greeting greeting ->
                Task.FromResult($"Greeter received greeting: *{greeting}*")
            | Number number ->
                Task.FromResult($"Greeter received number: {number}")
