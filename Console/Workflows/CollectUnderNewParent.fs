module Console.CollectUnderNewParent

open System.Net
open Console.TaskQueries.Today
open Console.UserInteraction
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.SyncApi
open FsHttp
open TodoistAdapter.Dtos.CreateTaskDto
open TodoistAdapter.Types.TodoistTask

let collectUnderNewParent state ui =
    async {
        let chosenTasks = ui |> chooseTodayTasksGroupedByLabel state
        match chosenTasks with
        | [] -> return []
        | chosenTasks ->
            let! createdParent =
                ui.ask "Name of new parent task"
                |> fun content -> { defaultCreateTask with content = content; due_string = Some "today"  }
                |> createTask
                |> Async.RunSynchronously
                |> Response.deserializeJson<Task>
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