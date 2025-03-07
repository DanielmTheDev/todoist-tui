module Console.PostponeToday

open System
open Console.Queries.chooseFutureTasks
open Console.Queries.chooseTodayTasksGroupedByLabel
open TodoistAdapter.SyncApi
open TodoistAdapter.Types.Due
open TodoistAdapter.Types.DueDate
open TodoistAdapter.Types.TodoistTask

type TasksAtDate = { date: DateOnly; numberOfTasks: int }

let private except (chosenTasks: Task list) : Task list -> Task list =
    List.filter (fun futureTask -> not (chosenTasks |> List.exists (fun chosenTask -> chosenTask.id = futureTask.id)))

let private updateDay date task =
    let updatedDue =
        match task.due with
        | Some due -> { due with date = Some (updateDay date due.date.Value) }
        | _ -> { defaultDue with date = Some (TodoistDateOnly date) }
    { task with due = Some updatedDue }

let private reorderLoadList leastLoadedDay loadList =
    { leastLoadedDay with numberOfTasks = leastLoadedDay.numberOfTasks + 1 } :: List.tail loadList
            |> List.sortBy _.numberOfTasks

let rec private distributeTasks (tasks: Task list) (loadList: TasksAtDate list) =
    match tasks with
    | [] -> []
    | task :: remainingTasks ->
        let leastLoadedDay = List.head loadList
        let updatedLoadList = loadList |> reorderLoadList leastLoadedDay
        let updatedTask = task |> updateDay leastLoadedDay.date
        updatedTask :: distributeTasks remainingTasks updatedLoadList

let postponeToday state ui =
    async {
        let chosenTasks = ui |> chooseTodayTasksGroupedByLabel state
        match chosenTasks with
        | [] -> return []
        | chosenTasks ->
            let! updateResults =
                chooseFutureTasks ui
                |> except chosenTasks
                |> List.groupBy _.due.Value.date
                |> List.map (fun (date, tasks) -> { date = (dateOnlyOf date.Value); numberOfTasks = tasks.Length })
                |> List.sortBy _.numberOfTasks
                |> distributeTasks chosenTasks
                |> updateTask
            return [ updateResults ]
    }
