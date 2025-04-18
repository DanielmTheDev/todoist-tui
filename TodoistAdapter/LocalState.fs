module TodoistAdapter.LocalState

open TodoistAdapter.SyncApi
open TodoistAdapter.Types.State

let mutable private state = defaultState

let private mergeLists existing updates idSelector =
    updates@existing
    |> List.distinctBy idSelector

let private mergeState current incremental =
    { current with
        sync_token = incremental.sync_token
        items = mergeLists current.items incremental.items _.id
        projects = mergeLists current.projects incremental.projects _.id
        labels = mergeLists current.labels incremental.labels _.id
        reminders = mergeLists current.reminders incremental.reminders _.id
        filters = mergeLists current.filters incremental.filters _.id }

let refreshedState () =
    async {
        let! incremental = syncAllResourcesIncremental ()
        state <- mergeState state incremental
        return state
    }
