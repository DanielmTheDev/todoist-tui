module TodoistAdapter.Types.DueString

open System

type DueString = string

let fromDateOnly (dateOnly: DateOnly) =
    DueString (dateOnly.ToString("dd/MM/yyyy"))

