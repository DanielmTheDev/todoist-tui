module Console.PostponeToday

open System
open Console.Communication
open Console.ConsoleQueries
open Console.TaskDateUpdating
open Console.Types

let private except (chosenTasks: UpdateTaskDto list) : TodoistTask list -> TodoistTask list =
    List.filter (fun futureTask -> not (chosenTasks |> List.exists (fun chosenTask -> chosenTask.id = futureTask.id)))

let rec private distributeTasks (tasks: UpdateTaskDto list) (loadList: (DateOnly * int) list) =
    match tasks with
    | [] -> []
    | task :: remainingTasks ->
        let date, taskCount = List.head loadList
        let updatedLoadList =
            (date, taskCount + 1) :: List.tail loadList
            |> List.sortBy snd
        let updatedTask = task |> updateDueDatePreservingRecurring date
        updatedTask :: distributeTasks remainingTasks updatedLoadList

let postponeToday () =
    let chosenTasks = chooseTodayTasksGroupedByLabel ()
    chooseFutureTasks ()
    |> except chosenTasks
    |> List.groupBy _.due.Value.date
    |> List.map (fun (date, tasks) -> (date, tasks.Length))
    |> List.sortBy snd
    |> distributeTasks chosenTasks
    |> List.iter updateTask
