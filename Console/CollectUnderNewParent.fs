module Console.CollectUnderNewParent

open System.Net
open Console.ConsoleQueries
open Console.UserInteraction
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.CommunicationSyncApi
open TodoistAdapter.RestTypes
open FsHttp

let collectUnderNewParent ui  =
    async {
        let! chosenTasks = chooseTodayTasksGroupedByLabel ui
        match chosenTasks with
        | [] -> return []
        | chosenTasks ->
            let! createdParent =
                ui.ask "Name of new parent task"
                |> fun content -> { emptyCreateTaskDto with content = content; due_string = Some "today"  }
                |> createTask
                |> Async.RunSynchronously
                |> Response.deserializeJson<TodoistTask>
                |> async.Return

            let! response = moveBelowParent createdParent.id (chosenTasks |> List.map _.id)

            match response with
            | response when response.statusCode = HttpStatusCode.OK  ->
                let! updateResults =
                    chosenTasks
                    |> List.map (fun t -> { t with due = None })
                    |> updateTask
                return [updateResults]
            | _ ->
                return failwith "Something went wrong"
    }