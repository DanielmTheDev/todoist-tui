module Console.ScheduleToday

open Console.Communication
open Console.LocalState
open Console.Mapping
open Console.Time
open Console.Types
open SpectreCoff

type concatLabelsWithTheirTasks = string * UpdateTaskDto list

let private createTaskWithNewTime task =
    let updatedTime = chooseFrom scheduleTimes $"What time should '{task.content}' be scheduled"
    { task with due_string = Some updatedTime; due_datetime = None; due_date = None }

let private emptyChoiceGroupsWithContentAsDisplay=
    { DisplayFunction = (fun updateTaskDto -> updateTaskDto.content |> Option.defaultValue ""); Groups = [] }: ChoiceGroups<UpdateTaskDto>

let private appendLabelGroup accChoiceGroups (labelsAsString, tasks) =
    { accChoiceGroups with Groups = accChoiceGroups.Groups |> List.append [{ Group = { emptyUpdateTaskDto with content = Some labelsAsString }; Choices = Array.ofList tasks }] }

let createChoiceGroup: concatLabelsWithTheirTasks list -> ChoiceGroups<UpdateTaskDto> =
    List.fold appendLabelGroup emptyChoiceGroupsWithContentAsDisplay

let private askForNewLabel tasks =
    let newLabel = chooseFrom labels "Add new label to manipulated tasks"
    match newLabel with
    | "" -> tasks
    | label -> tasks |> List.map (fun task -> { task with labels = Some [|label|] }: UpdateTaskDto)

let scheduleToday () =
    (getTodayTasks ())
    |> List.map toUpdateDto
    |> List.groupBy (fun task -> String.concat " " (Array.sort (Option.defaultValue [||] task.labels)))
    |> createChoiceGroup
    |> fun choices -> chooseGroupedFrom choices "which tasks"
    |> List.map createTaskWithNewTime
    |> askForNewLabel
    |> List.iter updateTask