module Console.LoadBalancing

open System
open TodoistAdapter.Types.Due
open TodoistAdapter.Types.State
open TodoistAdapter.Types.TodoistTask
open UserInteraction

type TasksAtDate = { date: DateOnly; numberOfTasks: int }

let tasksBetweenDates state startDate endDate =
    state.items
    |> List.filter (fun task ->
    let taskDate = dueDateOf task.due |> Option.defaultValue DateOnly.MinValue
    taskDate > startDate && taskDate <= endDate)

let groupTaskNumberByDate tasks =
    tasks
    |> List.groupBy (fun task -> dueDateOf task.due |> Option.defaultValue DateOnly.MinValue)
    |> List.map (fun (date, tasks) -> { date = date; numberOfTasks = tasks.Length })
    |> List.sortBy _.numberOfTasks

let daysAhead ui =
    ui.chooseFrom (List.init 10 (fun i -> $"{i + 1}")) "How many days to look ahead?" |> int