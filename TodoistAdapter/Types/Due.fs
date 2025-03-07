module TodoistAdapter.Types.Due

open System
open TodoistAdapter.Types.DueDate

type Due = {
    [<System.Text.Json.Serialization.JsonConverter(typeof<OptionDueDateConverter>)>]
    date: DueDate option
    timezone: string option
    is_recurring: bool
    string: string
    datetime: DateTime option
}

let defaultDue = {
    date = None
    timezone = None
    is_recurring = false
    string = ""
    datetime = None
}

let todaysDate () =
    DateOnly.FromDateTime DateTime.Today

let dueDateOf due =
    due
    |> Option.bind _.date
    |> Option.map dateOnlyOf
