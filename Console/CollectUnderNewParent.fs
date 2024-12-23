module Console.CollectUnderNewParent

open System.Net
open Console.ConsoleQueries
open TodoistAdapter.Communication
open TodoistAdapter.Types
open FsHttp
open SpectreCoff

let collectUnderNewParent () =

    match chooseTodayTasksGroupedByLabel () with
    | [] -> []
    | chosenTasks ->
        let createdParent =
            ask<string> "Name of new parent task"
            |> fun content -> { emptyCreateTaskDto with content = content; due_string = Some "today"  }
            |> createTask
            |> Async.RunSynchronously
            |> Response.deserializeJson<TodoistTask>

        let response =
            moveBelowParent createdParent.id (chosenTasks |> List.map _.id)
            |> Async.RunSynchronously

        match response with
        | response when response.statusCode = HttpStatusCode.OK  ->
            chosenTasks
            |> List.map (fun t -> { t with due_date = None; due_string = Some "no date"; due_datetime = None })
            |> List.map updateTask
        | _ ->
            failwith "Something went wrong"