open Console
open Spectre.Console
open SpectreCoff
open Tasks

let choices = ["Add Task"; "Manipulate Today"]

LocalState.init ()

while true do
    AnsiConsole.Clear |> ignore
    let choice = chooseFrom choices "What do you want to do?"

    match choice with
    | "Add Task" -> addTask ()
    | "Manipulate Today" -> manipulateToday ()
    | _ -> failwith "Choice does not exist"

