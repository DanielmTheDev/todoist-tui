module Console.TaskQueries.Add

open Console.UserInteraction
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Dtos.CreateTaskDto
open TodoistAdapter.Types.State
open TodoistAdapter.Types.TodoistTask

type TasksGroupedByLabel = string * Task list

let addTask (state: State) (ui: UserInteraction) =
    async {
        let content = ui.ask "💬"
        let due = ui.askSuggesting "tod" "⏲️"
        let label = ui.chooseFrom ([""]@(state |> labelNames)) "🏷️"
        let! response =
            { defaultCreateTask with content = content; due_string = Some due; labels = Some [|label|] }
            |> createTask
        return [response]
    }


