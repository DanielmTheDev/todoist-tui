module TodoistAdapter.Sync.Args.Arg

open System.Text.Json
open System.Text.Json.Serialization
open TodoistAdapter.Sync.Args.ItemDeleteArgs
open TodoistAdapter.Sync.Args.ItemUpdateArgs
open TodoistAdapter.Sync.Args.ReminderAddArgs

type Args =
    | ItemUpdate of UpdateItem
    | ItemDelete of DeleteItem
    | ReminderAdd of AddReminder

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