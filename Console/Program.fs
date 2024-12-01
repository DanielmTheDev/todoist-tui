open Console
open Console.PostponeToday
open Console.ScheduleToday
open SpectreCoff
open ConsoleQueries

let choices = ["Add Task"; "Schedule Today"; "Postpone Today"]

LocalState.init ()

while true do
    let choice = chooseFrom choices "What do you want to do?"

    match choice with
    | "Add Task" -> addTask ()
    | "Schedule Today" -> scheduleToday ()
    | "Postpone Today" -> postponeToday ()
    | _ -> failwith "Choice does not exist"

