[<RequireQualifiedAccess>]
module TodoistAdapter.Tests.TestData.TestTasks

open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Dtos.CreateTaskDto
open TodoistAdapter.Initialization
open TodoistAdapter.SyncApi
open TodoistAdapter.Types.TodoistTask

do initializeAll () |> Async.RunSynchronously

let create dueString =
        { defaultCreateTask with content = "test task"; due_string = Some dueString; priority = Some 4 }
        |> createTask
        |> Async.RunSynchronously
        |> Response.assert2xx
        |> Response.deserializeJson<Task>

let createToday () =
        create "today"

let deleteAllExisting () =
    async {
        let! allItems = fullSync ()
        return! allItems.items |> deleteTasks
    }
    |> Async.RunSynchronously
    |> Response.assert2xx
    |> ignore

