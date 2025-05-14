module TodoistAdapter.Initialization

let initializeCommunication () =
    Http.configureClient ()
    SyncApi.initUser ()

let initializeAll () =
    initializeCommunication ()
    LocalState.refreshedState () |> ignore
    async.Return ()

