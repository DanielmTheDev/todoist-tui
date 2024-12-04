open Console
open Console.PostponeToday
open Console.ResetPriorities
open Console.ScheduleToday
open SpectreCoff
open ConsoleQueries

let choices = ["Add Task"; "Schedule Today"; "Postpone Today"; "Reset Today's priority"]

LocalState.init ()

while true do
    let choice = chooseFrom choices "What do you want to do?"

    match choice with
    | "Add Task" -> addTask ()
    | "Schedule Today" -> scheduleToday ()
    | "Postpone Today" -> postponeToday ()
    | "Reset Today's priority" -> resetTodayPriority ()
    | _ -> failwith "Choice does not exist"

