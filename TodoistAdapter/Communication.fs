module TodoistAdapter.Communication

open System
open TodoistAdapter.Types
open FsHttp

let restApiUrl = "https://api.todoist.com/rest/v2"
let syncApiUrl = "https://api.todoist.com/sync/v9/sync"

let mutable syncToken = "*"

let init ()=
    GlobalConfig.defaults
    |> Config.transformHeader (fun header ->
        { header with headers = header.headers.Add("Authorization", "Bearer c26345440c983ecc88f94f5171ed8404500b4207") })
    |> GlobalConfig.set

let requestLabels () =
    http {
        GET $"{restApiUrl}/labels"
    }
    |> Request.send
    |> fun response -> response.DeserializeJson<TodoistLabel list> ()
    |> List.map _.name


let updateTask (payload: UpdateTaskDto) =
    http {
        POST $"{restApiUrl}/tasks/{payload.id}"
        body
        jsonSerialize payload
    }
    |> Request.sendAsync

let createTask (payload: CreateTaskDto) =
    http {
        POST $"{restApiUrl}/tasks"
        body
        jsonSerialize payload
    }
    |> Request.sendAsync

let completeTask (taskId: string) =
    http {
        POST $"{restApiUrl}/tasks/{taskId}/close"
    }
    |> Request.sendAsync

let getTodayTasks () =
    http {
        GET $"{restApiUrl}/tasks"
        query [ "filter", "due today" ]
    }
    |> Request.sendAsync
    |> fun asyncResponse ->
        async {
            let! response = asyncResponse
            return response.DeserializeJson<TodoistTask list> ()
        }

let getAheadTasks (daysAhead: int) =
    http {
        GET $"{restApiUrl}/tasks"
        query [ "filter", $"{daysAhead} days" ]
    }
    |> Request.send
    |> Response.deserializeJson<TodoistTask list>

let moveBelowParent parentId childIds =
    let commands =
        childIds
        |> List.collect (fun childId ->
            [
              { ``type`` = "item_move"
                uuid = (Guid.NewGuid ()).ToString()
                args = { emptyArgs with id = childId; parent_id = Some parentId } }
            ])
    http {
    POST syncApiUrl
    body
    jsonSerialize {| commands = commands |}
    }
    |> Request.sendAsync

let fullSync () =
    async {
        let payload =
            { defaultPayload with
                sync_token = syncToken
                resource_types = ["all"] }

        let! response =
            http {
                POST syncApiUrl
                body
                jsonSerialize payload
            }
            |> Request.sendAsync
        let syncResponse = response.DeserializeJson<SyncResponse>()
        syncToken <- syncResponse.sync_token
        return syncResponse
    }