#r "bin/Debug/net9.0/Console.dll"
#r "nuget: EluciusFTW.SpectreCoff, 0.49.4"
#r "nuget: FsHttp"
#r "nuget: FSharp.Data"
open System
open System.IO
open System.Text.RegularExpressions
open Console.Communication
open Console.ConsoleQueries
open Console.Types
open Console.Mapping
open Console.Types
open FsHttp
open FsHttp.Dsl.Print

// List.init 1 (fun i -> { emptyCreateTaskDto with content = $"Check something"; due_string = Some "today"; priority = Some 4 })
// |> List.map createTask
// |> Async.Parallel
// |> Async.StartAsTask

let payload =
    {|  sync_token = "*"
        resource_types = ["labels"] |}

http {
    POST "sync"
    body
    jsonSerialize payload
}
|> Request.send
|> Response.deserializeJson<SyncResponse>


