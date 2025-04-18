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
            let parentContent = ui.ask "Name of new parent task"
            let! response =
                { defaultCreateTask with content = parentContent; due_string = Some "today"  }
                |> createTask
                |> ui.spinner "Creating parent task"

            let createdParent =
                response
                |> Response.assert2xx
                |> Response.deserializeJson<Task>

            let! response =
                moveBelowParent createdParent.id (chosenTasks |> List.map _.id)
                |> ui.spinner "Moving children"

            match response with
            | response when response.statusCode = HttpStatusCode.OK  ->
                let! updateResults =
                    chosenTasks
                    |> List.map (fun t -> { t with due = None })
                    |> updateTask
                    |> ui.spinner "Removing due time"
                return [updateResults]
            | _ ->
                return failwith "Something went wrong"
    }