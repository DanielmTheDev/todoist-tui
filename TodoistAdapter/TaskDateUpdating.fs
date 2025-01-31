module TodoistAdapter.TaskDateUpdating

open System
open System.Text.RegularExpressions
open TodoistAdapter.RestTypes

let private appendStartDate (recurringPatten: string) (date: DateTime) =
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

let updateDueDatePreservingRecurring (date: DateTime) (task: TodoistTask) : TodoistTask =
    match task.due with
    | Some due when not (String.IsNullOrWhiteSpace due.string) ->
        let dueString = addDatePreservingRecurring due.string date
        { task with due = Some { due with string = dueString } }
    | _ -> { task with due = Some { task.due.Value with date = Some date } }