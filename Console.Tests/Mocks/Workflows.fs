[<RequireQualifiedAccess>]
module Console.Tests.Workflows

open Console.PostponeToday
open Console.ScheduleToday
open Console.TaskQueries.Add
open Console.AddTaskWithLoadBalancing
open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.SyncApi
open TodoistAdapter.Types.State
open TodoistAdapter.Types.TodoistTask

let private create ui =
    let createdTask = ( ui
        |> addTask defaultState
        |> Async.RunSynchronously
        |> List.map (fun response -> response |> Response.assert2xx)
        |> List.map Response.deserializeJson<Task>).Head
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

let reschedule time label tasks state =
    MockInteractions.create ()
    |> MockInteractions.addChooseGroupedFromWith tasks
    |> MockInteractions.addChooseFrom time
    |> MockInteractions.addChooseFrom label
    |> MockInteractions.build
    |> scheduleToday state
    |> Async.RunSynchronously
    |> List.map (fun r -> r |> Response.assert2xx)

let postpone tasks days state =
    MockInteractions.create ()
    |> MockInteractions.addChooseGroupedFrom tasks
    |> MockInteractions.addChooseFrom days
    |> MockInteractions.build
    |> postponeToday state
    |> Async.RunSynchronously

let deleteAllExistingTasks () =
    async {
        let! allItems = fullSync ()
        return! allItems.items |> deleteTasks
    }
    |> Async.RunSynchronously
    |> Response.assert2xx
    |> ignore
