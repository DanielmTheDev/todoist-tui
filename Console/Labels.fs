module Console.Labels

open System.Net.Http.Json
open Communication
open Console.Types

let requestLabels () =
    async {
        let! response = httpClient.GetFromJsonAsync<Label List>("labels") |> Async.AwaitTask
        return response |> List.map (_.name)
    } |> Async.RunSynchronously