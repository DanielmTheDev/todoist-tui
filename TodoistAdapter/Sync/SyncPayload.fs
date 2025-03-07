module TodoistAdapter.Sync.SyncPayload

type SyncPayload = {
    sync_token: string
    resource_types: string list
    commands: Commands.Command list option
}

let defaultPayload =
    { sync_token = "*"
      resource_types = []
      commands = None }