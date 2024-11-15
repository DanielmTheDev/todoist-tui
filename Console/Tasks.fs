module Console.Tasks

open System.Net.Http.Json
open Communication
open Console.Mapping
open Console.Types
open SpectreCoff
open LocalState
open Time

let private updateTask (payload: UpdateTaskDto) =
    async {
        let! response = httpClient.PostAsJsonAsync($"tasks/{payload.id}", payload) |> Async.AwaitTask
        if response.IsSuccessStatusCode then
            C "✅" |> toConsole
        else
            P $"🏮: Statuscode: {response.StatusCode}" |> toConsole
    } |> Async.RunSynchronously

let private createTask (payload: CreateTaskDto) =
    async {
        let! response = httpClient.PostAsJsonAsync("tasks", payload) |> Async.AwaitTask

        if response.IsSuccessStatusCode then
            C "✅" |> toConsole
        else
            P $"🏮: Statuscode: {response.StatusCode}" |> toConsole
    } |> Async.RunSynchronously

let private getTodayTasks () =
    async {
        let uri = httpClient |> buildUriWithQuery "tasks" [("filter", "due today")]
        return! httpClient.GetFromJsonAsync<TodoistTask List>(uri) |> Async.AwaitTask
    }
    |> Async.RunSynchronously

let private createTaskWithNewTime task =
    let updatedTime = chooseFrom scheduleTimes $"What time should '{task.content}' be scheduled"
    { task with due_string = Some updatedTime; due_datetime = None; due_date = None }

let createChoiceGroup =
    List.fold
        (fun accChoiceGroups (labelsAsString, tasks) -> { accChoiceGroups with Groups = accChoiceGroups.Groups |> List.append [{ Group = { emptyUpdateTaskDto with content = Some labelsAsString }; Choices = Array.ofList tasks }] })
        ({ DisplayFunction = (fun updateTaskDto -> updateTaskDto.content |> Option.defaultValue ""); Groups = [] }: ChoiceGroups<UpdateTaskDto>)

let private askForNewLabel tasks =
    let newLabel = chooseFrom labels "Add new label to manipulated tasks"
    match newLabel with
    | "" -> tasks
    | label -> tasks |> List.map (fun task -> { task with labels = Some [|label|] })

let addTask () =
    let content = ask "💬"
    let due = askSuggesting "tod" "⏲️"
    let label = chooseFrom labels "🏷️"
    { emptyCreateTaskDto with content = content; due_string = Some due; labels = Some [|label|] }
    |> createTask

let scheduleToday () =
    (getTodayTasks ())
    |> List.map toUpdateDto
    |> List.groupBy (fun task -> String.concat " " (Array.sort (Option.defaultValue [||] task.labels)))
    |> createChoiceGroup
    |> fun choices -> chooseGroupedFrom choices "which tasks"
    |> List.map createTaskWithNewTime
    |> askForNewLabel
    |> List.iter updateTask