module Console.Choice

type TaskChoice =
    | AddTask
    | CompleteTasks
    | ScheduleToday
    | CollectUnderNewParent
    | PostponeToday
    | ResetTodayPriority

let choiceToString =
    function
    | AddTask -> "Add Task"
    | CompleteTasks -> "Complete Tasks"
    | ScheduleToday -> "Schedule Today"
    | CollectUnderNewParent -> "Collect under new parent task"
    | PostponeToday -> "Postpone Today"
    | ResetTodayPriority -> "Reset Today's priority"

let choices =
    [ AddTask
      CompleteTasks
      ScheduleToday
      CollectUnderNewParent
      PostponeToday
      ResetTodayPriority ]