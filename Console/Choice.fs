module Console.Choice

type TaskChoice =
    | AddTask
    | AddTaskWithLoadBalancing
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

let choices =
    [ AddTask
      AddTaskWithLoadBalancing
      CompleteTasks
      ScheduleToday
      CollectUnderNewParent
      PostponeToday
      ResetTodayPriority ]