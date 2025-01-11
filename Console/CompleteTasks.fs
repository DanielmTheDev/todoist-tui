module Console.CompleteTasks

open TodoistAdapter.Communication
open Console.ConsoleQueries

let completeTasks () =
    async {
        let! chosenTasks = chooseTodayTasksGroupedByLabel ()
        let! responses =
             chosenTasks
             |> List.map (fun t -> completeTask t.id)
             |> Async.Parallel
        return List.ofArray responses
    }