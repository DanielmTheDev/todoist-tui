module Console.ResetPriorities

open Console.UserInteraction
open TodoistAdapter.SyncApi
open TodoistAdapter.Types.State
open TodoistAdapter.Types.TodoistTask

let resetTodayPriority (state: State) ui =
    async {
        let tasks = state |> todaysItems
        let! updateResults =
            tasks
            |> List.map (fun t -> { t with priority = Some 1 })
            |> updateTask
            |> ui.spinner "Resetting priorities"
        return [updateResults]
    }
