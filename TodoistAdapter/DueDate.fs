module TodoistAdapter.TaskDateUpdating

open System
open TodoistAdapter.RestTypes

let todaysDate () =
    DateOnly.FromDateTime DateTime.Today

let dateOnlyOf date =
    match date with
    | TodoistDateOnly date -> date
    | TodoistDateTime dateTime -> DateOnly.FromDateTime dateTime

let dueDateOf task =
    task.due
    |> Option.bind _.date
    |> Option.map dateOnlyOf

let updateDay (newDate: DateOnly) (oldDueDate: DueDate) =
    match oldDueDate with
    | TodoistDateOnly _ -> DueDate.TodoistDateOnly newDate
    | TodoistDateTime oldTime ->
        let updatedDateTime = DateTime(newDate.Year, newDate.Month, newDate.Day, oldTime.Hour, oldTime.Minute, oldTime.Second)
        TodoistDateTime updatedDateTime

let updateTime newTime oldDueDate =
    match oldDueDate with
    | TodoistDateOnly dateOnly -> TodoistDateTime (dateOnly.ToDateTime newTime)
    | TodoistDateTime dateTime -> TodoistDateTime (DateTime ((DateOnly.FromDateTime dateTime), newTime))