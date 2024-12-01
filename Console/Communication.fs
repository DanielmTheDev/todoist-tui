module Console.Communication

open System
open System.Net.Http
open System.Net.Http.Headers
open System.Net.Http.Json
open System.Web
open Console.Types
open SpectreCoff

let private httpClient =
    let client = new HttpClient()
    client.BaseAddress <- Uri("https://api.todoist.com/rest/v2/")
    client.DefaultRequestHeaders.Authorization <- AuthenticationHeaderValue("Bearer", "your-api-key")
    client

let private buildUriWithQuery (baseUrl: string) queryParams (client: HttpClient) =
    let fullUrl = Uri(client.BaseAddress, baseUrl)
    let uriBuilder = UriBuilder(fullUrl)
    let query = HttpUtility.ParseQueryString(uriBuilder.Query)
    for key, value in queryParams do
        query[key] <- value
    uriBuilder.Query <- query.ToString()
    uriBuilder.Uri.ToString()

let showResult (response: HttpResponseMessage) =
    if response.IsSuccessStatusCode then
        C "✅" |> toConsole
    else
        P $"🏮: Statuscode: {response.StatusCode}" |> toConsole

let requestLabels () =
    async {
        let! response = httpClient.GetFromJsonAsync<Label List>("labels") |> Async.AwaitTask
        return response |> List.map _.name
    } |> Async.RunSynchronously

let updateTask (payload: UpdateTaskDto) =
    async {
        let! response = httpClient.PostAsJsonAsync($"tasks/{payload.id}", payload)|> Async.AwaitTask
        response |> showResult
    } |> Async.RunSynchronously

let createTask (payload: CreateTaskDto) =
    async {
        let! response = httpClient.PostAsJsonAsync("tasks", payload) |> Async.AwaitTask
        response |> showResult
    } |> Async.RunSynchronously

let getTodayTasks () =
    async {
        let uri = httpClient |> buildUriWithQuery "tasks" [("filter", "due today")]
        return! httpClient.GetFromJsonAsync<TodoistTask List>(uri) |> Async.AwaitTask
    }
    |> Async.RunSynchronously

let getAheadTasks (daysAhead: int )=
    async {
        let uri = httpClient |> buildUriWithQuery "tasks" [("filter", $"{daysAhead} days")]
        return! httpClient.GetFromJsonAsync<TodoistTask List>(uri) |> Async.AwaitTask
    }
    |> Async.RunSynchronously