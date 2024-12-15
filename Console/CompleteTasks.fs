module Console.CompleteTasks

open Console.Communication
open Console.ConsoleQueries

let completeTasks () =
    (chooseTodayTasksGroupedByLabel ())
    |> List.map (fun t -> completeTask t.id)