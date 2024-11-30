open Console
open Console.ScheduleToday
open SpectreCoff
open Tasks

let choices = ["Add Task"; "Schedule Today"]

LocalState.init ()

while true do
    let choice = chooseFrom choices "What do you want to do?"

    match choice with
    | "Add Task" -> addTask ()
    | "Schedule Today" -> scheduleToday ()
    | _ -> failwith "Choice does not exist"

