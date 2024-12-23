module TodoistAdapter.TaskDateUpdating

open System
open System.Text.RegularExpressions
open TodoistAdapter.Types

let private appendStartDate (recurringPatten: string) (date: DateOnly) =
    let asString = date.ToString("dd/MM")
    match recurringPatten with
    | "" -> asString
    | _ -> @$"{recurringPatten} starting {asString}"

let private extractRecurringPattern dueString =
    let pattern = @"\b(every\s+.*?)(?=\s+starting|\s+ending|$)"
    let result = Regex.Match(dueString, pattern, RegexOptions.IgnoreCase)
    if result.Success
        then result.Groups[1].Value.Trim()
    else ""

let private addDatePreservingRecurring = extractRecurringPattern >> appendStartDate

let updateDueDatePreservingRecurring (date: DateOnly) (task: UpdateTaskDto) : UpdateTaskDto =
    match task.due_string with
    | Some currentDueString when not (String.IsNullOrWhiteSpace currentDueString) ->
        let dueString = addDatePreservingRecurring currentDueString date
        { task with due_date = None; due_string = Some dueString }
    | _ -> { task with due_string = None; due_date = Some (date.ToString("yyyy-MM-dd")) }