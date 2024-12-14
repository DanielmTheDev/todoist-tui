module Console.ScheduleToday

open Console.Communication
open Console.LocalState
open Console.ConsoleQueries
open Console.Time
open Console.Types
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
    (chooseTodayTasksGroupedByLabel ())
    |> List.map createTaskWithNewTime
    |> askForNewLabel
    |> List.map updateTask