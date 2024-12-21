module Console.ConsoleQueries

open System
open Communication
open Console.Mapping
open Console.Types
open Spectre.Console
open SpectreCoff
open LocalState

type TasksGroupedByLabel = string * UpdateTaskDto list

defaultGroupedSelectionOptions <- { defaultGroupedSelectionOptions with Optional = true; PageSize = 30 }

let private displayColoredByPriority =
    fun updateTaskDto ->
        let color =
            match updateTaskDto.priority with
            | Some 4 -> Color.Red
            | Some 3 -> Color.Orange1
            | Some 2 -> Color.LightSteelBlue
            | _ -> Color.White
        (updateTaskDto.content
        |> Option.defaultValue "")
        |> markupString (Some color) []

let private emptyChoiceGroupsWithContentAsDisplay =
    { DisplayFunction = displayColoredByPriority
      Groups = [] }: ChoiceGroups<UpdateTaskDto>

let private appendGroup accChoiceGroups (tasksByLabel: TasksGroupedByLabel) =
    { accChoiceGroups with
        Groups =
            accChoiceGroups.Groups
            |> List.append
                [{ Group = { emptyUpdateTaskDto with content = Some (fst tasksByLabel) }
                   Choices =
                       (snd tasksByLabel)
                       |> List.sortByDescending (fun task -> task.priority |> Option.defaultValue Int32.MaxValue)
                       |> Array.ofList }] }

let private createChoiceGroup: TasksGroupedByLabel list -> ChoiceGroups<UpdateTaskDto> =
    List.fold appendGroup emptyChoiceGroupsWithContentAsDisplay

let addTask () =
    let content = ask "💬"
    let due = askSuggesting "tod" "⏲️"
    let label = chooseFrom labels "🏷️"
    { emptyCreateTaskDto with content = content; due_string = Some due; labels = Some [|label|] }
    |> createTask
    |> fun async -> [async]

let chooseFutureTasks () =
    chooseFrom (List.init 10 (fun i -> $"{i}")) "how many days?"
    |> int
    |> getAheadTasks
    |> List.filter (fun task -> task.due.Value.date > DateOnly.FromDateTime(DateTime.Now))

let chooseTodayTasksGroupedByLabel () =
    (getTodayTasks ())
    |> List.map toUpdateDto
    |> List.groupBy (fun task -> String.concat " " (Array.sort (Option.defaultValue [||] task.labels)))
    |> createChoiceGroup
    |> fun choices -> chooseGroupedFrom choices "which tasks"