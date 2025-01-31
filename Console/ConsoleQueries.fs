module Console.ConsoleQueries

open System
open Console.UserInteraction
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.RestTypes
open Spectre.Console
open SpectreCoff
open LocalState

type TasksGroupedByLabel = string * TodoistTask list

do
    defaultGroupedSelectionOptions <- { defaultGroupedSelectionOptions with Optional = true; PageSize = 30 }

let prependRecurringSymbol task =
    let content =
        match task.due with
        | Some due when due.is_recurring -> $"♻️ {task.content}"
        | _ -> task.content
    { task with content = content }

let private colorByPriority (task: TodoistTask) =
    let color =
        match task.priority with
        | Some 4 -> Color.Red
        | Some 3 -> Color.Orange1
        | Some 2 -> Color.LightSteelBlue
        | _ -> Color.White
    { task with content = task.content |> markupString (Some color) [] }

let private styleContent (task: TodoistTask) =
    task
    |> colorByPriority
    |> prependRecurringSymbol
    |> _.content

let private emptyChoiceGroupsWithContentAsDisplay =
    { DisplayFunction = styleContent
      Groups = [] }: ChoiceGroups<TodoistTask>

let private appendGroup (accChoiceGroups: ChoiceGroups<TodoistTask>) (tasksByLabel: TasksGroupedByLabel) =
    { accChoiceGroups with
        Groups =
            accChoiceGroups.Groups
            |> List.append
                [{ Group = { emptyTodoistTask with content = fst tasksByLabel }
                   Choices =
                       (snd tasksByLabel)
                       |> List.sortByDescending (fun task -> task.priority |> Option.defaultValue Int32.MaxValue)
                       |> Array.ofList }] }

let private createChoiceGroup: TasksGroupedByLabel list -> ChoiceGroups<TodoistTask> =
    List.fold appendGroup emptyChoiceGroupsWithContentAsDisplay

let addTask (ui: UserInteraction) =
    async {
        let content = ui.ask "💬"
        let due = ui.askSuggesting "tod" "⏲️"
        let label = ui.chooseFrom labels "🏷️"
        let! response
            = { emptyCreateTaskDto with content = content; due_string = Some due; labels = Some [|label|] }
            |> createTask
        return [response]
    }

let chooseFutureTasks ui =
    ui.chooseFrom (List.init 10 (fun i -> $"{i}")) "how many days?"
    |> int
    |> getAheadTasks
    |> List.filter (fun task -> task.due.Value.date.Value > DateTime.Now)

let chooseTodayTasksGroupedByLabel (ui: UserInteraction) =
    async {
        let! tasks = getTodayTasks ()
        return tasks
        |> List.groupBy (fun task -> String.concat " " (Array.sort (Option.defaultValue [||] task.labels)))
        |> createChoiceGroup
        |> fun choices -> ui.chooseGroupedFromWith defaultGroupedSelectionOptions choices "which tasks"
    }
