#r "bin/Debug/net9.0/Console.dll"
#r "nuget: EluciusFTW.SpectreCoff, 0.49.4"
#r "nuget: FsHttp"

open System
open System.Text.RegularExpressions
open Console.Communication
open Console.Types
open Console.Mapping
open Console.Types
open FsHttp

let init () =
    GlobalConfig.defaults
    |> Config.useBaseUrl "https://api.todoist.com/rest/v2/"
    |> Config.transformHeader (fun header ->
        { header with headers = header.headers.Add("Authorization", "Bearer c26345440c983ecc88f94f5171ed8404500b4207") })
    |> GlobalConfig.set

let createTask (payload: CreateTaskDto) =
    http {
        POST "tasks"
        body
        jsonSerialize payload
    }
    |> Request.sendAsync

init ()

List.init 5 (fun i -> { emptyCreateTaskDto with content = $"Test Task {i}"; due_string = Some "today"; priority = Some 4 })
|> List.map createTask
|> Async.Parallel
|> Async.RunSynchronously
