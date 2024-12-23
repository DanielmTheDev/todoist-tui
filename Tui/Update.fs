module Tui.Update

open Terminal.Gui
open Terminal.Gui.Elmish
open TodoistAdapter.Types
open Tui.Types

let update msg model =
    match msg with
    | SelectItem "Quit" ->
        Application.RequestStop()
        model, Cmd.none
    | SelectItem item ->
        { model with SelectedItem = $"You selected: {item}" }, Cmd.none
    | ExitApp ->
        Application.RequestStop()
        model, Cmd.none
    | SetAddTaskContent content -> { model with Model.AddTask.Content = content }, Cmd.none
    | SetAddTaskDue due -> { model with Model.AddTask.Due = due }, Cmd.none
    | AddTask ->
        { model with Model.AddTask.Due = "today"; Model.AddTask.Content = "" },
        Cmd.OfAsync.attempt
            TodoistAdapter.Communication.createTask { emptyCreateTaskDto with content = model.AddTask.Content; due_string = Some model.AddTask.Due; priority = Some 4 }
            (fun response -> TaskCreated)
    | TaskCreated -> model, Cmd.none