module Console.Interactive

open Console.AddTaskWithLoadBalancing
open Console.Choice
open Console.CollectUnderNewParent
open Console.CompleteTasks
open Console.PostponeToday
open Console.ResetPriorities
open Console.ScheduleToday
open Console.TaskQueries.Add
open Console.UserInteraction
open Console.Workflows.CompleteButRemindLater
open SpectreCoff
open TodoistAdapter.LocalState

let rec runInteractively (ui: UserInteraction) =
    let timer = State.refreshPeriodically ()


    let rec loop () =
        let choiceStrings = List.map choiceToString choices
        let chosen = chooseFrom choiceStrings "What do you want to do?"

        let choice = choices |> List.find (fun c -> choiceToString c = chosen)

        let execute action =
            ui
            |> action (Status.start "Synchronizing" (fun _ -> refreshedState ()) |> Async.RunSynchronously)
        match choice with
        | AddTask -> execute addTask
        | AddTaskWithLoadBalancing -> execute addTaskWithLoadBalancing
        | CompleteButAddReminder -> execute completeButRemindLater
        | CompleteTasks -> execute completeTasks
        | ScheduleToday -> execute scheduleToday
        | CollectUnderNewParent -> execute collectUnderNewParent
        | PostponeToday -> execute postponeToday
        | ResetTodayPriority -> execute resetTodayPriority
        |> Async.RunSynchronously
        |> ignore
        refreshedState () |> ignore
        loop ()
    try
        loop ()
    finally
        timer.Dispose()