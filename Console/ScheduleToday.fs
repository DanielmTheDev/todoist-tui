module Console.ScheduleToday

open TodoistAdapter.Communication
open Console.LocalState
open Console.ConsoleQueries
open Console.Time
open TodoistAdapter.Types
open SpectreCoff

let private createTaskWithNewTime task =
    let updatedTime = chooseFrom scheduleTimes $"What time should '{task.content.Value}' be scheduled"
    { task with due_string = Some updatedTime; due_datetime = None; due_date = None }

let private askForNewLabel tasks =
    let newLabel = chooseFrom labels "Add new label to manipulated tasks"
    match newLabel with
    | "" -> tasks
    | label -> tasks |> List.map (fun task -> { task with labels = Some [|label|] }: UpdateTaskDto)

let scheduleToday () =
    async {
        let! chosenTasks = chooseTodayTasksGroupedByLabel ()
        let tasksWithNewTime = chosenTasks |> List.map createTaskWithNewTime
        let tasksWithLabel = askForNewLabel tasksWithNewTime
        let! updateResults = tasksWithLabel |> List.map updateTask |> Async.Parallel
        return updateResults |> List.ofArray
    }