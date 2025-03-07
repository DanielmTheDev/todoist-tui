module Console.ScheduleToday

open System
open Console.Queries.chooseTodayTasksGroupedByLabel
open Console.UserInteraction
open TodoistAdapter.SyncApi
open TodoistAdapter.Types.Due
open TodoistAdapter.Types.DueDate
open TodoistAdapter.Types.State
open TodoistAdapter.Types.TodoistTask

let private scheduleTimes =
    List.init 24 (fun i -> DateTime.Now.Hour + 1 + i * 2)
    |> List.filter (fun i -> i < 24)
    |> List.map (fun i -> $"{i}:00")

let private createTaskWithNewTime (ui: UserInteraction) (task: Task) =
    let updatedTime =
        ui.chooseFrom scheduleTimes $"What time should '{task.content}' be scheduled"
        |> TimeOnly.Parse

    let due =
        task.due
        |> Option.defaultValue defaultDue

    let dueDate =
        due.date
        |> Option.defaultValue (TodoistDateOnly (todaysDate ()))
        |> updateTime updatedTime

    { task with due = Some { due with date = Some dueDate } }

let private askForNewLabel ui tasks (state: State) =
    let newLabel = ui.chooseFrom (state |> labelNames) "Add new label to manipulated tasks"
    match newLabel with
    | "" -> tasks
    | label -> tasks |> List.map (fun task -> { task with labels = Some [label] }: Task)

let scheduleToday state ui =
    async {
        let tasksWithNewTime =
            ui
            |> chooseTodayTasksGroupedByLabel state
            |> List.map (createTaskWithNewTime ui)
        let! response = askForNewLabel ui tasksWithNewTime state |> updateTask
        return [response]
    }