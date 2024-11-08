#r "bin/Debug/net8.0/Console.dll"

open System.Net.Http.Json
open Console.Communication
open Console.Types

let private requestLabels () =
    async {
        let! response = httpClient.GetFromJsonAsync<AddTaskPayload array>("tasks?filter=due%20today") |> Async.AwaitTask
        return response
    } |> Async.RunSynchronously

requestLabels ()