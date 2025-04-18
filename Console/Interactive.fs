module Console.Interactive

open System.Threading
open Console.AddTaskWithLoadBalancing
open Console.Choice
open Console.CollectUnderNewParent
open Console.CompleteTasks
open Console.PostponeToday
open Console.ResetPriorities
open Console.ScheduleToday
open Console.TaskQueries.Add
open Console.UserInteraction
open SpectreCoff
open TodoistAdapter.LocalState

let rec runInteractively (ui: UserInteraction) =
    let timer = new Timer((fun _ -> refreshedState () |> ignore), null, 30000, 30000)

    let rec loop () =
        let choiceStrings = List.map choiceToString choices
        let chosen = chooseFrom choiceStrings "What do you want to do?"

        let choice = choices |> List.find (fun c -> choiceToString c = chosen)

        let execute action = ui |> action (refreshedState ())
        match choice with
        | AddTask -> execute addTask
        | AddTaskWithLoadBalancing -> execute addTaskWithLoadBalancing
        | CompleteTasks -> execute completeTasks
        | ScheduleToday -> execute scheduleToday
        | CollectUnderNewParent -> execute collectUnderNewParent
        | PostponeToday -> execute postponeToday
        | ResetTodayPriority -> resetTodayPriority (refreshedState ())
        |> Async.RunSynchronously
        |> ignore
        refreshedState () |> ignore
        loop ()

    try
        loop ()
    finally
        timer.Dispose()