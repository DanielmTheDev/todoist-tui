open Console.CommandLine
open Console.Interactive
open Console.UserInteraction
open TodoistAdapter.Initialization
open Argu

let parser = ArgumentParser.Create<Arguments>(programName = "todoist-tui")

initialize ()

let results = parser.ParseCommandLine ()
if results.GetAllResults().Length <> 0 then
    runWithCommandArgs spectreCoffUi results
else
    runInteractively spectreCoffUi
