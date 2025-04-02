namespace Grains

open System.Threading.Tasks
open Orleans

type IHelloGrain =
    inherit IGrainWithStringKey
    
    /// Says hello to the caller
    abstract member SayHello : greeting:string -> Task<string>

// The grain implementation
type HelloGrain() = 
    inherit Grain()
    
    interface IHelloGrain with
        member this.SayHello(greeting:string) =
            Task.FromResult($"Hello from F#! You said: {greeting}")
