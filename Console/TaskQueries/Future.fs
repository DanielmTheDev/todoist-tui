module Console.TaskQueries.Future

open System
open Console.UserInteraction
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Types.Due

let chooseFutureTasks (ui: UserInteraction) =
    ui.chooseFrom (List.init 10 (fun i -> $"{i}")) "how many days?"
    |> int
    |> fun i -> i + 1
    |> getAheadTasks
    |> List.filter (fun task -> (dueDateOf task.due) |> Option.defaultValue DateOnly.MinValue  > todaysDate ())