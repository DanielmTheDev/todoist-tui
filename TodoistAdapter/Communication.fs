module TodoistAdapter.Communication

open System
open TodoistAdapter.Types
open FsHttp

type Payload = {
    sync_token: string
    resource_types: string list
}

let defaultPayload =
    { sync_token = "*"
      resource_types = [] }

let restApiUrl = "https://api.todoist.com/rest/v2"
let syncApiUrl = "https://api.todoist.com/sync/v9/sync"

let init ()=
    GlobalConfig.defaults
    |> Config.transformHeader (fun header ->
        { header with headers = header.headers.Add("Authorization", "your-api-key-here") })
    |> GlobalConfig.set

let requestLabels () =
    http {
        GET $"{restApiUrl}/labels"
    }
    |> Request.send
    |> fun response -> response.DeserializeJson<Label list> ()
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
    |> Request.send
    |> Response.deserializeJson<TodoistTask list>

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
    POST "https://api.todoist.com/sync/v9/sync"
    body
    jsonSerialize {| commands = commands |}
    }
    |> Request.sendAsync