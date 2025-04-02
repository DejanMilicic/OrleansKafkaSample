namespace OKSilo

open System
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.DependencyInjection
open Orleans
open Orleans.Hosting
open Orleans.Configuration
open OKGrains

///assembly attributes needed for Orleans to work in F#
module Load =

    [<assembly: Orleans.ApplicationPartAttribute("OKCodeGen")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Core.Abstractions")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Serialization")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Core")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Persistence.Memory")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Runtime")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Reminders")>]
    [<assembly: Orleans.ApplicationPartAttribute("OrleansDashboard.Core")>]
    [<assembly: Orleans.ApplicationPartAttribute("OrleansDashboard")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Clustering.Redis")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Persistence.Redis")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Streaming")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Serialization.Abstractions")>]
    [<assembly: Orleans.ApplicationPartAttribute("Orleans.Serialization")>]
    ()        

module Program =
    [<EntryPoint>]
    let main args =
        use host = 
            Host
                .CreateDefaultBuilder(args)
                .UseOrleans(fun ctx sb -> 
                    sb                    
                        .UseInMemoryReminderService()
                        .UseDashboard()
                        .UseLocalhostClustering()
                        .AddMemoryGrainStorage("PubSubStore")
                        |> ignore
                )
                .Build()

        host.StartAsync() |> Async.AwaitTask |> Async.RunSynchronously

        let grainFactory = host.Services.GetRequiredService<IGrainFactory>()

        task {
            let friend = grainFactory.GetGrain<IHelloGrain>("abc")
            let! response = friend.SayHello("Good morning!")
            printfn "\n\n%s\n\n" response
        } |> Async.AwaitTask |> ignore

        // Keep the host running until Ctrl+C is pressed
        host.WaitForShutdown()
    
        // Clean shutdown when the app is terminated
        host.StopAsync() |> Async.AwaitTask |> Async.RunSynchronously

        0
