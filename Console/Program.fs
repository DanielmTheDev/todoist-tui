open Console
open SpectreCoff
open Tasks

let choices = ["Add Task"; "Manipulate Today"]

LocalState.init ()

while true do
    let choice = chooseFrom choices "What do you want to do?"

    match choice with
    | "AddTask" -> addTask
    | "Manipulate" -> failwith "todo"
    | _ -> failwith "Choice does not exist"

