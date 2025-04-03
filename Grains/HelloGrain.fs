// https://medium.com/@alexander.prooks/orleans-with-f-story-6036045f1f10
// https://github.com/OrleansContrib/Orleankka/blob/0b933e317cd97e99fc8c055de8649faffb0559c7/Samples/FSharp/Shop/Account.fs#L21
// https://github.com/OrleansContrib/Orleankka/issues/154
// https://github.com/AntyaDev/FOrleans
// https://github.com/aprooks/Orleankka/blob/909b6bac11238bbe9c6dad2ea4e75485ec6dc0a3/Source/FSharp.Demo.Shop/Program.fs

// 
namespace Grains

open Orleankka
open Orleankka.FSharp
open Orleans
open System.Threading.Tasks
open System

// Define the actor interface - this must extend IActorGrain for Orleankka
type IHelloGrain =
    inherit IActorGrain  // This is the Orleankka actor interface
    inherit IGrainWithStringKey  

[<Serializable>]
type HelloMessage = 
    | Greeting of string
    interface ActorMessage<IHelloGrain>

// Implement the grain
type HelloGrain() =
    inherit ActorGrain()  // This is the Orleankka base class
    interface IHelloGrain

    // Check which method signature your version of Orleankka uses
    override this.Receive(message) =
        task {
            match message with
            | :? HelloMessage as msg ->
                printfn "Hello"
                return box "Greeting processed"
            | _ -> return TaskResult.Unhandled
        }