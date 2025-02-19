namespace TodoistAdapter.Tests

open System
open Console.CollectUnderNewParent
open Console.Tests
open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.CommunicationSyncApi
open TodoistAdapter.RestTypes
open TodoistAdapter.TaskDateUpdating
open Xunit
open FsUnit.Xunit

type ConsoleTests() =
    do init ()

    [<Fact>]
    member _.``Adds a task``() =
        let createdTask = Workflows.createFull "Test Task" "tod" "testLabel"

        createdTask.content |> should equal "Test Task"
        (dueDateOf createdTask).Value |> should equal (todaysDate ())
        createdTask.labels |> should equal (Some [ "testLabel" ])

    [<Fact>]
    member _.``Collect under new parent adds parent task and removes due from children``() =
        let created1 = Workflows.createToday ()
        let created2 = Workflows.createToday ()
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
    member _.``Postpones tasks with links and retains recurring``() =
        Workflows.deleteAllExistingTasks ()
        |> Async.RunSynchronously
        |> Response.assert2xx
        |> ignore
        let link = "[Test Link](https://www.google.com)"
        let todayTask1 = Workflows.createToday ()
        let todayTask2 = Workflows.createFull link "every day" ""
        let todayTasks = [todayTask1; todayTask2]
        let tomorrowTask = Workflows.createWithDueString "tom"
        let dayAfterTomorrowTask = Workflows.createWithDueString "in 2 days"

        Workflows.postpone todayTasks "2" |> ignore

        let allItems = (fullSync () |> Async.RunSynchronously).items
        allItems
        |> List.filter (fun item -> (dateOnlyOf item.due.Value.date.Value) = (DateOnly.FromDateTime (DateTime.Today.AddDays 1)))
        |> List.map _.id
        |> should equal [todayTask1.id; todayTask2.id; tomorrowTask.id]

        (allItems |> List.find (fun item -> item.id = todayTask2.id)).due.Value.is_recurring |> should equal true
        todayTasks.Tail.Head.due.Value.is_recurring |> should equal true

        allItems
        |> List.filter (fun item -> (dateOnlyOf item.due.Value.date.Value) = (DateOnly.FromDateTime (DateTime.Today.AddDays 2)))
        |> List.map _.id
        |> should equal [dayAfterTomorrowTask.id]

    [<Fact>]
    member _.``When scheduling a recurring task with link it stays recurring and adds label``() =
        let link = "[Test Link](https://www.google.com)"
        let recurringTask = Workflows.createFull link "every 5 days" ""

        Workflows.reschedule "23:30" "testLabel" [recurringTask] |> ignore

        let postponedTask = getTask recurringTask.id |> Async.RunSynchronously
        postponedTask.due.Value.is_recurring |> should equal true
        let expectedDateTime = DateTime ((DateOnly.FromDateTime DateTime.Now), (TimeOnly (23, 30)))
        postponedTask.due.Value.datetime.Value |> should equal expectedDateTime
        postponedTask.due.Value.string |> should equal "every 5 days"
        postponedTask.labels.Value.Head |> should equal "testLabel"
        postponedTask.content |> should equal link