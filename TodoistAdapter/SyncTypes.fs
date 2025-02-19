module TodoistAdapter.SyncTypes

open System.Text.Json
open System.Text.Json.Serialization
open TodoistAdapter.RestTypes

type Reminder = {
    id: string
    notify_uid: string
    item_id: string
    ``type``: string
    due: Due
    minute_offset: int
    is_deleted: bool
}

type User = {
    id: int
}

type SyncResponse = {
    sync_token: string
    full_sync: bool
    items: TodoistTask list
    projects: TodoistProject list
    labels: TodoistLabel list
    reminders: Reminder list
    user: User
}

type ItemUpdateArgs =
    { id: string
      parent_id: string option
      labels: string list option
      priority: int
      due: Due option }

type ReminderAddArgs =
    { id: string
      ``type``: string
      minute_offset: int
      notify_uid: int option
      item_id: string option }

type ItemDeleteArgs = { id: string }

type Args =
    | ItemUpdate of ItemUpdateArgs
    | ItemDelete of ItemDeleteArgs
    | ReminderAdd of ReminderAddArgs

type ArgsWriteOnlyConverter() =
    inherit JsonConverter<Args>()

    override _.Write(writer, value, options) =
        match value with
        | ItemUpdate update ->
            JsonSerializer.Serialize(writer, update, options)
        | ReminderAdd reminder ->
            JsonSerializer.Serialize(writer, reminder, options)
        | ItemDelete delete ->
            JsonSerializer.Serialize(writer, delete, options)

    override _.Read(reader, _, _) =
        raise (JsonException("Reading 'Args' from JSON is not supported."))

type Command =
    { ``type``: string
      uuid: string
      temp_id: string option
      [<System.Text.Json.Serialization.JsonConverter(typeof<ArgsWriteOnlyConverter>)>]
      args: Args }

type SyncPayload = {
    sync_token: string
    resource_types: string list
    commands: Command list option
}

let defaultPayload =
    { sync_token = "*"
      resource_types = []
      commands = None }

let emptyUpdateArgs: ItemUpdateArgs =
    { id = ""
      parent_id = None
      labels = None
      priority = 4
      due = None }

let emptyReminderArgs: ReminderAddArgs =
    { id = ""
      ``type`` = "relative"
      minute_offset = 0
      notify_uid = None
      item_id = None }