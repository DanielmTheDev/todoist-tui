module Console.Workflows.CompleteButRemindLater

open Console.ScheduleToday
open Console.TaskQueries.Today
open Console.UserInteraction
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Dtos.CreateTaskDto
open TodoistAdapter.Types.State

let completeButRemindLater (state: State) (ui: UserInteraction) =
    async {
        let chosenTasks = chooseTodayTasksGroupedByLabel state ui
        let label = ui.chooseFrom ([""]@(state |> labelNames)) "🏷️"
        let! createResponses =
            chosenTasks
            |> List.map (fun task -> (task, ui.chooseFrom scheduleTimes $"What time should [blue]'{task.content}'[/] be scheduled"))
            |> List.map (fun taskWithTime ->
                let originalTask = fst taskWithTime
                { defaultCreateTask with content = originalTask.content; due_string = Some (snd taskWithTime); labels = Some [|label|] })
            |> List.map createTask
            |> Async.Parallel
            |> ui.spinnerArray "Creating reminders"

        let! completeResponses =
            chosenTasks
            |> List.map (fun task -> completeTask task.id)
            |> Async.Parallel
            |> ui.spinnerArray "Completing tasks"

        return (Array.append createResponses completeResponses) |> List.ofArray
    }