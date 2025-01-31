module Console.PostponeToday

open System
open Console.ConsoleQueries
open TodoistAdapter.CommunicationSyncApi
open TodoistAdapter.TaskDateUpdating
open TodoistAdapter.RestTypes

let private except (chosenTasks: TodoistTask list) : TodoistTask list -> TodoistTask list =
    List.filter (fun futureTask -> not (chosenTasks |> List.exists (fun chosenTask -> chosenTask.id = futureTask.id)))

let rec private distributeTasks (tasks: TodoistTask list) (loadList: (DateTime * int) list) =
    match tasks with
    | [] -> []
    | task :: remainingTasks ->
        let date, taskCount = List.head loadList
        let updatedLoadList =
            (date, taskCount + 1) :: List.tail loadList
            |> List.sortBy snd
        let updatedTask = task |> updateDueDatePreservingRecurring date
        updatedTask :: distributeTasks remainingTasks updatedLoadList

let postponeToday ui =
    async {
        let! chosenTasks = chooseTodayTasksGroupedByLabel ui
        match chosenTasks with
        | [] -> return []
        | chosenTasks ->
            let! updateResults =
                chooseFutureTasks ui
                |> except chosenTasks
                |> List.groupBy _.due.Value.date
                |> List.map (fun (date, tasks) -> (date.Value, tasks.Length))
                |> List.sortBy snd
                |> distributeTasks chosenTasks
                |> updateTask
            return [updateResults]
    }