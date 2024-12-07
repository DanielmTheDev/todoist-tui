module Console.LocalState

open Communication

let mutable labels: string list = []

let init () =
    init ()
    labels <- [""]@requestLabels ()