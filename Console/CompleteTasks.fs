module Console.CompleteTasks

open TodoistAdapter.CommunicationRestApi
open Console.ConsoleQueries

let completeTasks ui =
    async {
        let! chosenTasks = chooseTodayTasksGroupedByLabel ui
        let! responses =
             chosenTasks
             |> List.map (fun t -> completeTask t.id)
             |> Async.Parallel
        return List.ofArray responses
    }