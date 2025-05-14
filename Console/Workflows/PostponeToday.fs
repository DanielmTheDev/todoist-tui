module Console.PostponeToday

open Console.TaskQueries.Today
open Console.LoadBalancing
open TodoistAdapter.SyncApi
open TodoistAdapter.Types.Due
open TodoistAdapter.Types.DueDate
open TodoistAdapter.Types.TodoistTask

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
        let daysAhead = ui |> daysAhead
        let today = todaysDate ()
        match chosenTasks with
        | [] -> return []
        | chosenTasks ->
            let! updateResults =
                tasksBetweenDates state today (today.AddDays(daysAhead))
                |> except chosenTasks
                |> groupTaskNumberByDate
                |> distributeTasks chosenTasks
                |> updateTask
                |> ui.spinner "Postponing tasks"
            return [ updateResults ]
    }
