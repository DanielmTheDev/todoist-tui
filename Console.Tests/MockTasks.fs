module Console.Tests.MockTasks

open Console.ConsoleQueries
open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.CommunicationSyncApi
open TodoistAdapter.RestTypes

let createTask ui =
    (addTask ui
     |> Async.RunSynchronously
     |> List.map (fun response -> response |> Response.assert2xx)
     |> List.map Response.deserializeJson<TodoistTask>)
        .Head

let createAndGetTask ui =
    let createdTask = createTask ui
    getTask createdTask.id |> Async.RunSynchronously

let createWithDueStringAndGet dueString =
    let ui =
        MockInteractions.create ()
        |> MockInteractions.addAsk "Test Task"
        |> MockInteractions.addAskSuggesting dueString
        |> MockInteractions.addChooseFrom ""
        |> MockInteractions.build

    let createdTask = createTask ui
    getTask createdTask.id |> Async.RunSynchronously

let createTodayAndGet () = createWithDueStringAndGet "tod"

let deleteAllExistingTasks () =
    async {
        let! allItems = fullSync ()
        return! allItems.items |> deleteTasks
    }
