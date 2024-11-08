module Console.LocalState

open Labels

let mutable labels: string list = []

let init () =
    labels <- Array.toList (Array.append [|""|] (requestLabels ()))