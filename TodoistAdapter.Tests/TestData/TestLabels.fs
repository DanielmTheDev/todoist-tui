[<RequireQualifiedAccess>]
module TodoistAdapter.Tests.TestData.TestLabels

open FsHttp
open TodoistAdapter.Initialization
open TodoistAdapter.SyncApi

do initializeAll () |> Async.RunSynchronously

let deleteAllExisting () =
    async {
        let! allItems = fullSync ()
        return! allItems.items |> deleteTasks
    }
    |> Async.RunSynchronously
    |> Response.assert2xx
    |> ignore

