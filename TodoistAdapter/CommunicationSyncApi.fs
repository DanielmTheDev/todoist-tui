module TodoistAdapter.CommunicationSyncApi

open System
open TodoistAdapter.SyncTypes
open TodoistAdapter.RestTypes
open FsHttp

let syncApiUrl = "https://api.todoist.com/sync/v9/sync"
let mutable incrementalSyncToken = "*"
let mutable userId = 0

let sync resourceTypes syncToken =
    async {
        let payload =
            { defaultPayload with
                sync_token = syncToken
                resource_types = resourceTypes }

        let! response =
            http {
                POST syncApiUrl
                body
                jsonSerialize payload
            }
            |> Request.sendAsync
        let syncResponse = response.DeserializeJson<SyncResponse>()
        incrementalSyncToken <- syncResponse.sync_token
        return syncResponse
    }

let fullSync () = sync ["all"] "*"
let fullSyncResources resources = sync resources "*"
let syncIncremental () = sync ["all"] incrementalSyncToken
let syncAllResourcesIncremental () = sync ["all"] incrementalSyncToken

let apiKey = $"""Bearer {Environment.GetEnvironmentVariable("TODOIST_API_TOKEN")}"""

let init () = // todo: this should be done in a seperate module, possibly in do clause
    GlobalConfig.defaults
    |> Config.transformHeader (fun header ->
            { header with headers = header.headers.Add("Authorization", apiKey) })
    |> GlobalConfig.set

    userId <-
        (fullSyncResources ["user"]
        |> Async.RunSynchronously).user.id

let moveBelowParent parentId childIds =
    let commands =
        childIds
        |> List.map (fun childId ->
            { ``type`` = "item_move"
              temp_id = None
              uuid = (Guid.NewGuid ()).ToString()
              args = ItemUpdate { emptyUpdateArgs with id = childId; parent_id = Some parentId } })
    http {
    POST syncApiUrl
    body
    jsonSerialize {| commands = commands |}
    }
    |> Request.sendAsync

let reminderCommand (id: string) =
    let tempId = (Guid.NewGuid ()).ToString()
    { ``type`` = "reminder_add"
      uuid = (Guid.NewGuid ()).ToString()
      temp_id = Some tempId
      args =
        ReminderAdd
            { emptyReminderArgs with
                  id = tempId
                  minute_offset = 0
                  notify_uid = Some userId
                  item_id = Some id } }

let itemUpdateCommand (task: TodoistTask) =
    let updateCommand =
        { ``type`` = "item_update"
          uuid = (Guid.NewGuid ()).ToString()
          temp_id = None
          args =
            ItemUpdate
                { emptyUpdateArgs with
                   id = task.id
                   priority = Option.defaultValue 4 task.priority
                   due = task.due |> Option.map
                       (fun due ->
                           { date = match due.string with | "" -> due.date | _ -> None
                             is_recurring = false
                             string = Option.defaultValue "" (task.due |> Option.map _.string)
                             datetime = None
                             timezone = None } ) } }
    match task.due with
    | Some _ -> [updateCommand; reminderCommand task.id]
    | None -> [updateCommand]

let itemDeleteCommand (task: TodoistTask) =
    { ``type`` = "item_delete"
      uuid = (Guid.NewGuid ()).ToString()
      temp_id = None
      args = ItemDelete { id = task.id } }

let updateTask (tasks: TodoistTask list) =
    let commands =
        tasks
        |> List.collect itemUpdateCommand
    http {
    POST syncApiUrl
    body
    jsonSerialize { defaultPayload with commands = Some commands }
    }
    |> Request.sendAsync

let deleteTasks (tasks: TodoistTask list) =
    let commands =
        tasks
        |> List.map itemDeleteCommand
    http {
    POST syncApiUrl
    body
    jsonSerialize { defaultPayload with commands = Some commands }
    }
    |> Request.sendAsync

