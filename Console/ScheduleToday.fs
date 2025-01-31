module Console.ScheduleToday

open Console.LocalState
open Console.ConsoleQueries
open Console.Time
open TodoistAdapter.CommunicationSyncApi
open TodoistAdapter.RestTypes
open SpectreCoff

let private createTaskWithNewTime (task: TodoistTask) =
    let updatedTime = chooseFrom scheduleTimes $"What time should '{task.content}' be scheduled"
    { task with due = Some { task.due.Value with string = updatedTime} }

let private askForNewLabel tasks =
    let newLabel = chooseFrom labels "Add new label to manipulated tasks"
    match newLabel with
    | "" -> tasks
    | label -> tasks |> List.map (fun task -> { task with labels = Some [|label|] }: TodoistTask)

let scheduleToday ui =
    async {
        let! chosenTasks = chooseTodayTasksGroupedByLabel ui
        let tasksWithNewTime = chosenTasks |> List.map createTaskWithNewTime
        let! response = askForNewLabel tasksWithNewTime |> updateTask
        return [response]
    }