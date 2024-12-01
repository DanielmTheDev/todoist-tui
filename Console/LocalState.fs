module Console.LocalState

open Communication

let mutable labels: string list = []

let init () =
    labels <- [""]@requestLabels ()