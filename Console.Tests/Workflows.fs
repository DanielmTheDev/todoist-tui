[<RequireQualifiedAccess>]
module Console.Tests.Workflows

open Console.ConsoleQueries
open Console.PostponeToday
open Console.ScheduleToday
open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.CommunicationSyncApi
open TodoistAdapter.RestTypes

let private create ui =
    let createdTask = (addTask ui
        |> Async.RunSynchronously
        |> List.map (fun response -> response |> Response.assert2xx)
        |> List.map Response.deserializeJson<TodoistTask>).Head
    getTask createdTask.id |> Async.RunSynchronously

let createFull name dueString label =
    MockInteractions.create ()
    |> MockInteractions.addAsk name
    |> MockInteractions.addAskSuggesting dueString
    |> MockInteractions.addChooseFrom label
    |> MockInteractions.build
    |> create

let createWithDueString dueString =
    createFull "Test Task" dueString ""

let createToday () = createWithDueString "tod"

let deleteAllExistingTasks () =
    async {
        let! allItems = fullSync ()
        return! allItems.items |> deleteTasks
    }

let reschedule time label tasks =
    MockInteractions.create ()
    |> MockInteractions.addChooseGroupedFromWith tasks
    |> MockInteractions.addChooseFrom time
    |> MockInteractions.addChooseFrom label
    |> MockInteractions.build
    |> scheduleToday
    |> Async.RunSynchronously
    |> List.map (fun r -> r |> Response.assert2xx)

let postpone tasks days =
    MockInteractions.create ()
    |> MockInteractions.addChooseGroupedFrom tasks
    |> MockInteractions.addChooseFrom days
    |> MockInteractions.build
    |> postponeToday
    |> Async.RunSynchronously