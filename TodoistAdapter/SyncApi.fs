module TodoistAdapter.SyncApi

open FsHttp
open TodoistAdapter.Sync
open TodoistAdapter.Sync.SyncPayload
open TodoistAdapter.Types.State
open TodoistAdapter.Types.TodoistTask

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

        let syncResponse =
            response
            |> Response.assert2xx
            |> Response.deserializeJson<State>

        incrementalSyncToken <- syncResponse.sync_token
        return syncResponse
    }

let fullSync () = sync ["all"] "*"
let fullSyncResources resources = sync resources "*"
let syncIncremental () = sync ["all"] incrementalSyncToken |> Async.RunSynchronously
let syncAllResourcesIncremental () = sync ["all"] incrementalSyncToken

let initUser () =
    let sync = (fullSyncResources ["user"] |> Async.RunSynchronously)
    userId <- sync.user.id
    incrementalSyncToken <- "*"

let moveBelowParent parentId childIds =
    let commands =
        childIds
        |> List.map (Commands.MoveItem.moveItem parentId)

    http {
    POST syncApiUrl
    body
    jsonSerialize {| commands = commands |}
    }
    |> Request.sendAsync

let updateTask (tasks: Task list) =
    let commands =
        tasks
        |> List.collect (fun t -> Commands.UpdateItem.itemUpdateCommand t userId)
    http {
    POST syncApiUrl
    body
    jsonSerialize { defaultPayload with commands = Some commands }
    }
    |> Request.sendAsync

let deleteTasks (tasks: Task list) =
    let commands =
        tasks
        |> List.map Commands.DeleteItem.deleteItem
    http {
    POST syncApiUrl
    body
    jsonSerialize { defaultPayload with commands = Some commands }
    }
    |> Request.sendAsync

