namespace SyncApi

open System
open FsHttp
open TodoistAdapter.Tests.TestData
open TodoistAdapter.Types.DueDate
open TodoistAdapter.Types.TodoistTask
open Xunit
open FsUnit.Xunit
open TodoistAdapter.SyncApi

type ApiTests() =
    [<Fact>]
    member _.``Updating a tasks time creates a reminder`` () =
        let createdTask = TestTasks.create "today"

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

    // [<Fact>]
    // member _.``Get filters`` () =
    //     let sync =
    //         sync ["filters"] "*"
    //         |> Async.RunSynchronously
    //     let reminders = sync.reminders |> List.filter (fun r -> r.item_id = createdTask.id)
    //     reminders |> should not' (be Empty)
    //     reminders.Head.``type`` |> should equal "relative"
    //     reminders.Head.minute_offset |> should equal 0