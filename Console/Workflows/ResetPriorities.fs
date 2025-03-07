module Console.ResetPriorities

open TodoistAdapter.SyncApi
open TodoistAdapter.Types.State
open TodoistAdapter.Types.TodoistTask

let resetTodayPriority (state: State) =
    async {
        let tasks = state |> todaysItems
        let! updateResults =
            tasks
            |> List.map (fun t -> { t with priority = Some 1 })
            |> updateTask
        return [updateResults]
    }
