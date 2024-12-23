module Console.ResetPriorities

open TodoistAdapter.Communication
open TodoistAdapter.Mapping

let resetTodayPriority () =
    getTodayTasks ()
    |> List.map toUpdateDto
    |> List.map (fun t -> { t with priority = Some 1 })
    |> List.map updateTask
