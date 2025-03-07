[<RequireQualifiedAccess>]
module Console.State

open TodoistAdapter
open TodoistAdapter.Types.State

let mutable state = defaultState

let init () =
    Http.configureClient ()
    SyncApi.initUser ()