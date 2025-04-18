module Console.AddTaskWithLoadBalancing

open System
open System.Net
open Console.UserInteraction
open Console.LoadBalancing
open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Dtos.CreateTaskDto
open TodoistAdapter.Types
open TodoistAdapter.Types.Due
open TodoistAdapter.Types.State

let addTaskWithLoadBalancing (state: State) (ui: UserInteraction) =
    async {
        let content = ui.ask "💬"
        let today = todaysDate ()
        let endDate = today.AddDays (ui |> daysAhead)

        let leastLoadedDay =
            tasksBetweenDates state today endDate
             |> groupTaskNumberByDate
             |> function
            | [] -> DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            | loadList -> loadList.Head.date

        let! op =
            { defaultCreateTask with
                content = content
                due_string = Some (DueString.fromDateOnly leastLoadedDay)
                priority = Some 4 }
            |> createTask
            |> ui.spinner "Adding"

        if op.statusCode >= HttpStatusCode.OK then
             ui.print $"Task created in [blue]{(leastLoadedDay.DayNumber - today.DayNumber)}[/] days"
        else
            ui.print $"Failed to create task (status code: {op.statusCode})"

        return [op]
    }