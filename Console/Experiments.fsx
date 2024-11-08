#r "bin/Debug/net8.0/Console.dll"

open System.Net.Http.Json
open Console.Communication
open Console.Types

let private requestLabels () =
    async {
        let! response = httpClient.GetFromJsonAsync<Label array>("labels") |> Async.AwaitTask
        return response |> Array.map (_.name)
    } |> Async.RunSynchronously

requestLabels ()