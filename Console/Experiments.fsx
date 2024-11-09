#r "bin/Debug/net8.0/Console.dll"

open System
open System.Net.Http
open System.Net.Http.Json
open System.Web
open Console.Communication
open Console.Types

let buildUriWithQuery (baseUrl: string) queryParams (client: HttpClient) =
    let fullUrl = Uri(client.BaseAddress, baseUrl)
    let uriBuilder = UriBuilder(fullUrl)
    let query = HttpUtility.ParseQueryString(uriBuilder.Query)
    for key, value in queryParams do
        query.[key] <- value
    uriBuilder.Query <- query.ToString()
    uriBuilder.Uri.ToString()

let private requestTasks () =
    async {
        let uri = httpClient |>buildUriWithQuery "tasks" [("filter", "due today")]

        uri |> printfn "%s"
        let! response = httpClient.GetFromJsonAsync<TodoistTask array>(uri) |> Async.AwaitTask
        return response
    } |> Async.RunSynchronously


requestTasks ()

