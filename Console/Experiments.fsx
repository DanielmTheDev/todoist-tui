#r "bin/Debug/net9.0/Console.dll"
#r "bin/Debug/net9.0/TodoistAdapter.dll"
#r "nuget: EluciusFTW.SpectreCoff, 0.49.4"
#r "nuget: FsHttp"
open System
open TodoistAdapter
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.RestTypes
open FsHttp

let createTasks count =
    List.init count (fun i -> { emptyCreateTaskDto with content = $"Check something {i}"; due_string = Some "today 11:00"; priority = Some 4 })
    |> List.map createTask
    |> Async.Parallel
    |> Async.RunSynchronously

let createAndUpdateOneTask () =
    createTasks 1
    |> Array.head
    |> Response.deserializeJson<TodoistTask>
    |> fun t ->
        let now = TimeOnly.FromDateTime(DateTime.Now)
        { t with due = Some { t.due.Value with string = $"today {now.AddMinutes 2}" } }
    |> fun t -> CommunicationSyncApi.updateTask [t]
    |> Async.RunSynchronously

createAndUpdateOneTask ()