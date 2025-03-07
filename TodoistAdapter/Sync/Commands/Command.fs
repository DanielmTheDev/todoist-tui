namespace TodoistAdapter.Sync.Commands

open TodoistAdapter.Sync.Args.Arg

type Command =
    { ``type``: string
      uuid: string
      temp_id: string option
      [<System.Text.Json.Serialization.JsonConverter(typeof<ArgsWriteOnlyConverter>)>]
      args: Args }