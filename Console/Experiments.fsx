#r "bin/Debug/net8.0/Console.dll"
#r "nuget: EluciusFTW.SpectreCoff, 0.49.3"

open System
open System.Text.RegularExpressions
open Console.Communication
open Console.Types
open Console.Mapping

let inFiveDays =  DateOnly.FromDateTime(DateTime.Now.AddDays(5))

let appendStartDate (recurringPatten: string) (date: DateOnly) =
    let asString = date.ToString("dd/MM")
    @$"{recurringPatten} starting {asString}"

let extractRecurringPattern dueString =
    let pattern = @"\b(every\s+.*?)(?=\s+starting|\s+ending|$)"
    let result = Regex.Match(dueString, pattern, RegexOptions.IgnoreCase)
    if result.Success
        then result.Groups[1].Value.Trim()
    else ""

let addDatePreservingRecurring = extractRecurringPattern >> appendStartDate

let updateDueDatePreservingRecurring (date: DateOnly) task  =
    match task.due with
    | Some { is_recurring = true } ->
        let dueString = addDatePreservingRecurring task.due.Value.string date
        { (task |> toUpdateDto) with due_date = None; due_string = Some dueString }
    | _ ->{ (task |> toUpdateDto) with due_string = None; due_date = Some (date.ToString("yyyy-MM-dd")) }

[1]
|> List.iter (fun i -> createTask { emptyCreateTaskDto with content = $"Hello, world!{i}"; due_string = Some "every day starting today" })

let todoistTask =
    getTodayTasks ()
    |> List.filter _.content.Contains("Hello")
    |> List.head

todoistTask
|> (updateDueDatePreservingRecurring inFiveDays)
|> updateTask