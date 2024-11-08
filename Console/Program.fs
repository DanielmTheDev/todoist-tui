open Console
open SpectreCoff
open Tasks

let choices = ["Add Task"; "Manipulate Today"]

LocalState.init ()

while true do
    let choice = chooseFrom choices "What do you want to do?"

    match choice with
    | "Add Task" -> addTask
    | "Manipulate Today" -> failwith "todo"
    | _ -> failwith "Choice does not exist"

