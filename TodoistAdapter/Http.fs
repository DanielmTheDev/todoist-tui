module TodoistAdapter.Http

open System
open FsHttp

let apiKey = $"""Bearer {Environment.GetEnvironmentVariable("TODOIST_API_TOKEN")}"""

let configureClient () =
    GlobalConfig.defaults
    |> Config.transformHeader (fun header ->
        { header with headers = header.headers.Add("Authorization", apiKey) })
    |> GlobalConfig.set
