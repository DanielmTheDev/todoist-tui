module Console.CompleteTasks

open TodoistAdapter.Communication
open Console.ConsoleQueries

let completeTasks () =
    (chooseTodayTasksGroupedByLabel ())
    |> List.map (fun t -> completeTask t.id)