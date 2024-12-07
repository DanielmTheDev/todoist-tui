module Console.Communication

open Console.Types
open FsHttp

let init () =
    GlobalConfig.defaults
    |> Config.useBaseUrl "https://api.todoist.com/rest/v2/"
    |> Config.transformHeader (fun header ->
        { header with headers = header.headers.Add("Authorization", "your-api-key-here") })
    |> GlobalConfig.set

let requestLabels () =
    http {
        GET "labels"
    }
    |> Request.send
    |> fun response -> response.DeserializeJson<Label list> ()
    |> List.map _.name

let updateTask (payload: UpdateTaskDto) =
    http {
        POST $"tasks/{payload.id}"
        body
        jsonSerialize payload
    }
    |> Request.sendAsync

let createTask (payload: CreateTaskDto) =
    http {
        POST "tasks"
        body
        jsonSerialize payload
    }
    |> Request.sendAsync

let getTodayTasks () =
    http {
        GET "tasks"
        query [ "filter", "due today" ]
    }
    |> Request.send
    |> Response.deserializeJson<TodoistTask list>

let getAheadTasks (daysAhead: int) =
    http {
        GET "tasks"
        query [ "filter", $"{daysAhead} days" ]
    }
    |> Request.send
    |> Response.deserializeJson<TodoistTask list>
