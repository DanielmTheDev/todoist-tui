[<RequireQualifiedAccess>]
module Console.State

open System.Threading
open TodoistAdapter
open TodoistAdapter.LocalState
open TodoistAdapter.Types.State

let mutable state = defaultState

let init () =
    Http.configureClient ()
    SyncApi.initUser ()

let refreshPeriodically () =
    new Timer(
            (fun _ ->
                try
                    refreshedState () |> ignore
                with _ -> ()),
            null,
            30000,
            30000)