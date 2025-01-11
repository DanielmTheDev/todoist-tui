module Tui.Update

open Terminal.Gui
open Terminal.Gui.Elmish
open TodoistAdapter.Types
open Tui.Mapping
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
    | SetAddTaskLabel label -> { model with Model.AddTask.Label = label }, Cmd.none
    | AddTask ->
        { model with Model.AddTask.Due = "today"; Model.AddTask.Content = "" },
        Cmd.OfAsync.attempt
            TodoistAdapter.Communication.createTask { emptyCreateTaskDto with content = model.AddTask.Content; labels = Some [|model.AddTask.Label|]; due_string = Some model.AddTask.Due; priority = Some 4 }
            (fun _ -> TaskCreated)
    | TaskCreated -> model, Cmd.none
    | FullSyncCompleted syncResponse ->
        let projects = syncResponse.projects |> List.map (fun project -> { project with name = project.name |> toScalarValueString })
        let labels = syncResponse.labels |> List.map (fun label -> { label with name = label.name |> toScalarValueString })
        let tasks =
            syncResponse.items
            |> List.map (fun task ->
                { task with
                    content = task.content |> toScalarValueString
                    labels =
                    (task.labels |> Option.defaultValue [||])
                        |> Array.map toScalarValueString
                        |> Some })
        { model with Tasks = tasks; Projects = projects; Labels = labels }, Cmd.none
