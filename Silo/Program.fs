namespace OKSilo

open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Orleans
open Orleans.Hosting
open Grains
open Orleankka
open Orleankka.Cluster
open Orleankka
open Orleankka.FSharp
open Microsoft.Extensions.Hosting // Usually already present for IHostBuilder
open Orleans.Hosting            // <-- This is the key one!

module Program =
    [<EntryPoint>]
    let main args =
        use host = 
            Host
                .CreateDefaultBuilder(args)
                //.ConfigureServices(fun s -> s.AddSingleton<IHelloGrain, HelloGrain>() |> ignore)
                .UseOrleans(fun ctx (sb:ISiloBuilder) -> 
                    sb                    
                        .UseInMemoryReminderService()
                        .UseDashboard()
                        .UseLocalhostClustering()
                        .AddMemoryGrainStorage("PubSubStore")
                        //.ConfigureServices(fun s -> 
                                //s.AddSingleton<Orleans.Serialization.ITypeSerializer<HelloMessage>>(new HelloMessageSerializer()    ) |> ignore
                            //s.AddSerializer(fun builder ->  builder.AddType<HelloMessage>())
                            //s.AddTransient<IHelloGrain, HelloGrain>() |> ignore
                            //s.RegisterOrleankkaActors(typeof<IHelloGrain>.Assembly) |> ignore
                        //)
                        |> ignore
                )
                .UseOrleankka()
                .Build()

        host.StartAsync() |> Async.AwaitTask |> Async.RunSynchronously

        (*
        let lightbulb = actorSystem.ActorOf<IHelloGrain>("eco")
        let x = lightbulb.Ask<_>(Greeting "world")
        *)

        let grainFactory = host.Services.GetRequiredService<IGrainFactory>()
        let greeter = grainFactory.GetGrain<IHelloGrain>("xx")



        //let grainFactory = host.Services.GetService<IGrainFactory>()

        //let xxx = ActorSystem.actorOf<HelloGrain>(actorSystem, "xxx")

        //let friend = grainFactory.GetGrain<HelloGrain>("grain-1")
        let grain = grainFactory.GetGrain<IHelloGrain>("grain-1")

        let actorSystem = host.Services.GetService<IActorSystem>()
        //let shop = actorSystem.TypedActorOf<IHelloGrain>("amazon")

        //let greeter = host.ActorSystem().ActorOf<IHelloGrain>("id")
        task {
            //let! response = greeter.ReceiveTell("world")

            let! xxx = grain.ReceiveAsk("world")

            //let! response = shop.Tell(Greeting "Good morning!")
            //printfn "Response: %A" response
            printfn "Response: "

        } |> Async.AwaitTask |> Async.RunSynchronously

        (*
        let greeter = host.ActorSystem().ActorOf<IHelloGrain>("id")

        task {
            
            let! _ = greeter.Tell(Greeting "world")

            //let! response = shop.Tell(Greeting "Good morning!")
            //let! response = grain. <? Greeting "Good morning!"
            //printfn "\n\n%s\n\n" response
            let a = 1
            ()
        } |> Async.AwaitTask |> Async.RunSynchronously
        *)

        // Keep the host running until Ctrl+C is pressed
        host.WaitForShutdown()
    
        // Clean shutdown when the app is terminated
        host.StopAsync() |> Async.AwaitTask |> Async.RunSynchronously

        0
