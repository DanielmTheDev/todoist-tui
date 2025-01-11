module Console.ResetPriorities

open TodoistAdapter.Communication
open TodoistAdapter.Mapping

let resetTodayPriority () =
    async {
        let! tasks = getTodayTasks ()
        let! updateResults =
            tasks
            |> List.map toUpdateDto
            |> List.map (fun t -> { t with priority = Some 1 })
            |> List.map updateTask
            |> Async.Parallel
        return updateResults |> List.ofArray
    }
