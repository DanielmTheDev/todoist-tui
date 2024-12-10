module Console.CollectUnderNewParent

open Console.ConsoleQueries
open Console.Communication
open Console.Types
open FsHttp
open SpectreCoff

let collectUnderNewParent () =
    let createdParent =
        ask<string> "Name of new parent task"
        |> fun content -> { emptyCreateTaskDto with content = content }
        |> createTask
        |> Async.RunSynchronously
        |> Response.deserializeJson<TodoistTask>
    let chosenTasks =
        chooseTodayTasksGroupedByLabel ()
        |> List.map (fun task -> task) // update_id not there on update. use sync api?
    failwith "test"
    0