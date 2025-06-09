namespace TodoistConsole

open System
open Console.AddTaskWithLoadBalancing
open Console.CollectUnderNewParent
open Console.Tests
open Console.Workflows.CompleteButRemindLater
open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Initialization
open TodoistAdapter.LocalState
open TodoistAdapter.SyncApi
open TodoistAdapter.Types.Due
open TodoistAdapter.Types.DueDate
open TodoistAdapter.Types.State
open Xunit
open FsUnit.Xunit

type Tests() =

    do initializeAll () |> Async.RunSynchronously

    [<Fact>]
    member _.``Adds a task``() =
        let createdTask = Workflows.createFull "Test Task" "tod" "testLabel"

        createdTask.content |> should equal "Test Task"
        (dueDateOf createdTask.due).Value |> should equal (todaysDate ())
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

        collectUi
        |> collectUnderNewParent (refreshedState () |> Async.RunSynchronously)
        |> Async.RunSynchronously |> ignore

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
        let link = "[Test Link](https://www.google.com)"
        let todayTask1 = Workflows.createToday ()
        let todayTask2 = Workflows.createFull link "every day" ""
        let tomorrowTask = Workflows.createWithDueString "tom"
        let dayAfterTomorrowTask = Workflows.createWithDueString "in 2 days"
        let state = refreshedState () |> Async.RunSynchronously
        let todayTasks = state |> itemsWithIds [todayTask1.id; todayTask2.id]

        Workflows.postpone todayTasks "2" state
        |> List.iter (fun r -> r |> Response.assert2xx |> ignore)

        let allItems = (fullSync () |> Async.RunSynchronously).items
        allItems
        |> List.filter (fun item -> (dateOnlyOf item.due.Value.date.Value) = (DateOnly.FromDateTime (DateTime.Today.AddDays 1)))
        |> List.map _.id
        |> should equal [todayTask1.id; tomorrowTask.id]

        (allItems |> List.find (fun item -> item.id = todayTask2.id)).due.Value.is_recurring |> should equal true
        todayTasks.Tail.Head.due.Value.is_recurring |> should equal true

        allItems
        |> List.filter (fun item -> (dateOnlyOf item.due.Value.date.Value) = (DateOnly.FromDateTime (DateTime.Today.AddDays 2)))
        |> List.map _.id
        |> should equal [todayTask2.id; dayAfterTomorrowTask.id]

    [<Fact>]
    member _.``When scheduling a recurring task with link it stays recurring and adds label``() =
        let link = "[Test Link](https://www.google.com)"
        let task = Workflows.createFull link "every 5 days" ""
        let state = refreshedState () |> Async.RunSynchronously
        let task = state |> itemWithId task.id

        Workflows.reschedule "23:30" "testLabel" [task] (refreshedState () |> Async.RunSynchronously) |> ignore

        let postponedTask = getTask task.id |> Async.RunSynchronously
        postponedTask.due.Value.is_recurring |> should equal true
        let expectedDateTime = DateTime ((DateOnly.FromDateTime DateTime.Now), (TimeOnly (23, 30)))
        postponedTask.due.Value.datetime.Value |> should equal expectedDateTime
        postponedTask.due.Value.string |> should equal "every 5 days"
        postponedTask.labels.Value.Head |> should equal "testLabel"
        postponedTask.content |> should equal link

    [<Fact>]
    member _.``Adds task to day with least load``() =
        Workflows.deleteAllExistingTasks ()
        Workflows.createWithDueString "tom" |> ignore
        Workflows.createWithDueString "in 2 days" |> ignore
        Workflows.createWithDueString "in 2 days" |> ignore
        Workflows.createWithDueString "in 3 days" |> ignore
        Workflows.createWithDueString "in 3 days" |> ignore
        let content = (Guid.NewGuid ()).ToString()
        let state = refreshedState () |> Async.RunSynchronously

        let ui =
            MockInteractions.create ()
            |> MockInteractions.addAsk content
            |> MockInteractions.addChooseFrom "3"
            |> MockInteractions.build

        let result = addTaskWithLoadBalancing state ui |> Async.RunSynchronously
        result |> List.iter (fun r -> r |> Response.assert2xx |> ignore)

        let allItems = (fullSync () |> Async.RunSynchronously).items
        let newTask = allItems |> List.find (fun item -> item.content = content)

        let expectedDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
        (dateOnlyOf newTask.due.Value.date.Value) |> should equal expectedDate

    [<Fact>]
    member _.``CompleteButRemindLater completes the task and creates a reminder for today``() =
        let content = (Guid.NewGuid ()).ToString()
        let originalTask = Workflows.createFull content "every 2 day" "testLabel"
        let ui =
            MockInteractions.create ()
            |> MockInteractions.addChooseGroupedFrom [originalTask]
            |> MockInteractions.addChooseFrom "testLabel"
            |> MockInteractions.addChooseFrom "23:00"
            |> MockInteractions.build
        let state = refreshedState () |> Async.RunSynchronously

        completeButRemindLater state ui |> Async.RunSynchronously |> ignore

        let itemPair =
            (fullSync ()
             |> Async.RunSynchronously).items
             |> List.filter (fun item -> item.content = content)

        let movedOriginalTask = itemPair |> List.find (fun item -> item.id = originalTask.id)
        movedOriginalTask.due.Value.date.Value |> should equal (fromDateTime (DateTime.Today.AddDays 2))
        let newlyCreatedReminder = itemPair |> List.find (fun item -> item.id <> originalTask.id)
        newlyCreatedReminder.due.Value.date.Value |> should equal (TodoistDateTime (DateTime.Today.AddHours(23)))