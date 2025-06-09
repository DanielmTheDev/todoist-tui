module Console.Choice

type TaskChoice =
    | AddTask
    | AddTaskWithLoadBalancing
    | CompleteButAddReminder
    | CompleteTasks
    | ScheduleToday
    | CollectUnderNewParent
    | PostponeToday
    | ResetTodayPriority

let choiceToString =
    function
    | AddTask -> "Add Task"
    | AddTaskWithLoadBalancing -> "Add task with load balancing"
    | CompleteTasks -> "Complete Tasks"
    | ScheduleToday -> "Schedule Today"
    | CollectUnderNewParent -> "Collect under new parent task"
    | PostponeToday -> "Postpone Today"
    | ResetTodayPriority -> "Reset Today's priority"
    | CompleteButAddReminder -> "Complete Task, but add reminder for today"

let choices =
    [ AddTask
      AddTaskWithLoadBalancing
      CompleteButAddReminder
      CompleteTasks
      ScheduleToday
      CollectUnderNewParent
      PostponeToday
      ResetTodayPriority ]