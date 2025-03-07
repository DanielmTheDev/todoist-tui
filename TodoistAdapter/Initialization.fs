module TodoistAdapter.Initialization

let initialize () =
    Http.configureClient ()
    SyncApi.initUser ()
    LocalState.refreshedState () |> ignore