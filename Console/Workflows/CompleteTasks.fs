module Console.CompleteTasks

open Console.TaskQueries.Today
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Types.State

let completeTasks (state: State) ui =
    async {
        let! responses =
             ui |> chooseTodayTasksGroupedByLabel state
             |> List.map (fun t -> completeTask t.id)
             |> Async.Parallel
        return List.ofArray responses
    }