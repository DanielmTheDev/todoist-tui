open Console.Choice
open Console.CollectUnderNewParent
open Console.TaskQueries.Add
open Console.UserInteraction
open Console.CompleteTasks
open Console.PostponeToday
open Console.ResetPriorities
open Console.ScheduleToday
open SpectreCoff
open TodoistAdapter.Initialization
open TodoistAdapter.LocalState

initialize ()

let rec mainLoop (ui: UserInteraction) =
    let choiceStrings = List.map choiceToString choices
    let chosen = chooseFrom choiceStrings "What do you want to do?"

    let choice = choices |> List.find (fun c -> choiceToString c = chosen)

    let execute action = ui |> action (refreshedState ())
    match choice with
    | AddTask -> execute addTask
    | CompleteTasks -> execute completeTasks
    | ScheduleToday -> execute scheduleToday
    | CollectUnderNewParent -> execute collectUnderNewParent
    | PostponeToday -> execute postponeToday
    | ResetTodayPriority -> resetTodayPriority (refreshedState ())
    |> Async.RunSynchronously
    |> ignore
    refreshedState () |> ignore
    mainLoop ui

mainLoop spectreCoffUi
