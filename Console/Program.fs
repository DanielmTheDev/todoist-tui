open Console
open Console.CollectUnderNewParent
open Console.CompleteTasks
open Console.PostponeToday
open Console.ResetPriorities
open Console.ScheduleToday
open SpectreCoff
open ConsoleQueries

type TaskChoice =
    | AddTask
    | CompleteTasks
    | ScheduleToday
    | CollectUnderNewParent
    | PostponeToday
    | ResetTodayPriority

let choiceToString = function
    | AddTask -> "Add Task"
    | CompleteTasks -> "Complete Tasks"
    | ScheduleToday -> "Schedule Today"
    | CollectUnderNewParent -> "Collect under new parent task"
    | PostponeToday -> "Postpone Today"
    | ResetTodayPriority -> "Reset Today's priority"

let choices = [AddTask; CompleteTasks; ScheduleToday; CollectUnderNewParent; PostponeToday; ResetTodayPriority]

LocalState.init ()

while true do
    let choiceStrings = List.map choiceToString choices
    let chosen = chooseFrom choiceStrings "What do you want to do?"

    let choice =
        choices
        |> List.find (fun c -> choiceToString c = chosen)

    match choice with
    | AddTask -> addTask ()
    | CompleteTasks -> completeTasks ()
    | ScheduleToday -> scheduleToday ()
    | CollectUnderNewParent -> collectUnderNewParent ()
    | PostponeToday -> postponeToday ()
    | ResetTodayPriority -> resetTodayPriority ()
    |> Async.RunSynchronously
    |> ignore