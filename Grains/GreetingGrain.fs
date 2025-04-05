namespace Grains

open System.Threading.Tasks
open Orleans
open System

[<Serializable>]
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
                Task.FromResult($"Greeter received greeting: {greeting}")
            | Number number ->
                Task.FromResult($"Greeter received number: {number}")
