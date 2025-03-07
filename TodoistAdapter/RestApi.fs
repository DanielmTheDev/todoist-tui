module TodoistAdapter.CommunicationRestApi

open FsHttp
open TodoistAdapter.Dtos.CreateTaskDto
open TodoistAdapter.Types.TodoistLabel
open TodoistAdapter.Types.TodoistTask

let restApiUrl = "https://api.todoist.com/rest/v2"

let requestLabels () =
    http {
        GET $"{restApiUrl}/labels"
    }
    |> Request.send
    |> fun response -> response.DeserializeJson<Label list> ()
    |> List.map _.name

// todo: All methods here should return Async<RealType>, not response
let createTask (payload: CreateTask) =
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
            return response.DeserializeJson<Task list> ()
        }

let getTask id =
    http {
        GET $"{restApiUrl}/tasks/{id}"
    }
    |> Request.sendAsync
    |> fun asyncResponse ->
        async {
            let! response = asyncResponse
            return response.DeserializeJson<Task> ()
        }

let getTaskByContent id =
    http {
        GET $"{restApiUrl}/tasks/{id}"
    }
    |> Request.sendAsync
    |> fun asyncResponse ->
        async {
            let! response = asyncResponse
            return response.DeserializeJson<Task> ()
        }

let getAheadTasks (daysAhead: int) =
    http {
        GET $"{restApiUrl}/tasks"
        query [ "filter", $"{daysAhead} days" ]
    }
    |> Request.send
    |> Response.deserializeJson<Task list>

