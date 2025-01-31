module TodoistAdapter.CommunicationRestApi

open TodoistAdapter.RestTypes
open FsHttp

let restApiUrl = "https://api.todoist.com/rest/v2"

let requestLabels () =
    http {
        GET $"{restApiUrl}/labels"
    }
    |> Request.send
    |> fun response -> response.DeserializeJson<TodoistLabel list> ()
    |> List.map _.name

// todo: All methods here should return Async<RealType>, not response
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

let getTask id =
    http {
        GET $"{restApiUrl}/tasks/{id}"
    }
    |> Request.sendAsync
    |> fun asyncResponse ->
        async {
            let! response = asyncResponse
            return response.DeserializeJson<TodoistTask> ()
        }

let getTaskByContent id =
    http {
        GET $"{restApiUrl}/tasks/{id}"
    }
    |> Request.sendAsync
    |> fun asyncResponse ->
        async {
            let! response = asyncResponse
            return response.DeserializeJson<TodoistTask> ()
        }

let getAheadTasks (daysAhead: int) =
    http {
        GET $"{restApiUrl}/tasks"
        query [ "filter", $"{daysAhead} days" ]
    }
    |> Request.send
    |> Response.deserializeJson<TodoistTask list>

