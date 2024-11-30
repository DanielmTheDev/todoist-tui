module Console.Tasks

open Communication
open Console.Types
open SpectreCoff
open LocalState

let addTask () =
    let content = ask "💬"
    let due = askSuggesting "tod" "⏲️"
    let label = chooseFrom labels "🏷️"
    { emptyCreateTaskDto with content = content; due_string = Some due; labels = Some [|label|] }
    |> createTask
