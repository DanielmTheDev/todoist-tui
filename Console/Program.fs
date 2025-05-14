open Console.CommandLine
open Console.Interactive
open Console.UserInteraction
open TodoistAdapter.Initialization
open Argu

let parser = ArgumentParser.Create<Arguments>(programName = "todoist-tui")

let results = parser.ParseCommandLine ()
if results.GetAllResults().Length <> 0 then
    initializeCommunication ()
    runWithCommandArgs spectreCoffUi results |> Async.RunSynchronously |> ignore
else
    SpectreCoff.Status.start "Synchronizing" (fun _ -> initializeAll ()) |> Async.RunSynchronously
    runInteractively spectreCoffUi |> Async.RunSynchronously |> ignore
