module Console.Communication

open System.Text.Json
open System.Text.Json.Serialization
open Console.Types
open FsHttp

let init () =
    GlobalConfig.defaults
    |> Config.useBaseUrl "https://api.todoist.com/rest/v2/"
    |> Config.transformHeader (fun header ->
        { header with headers = header.headers.Add("Authorization", "your-api-key-here") })
    |> GlobalConfig.set

    GlobalConfig.Json.defaultJsonSerializerOptions <-
        let options = JsonSerializerOptions()
        options.Converters.Add(JsonFSharpConverter())
        options

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
    |> Request.send
    |> ignore

let createTask (payload: CreateTaskDto) =
    http {
        POST "tasks"
        body
        jsonSerialize payload
    }
    |> Request.send
    |> ignore

let getTodayTasks () =
    http {
        GET "tasks"
        query [ "filter", "due today" ]
    }
    |> Request.send
    |> fun response -> response.DeserializeJson<TodoistTask list> ()

let getAheadTasks (daysAhead: int) =
    http {
        GET "tasks"
        query [ "filter", $"{daysAhead} days" ]
    }
    |> Request.send
    |> fun response -> response.DeserializeJson<TodoistTask list> ()
