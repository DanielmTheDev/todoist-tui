module Console.Labels

open System.Net.Http.Json
open Communication
open Console.Types

let requestLabels () =
    async {
        let! response = httpClient.GetFromJsonAsync<Label array>("labels") |> Async.AwaitTask
        return response |> Array.map (_.name)
    } |> Async.RunSynchronously