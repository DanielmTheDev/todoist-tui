module Tui.Program

open Terminal.Gui
open Terminal.Gui.Elmish
open TodoistAdapter
open Tui.Types
open Tui.Update
open Tui.View

let init () =
    { MenuItems = [ "Item 1"; "Item 2"; "Quit" ]
      SelectedItem = "Welcome to the app!"
      AddTask = { Content = ""; Due = "today"; Label = "" }
      Tasks = []
      Projects = []
      Labels = [] },
    Cmd.OfAsync.perform Communication.fullSync () FullSyncCompleted

[<EntryPoint>]
let main _ =
    Communication.init ()
    let l =ConfigurationManager.Locations
    let s =ConfigurationManager.RuntimeConfig
    Program.mkProgram init update view
    |> Program.run
    0
