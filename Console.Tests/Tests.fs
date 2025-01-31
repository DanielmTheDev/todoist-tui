namespace TodoistAdapter.Tests

open System
open Console.CollectUnderNewParent
open Console.PostponeToday
open Console.Tests
open Console.Tests.MockTasks
open FsHttp
open TodoistAdapter.CommunicationSyncApi
open TodoistAdapter.RestTypes
open Xunit
open FsUnit.Xunit

type ConsoleTests() =
    do init ()

    [<Fact>]
    member _.``Adds a task``() =
        let ui =
            MockInteractions.create ()
            |> MockInteractions.addAsk "Test Task"
            |> MockInteractions.addAskSuggesting "tod"
            |> MockInteractions.addChooseFrom "testLabel"
            |> MockInteractions.build

        let createdTask = createAndGetTask ui

        createdTask.content |> should equal "Test Task"
        createdTask.due.Value.date.Value.Date |> should equal DateTime.Now.Date
        createdTask.labels |> should equal (Some [| "testLabel" |])

    [<Fact>]
    member _.``Collect under new parent adds parent task and removes due from children``() =
        let created1 = createTodayAndGet ()
        let created2 = createTodayAndGet ()
        let parentContent = Guid.NewGuid().ToString()

        let collectUi =
            MockInteractions.create ()
            |> MockInteractions.addAsk parentContent
            |> MockInteractions.addChooseGroupedFrom [ created1; created2 ]
            |> MockInteractions.build

        collectUnderNewParent collectUi |> Async.RunSynchronously |> ignore

        let allItems = (fullSync () |> Async.RunSynchronously).items
        let parent = allItems |> List.find (fun item -> item.content = parentContent)

        [ created1.id; created2.id ]
        |> List.iter (fun id ->
            let item = allItems |> List.find (fun item -> item.id = id)
            item.parent_id |> should equal (Some parent.id)
            item.due |> should equal None)

    [<Fact>]
    member _.``Postpones tasks``() =
        deleteAllExistingTasks ()
        |> Async.RunSynchronously
        |> Response.assert2xx
        |> ignore
        let todayTasks = [createTodayAndGet (); createTodayAndGet ()]
        let tomorrowTask = createWithDueStringAndGet "tom"
        let dayAfterTomorrowTask = createWithDueStringAndGet "in 2 days"
        let ui =
            MockInteractions.create ()
            |> MockInteractions.addChooseGroupedFrom todayTasks
            |> MockInteractions.addChooseFrom "2"
            |> MockInteractions.build

        postponeToday ui |> Async.RunSynchronously |> ignore

        let allItems = (fullSync () |> Async.RunSynchronously).items
        allItems
        |> List.filter (fun item -> item.due.Value.date.Value = (DateTime.Today.AddDays 1))
        |> List.map _.id
        |> should equal [todayTasks.Head.id; todayTasks.Tail.Head.id; tomorrowTask.id]

        allItems
        |> List.filter (fun item -> item.due.Value.date.Value = (DateTime.Today.AddDays 2))
        |> List.map _.id
        |> should equal [dayAfterTomorrowTask.id]