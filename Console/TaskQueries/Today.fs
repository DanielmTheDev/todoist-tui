module Console.Queries.chooseTodayTasksGroupedByLabel

open System
open Console.ConsoleQueries
open Console.UserInteraction
open Spectre.Console
open SpectreCoff
open TodoistAdapter.Types.Due
open TodoistAdapter.Types.State
open TodoistAdapter.Types.TodoistTask

do
    defaultGroupedSelectionOptions <- { defaultGroupedSelectionOptions with Optional = true; PageSize = 30 }

let private colorByPriority (task: Task) =
    let color =
        match task.priority with
        | Some 4 -> Color.Red
        | Some 3 -> Color.Orange1
        | Some 2 -> Color.LightSteelBlue
        | _ -> Color.White
    { task with content = task.content |> markupString (Some color) [] }

let private prependRecurringSymbol (task: Task) =
    let content =
        match task.due with
        | Some due when due.is_recurring -> $"♻️ {task.content}"
        | _ -> task.content
    { task with content = content }

let private styleContent (task: Task) =
    task
    |> colorByPriority
    |> prependRecurringSymbol
    |> _.content

let private appendGroup (accChoiceGroups: ChoiceGroups<Task>) (tasksByLabel: TasksGroupedByLabel) =
    { accChoiceGroups with
        Groups =
            accChoiceGroups.Groups
            |> List.append
                [{ Group = { defaultTask with content = fst tasksByLabel }
                   Choices =
                       (snd tasksByLabel)
                       |> List.sortByDescending (fun task -> task.priority |> Option.defaultValue Int32.MaxValue)
                       |> Array.ofList }] }

let private emptyChoiceGroupsWithContentAsDisplay =
    { DisplayFunction = styleContent
      Groups = [] }: ChoiceGroups<Task>

let private createChoiceGroup: TasksGroupedByLabel list -> ChoiceGroups<Task> =
    List.fold appendGroup emptyChoiceGroupsWithContentAsDisplay

let chooseTodayTasksGroupedByLabel (state: State) (ui: UserInteraction) =
    state |> todaysItems
    |> List.groupBy (fun task -> String.concat " " (List.sort (Option.defaultValue [] task.labels)))
    |> createChoiceGroup
    |> fun choices -> ui.chooseGroupedFromWith defaultGroupedSelectionOptions choices "which tasks"