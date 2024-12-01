module Console.ConsoleQueries

open System
open Communication
open Console.Mapping
open Console.Types
open SpectreCoff
open LocalState

type concatLabelsWithTheirTasks = string * UpdateTaskDto list

let private emptyChoiceGroupsWithContentAsDisplay=
    { DisplayFunction = (fun updateTaskDto -> updateTaskDto.content |> Option.defaultValue ""); Groups = [] }: ChoiceGroups<UpdateTaskDto>

let private appendLabelGroup accChoiceGroups (labelsAsString, tasks) =
    { accChoiceGroups with Groups = accChoiceGroups.Groups |> List.append [{ Group = { emptyUpdateTaskDto with content = Some labelsAsString }; Choices = Array.ofList tasks }] }

let private createChoiceGroup: concatLabelsWithTheirTasks list -> ChoiceGroups<UpdateTaskDto> =
    List.fold appendLabelGroup emptyChoiceGroupsWithContentAsDisplay

let addTask () =
    let content = ask "💬"
    let due = askSuggesting "tod" "⏲️"
    let label = chooseFrom labels "🏷️"
    { emptyCreateTaskDto with content = content; due_string = Some due; labels = Some [|label|] }
    |> createTask

let chooseFutureTasks () =
    chooseFrom ["2"; "3"; "4"; "5"] "how many days?"
    |> int
    |> getAheadTasks
    |> List.filter (fun task -> task.due.Value.date > DateOnly.FromDateTime(DateTime.Now))

let chooseTodayTasksGroupedByLabel () =
    (getTodayTasks ())
    |> List.map toUpdateDto
    |> List.groupBy (fun task -> String.concat " " (Array.sort (Option.defaultValue [||] task.labels)))
    |> createChoiceGroup
    |> fun choices -> chooseGroupedFrom choices "which tasks"