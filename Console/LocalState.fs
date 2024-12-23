module Console.LocalState

open TodoistAdapter.Communication

let mutable labels: string list = []

let init () =
    init ()
    labels <- [""]@requestLabels ()