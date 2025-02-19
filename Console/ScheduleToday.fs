module Console.ScheduleToday

open System
open Console.LocalState
open Console.ConsoleQueries
open Console.UserInteraction
open TodoistAdapter.CommunicationSyncApi
open TodoistAdapter.RestTypes
open TodoistAdapter.TaskDateUpdating

let private scheduleTimes =
    List.init 24 (fun i -> DateTime.Now.Hour + 1 + i * 2)
    |> List.filter (fun i -> i < 24)
    |> List.map (fun i -> $"{i}:00")

let private createTaskWithNewTime (ui: UserInteraction) (task: TodoistTask) =
    let updatedTime =
        ui.chooseFrom scheduleTimes $"What time should '{task.content}' be scheduled"
        |> TimeOnly.Parse

    let due =
        task.due
        |> Option.defaultValue emptyDue

    let dueDate =
        due.date
        |> Option.defaultValue (TodoistDateOnly (todaysDate ()))
        |> updateTime updatedTime

    { task with due = Some { due with date = Some dueDate } }

let private askForNewLabel ui tasks =
    let newLabel = ui.chooseFrom labels "Add new label to manipulated tasks"
    match newLabel with
    | "" -> tasks
    | label -> tasks |> List.map (fun task -> { task with labels = Some [label] }: TodoistTask)

let scheduleToday ui =
    async {
        let! chosenTasks = chooseTodayTasksGroupedByLabel ui
        let tasksWithNewTime = chosenTasks |> List.map (createTaskWithNewTime ui)
        let! response = askForNewLabel ui tasksWithNewTime |> updateTask
        return [response]
    }