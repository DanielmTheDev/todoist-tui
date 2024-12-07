module Console.ResetPriorities

open Console.Communication
open Console.Mapping

let resetTodayPriority () =
    getTodayTasks ()
    |> List.map toUpdateDto
    |> List.map (fun t -> { t with priority = Some 1 })
    |> List.map updateTask
