module Console.ResetPriorities

open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.CommunicationSyncApi

let resetTodayPriority () =
    async {
        let! tasks = getTodayTasks ()
        let! updateResults =
            tasks
            |> List.map (fun t -> { t with priority = Some 1 })
            |> updateTask
        return [updateResults]
    }
