module Console.Tasks

open System.Net.Http.Json
open Communication
open Console.Types
open SpectreCoff
open LocalState

let private postTask payload =
    async {
        let! response = httpClient.PostAsJsonAsync("tasks", payload) |> Async.AwaitTask

        if response.IsSuccessStatusCode then
            C "✅" |> toConsole
        else
            P $"🏮: Statuscode: {response.StatusCode}" |> toConsole
    }

let private getTodayTasks () =
    async {
        let uri = httpClient |> buildUriWithQuery "tasks" [("filter", "due today")]
        return httpClient.GetFromJsonAsync<TodoistTask List>(uri) |> Async.AwaitTask
    } |> Async.RunSynchronously

let addTask () =
    let content = ask "💬"
    let due = askSuggesting "tod" "⏲️"
    let label = chooseFrom labels "🏷️"

    { emptyTask with content = content; due_string = Some due; labels = Some [|label|] }
    |> postTask
    |> Async.RunSynchronously

let manipulateToday () =
    let tasks =
        (getTodayTasks ())
        |> Async.RunSynchronously
        |> List.map (_.content)
    let choices = chooseMultipleFrom tasks "which tasks"
    printf $"%A{choices[0]}"
    System.Console.ReadLine () |> ignore
    ()