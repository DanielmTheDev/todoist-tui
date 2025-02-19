namespace TodoistAdapter.Tests

open System
open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.RestTypes
open TodoistAdapter.SyncTypes
open Xunit
open FsUnit.Xunit
open TodoistAdapter.CommunicationSyncApi

type CommunicationSyncApiTests() =

    let createTask dueString =
        { emptyCreateTaskDto with content = "test task"; due_string = Some dueString; priority = Some 4 }
        |> createTask
        |> Async.RunSynchronously
        |> Response.assert2xx
        |> Response.deserializeJson<TodoistTask>

    [<Fact>]
    member _.``Updating a tasks time creates a reminder`` () =
        init ()
        let createdTask = createTask "today"

        updateTask [{ createdTask with due = Some { createdTask.due.Value with date = Some (TodoistDateTime DateTime.Now)} }]
        |> Async.RunSynchronously
        |> Response.assert2xx
        |> ignore

        let sync =
            sync ["reminders"] "*"
            |> Async.RunSynchronously
        let reminders = sync.reminders |> List.filter (fun r -> r.item_id = createdTask.id)
        reminders |> should not' (be Empty)
        reminders.Head.``type`` |> should equal "relative"
        reminders.Head.minute_offset |> should equal 0
