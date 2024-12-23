module Tui.Program

open Terminal.Gui.Elmish
open TodoistAdapter
open Tui.Types
open Tui.Update
open Tui.View

let init () =
    { MenuItems = [ "Item 1"; "Item 2"; "Quit" ]
      SelectedItem = "Welcome to the app!"
      AddTask = { Content = ""; Due = "today" } }, Cmd.none

[<EntryPoint>]
let main _ =
    Communication.init ()
    Program.mkProgram init update view
    |> Program.run
    0
