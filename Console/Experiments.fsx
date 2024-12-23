#r "bin/Debug/net9.0/Console.dll"
#r "nuget: EluciusFTW.SpectreCoff, 0.49.4"
#r "nuget: FsHttp"
#r "nuget: FSharp.Data"
open System
open System.IO
open System.Text.RegularExpressions
open TodoistAdapter.Communication
open Console.ConsoleQueries
open TodoistAdapter.Types
open Console.Mapping
open TodoistAdapter.Types
open FsHttp
open Spectre.Console
open SpectreCoff

let s = "test" |> markupString (Some Color.Red) []



init()

let createTasks count =
    List.init count (fun i -> { emptyCreateTaskDto with content = $"Check something {i}"; due_string = Some "today"; priority = Some 4 })
    |> List.map createTask
    |> Async.Parallel
    |> Async.RunSynchronously

// createTasks 4
//
// type args =
//     { id: string
//       parent_id: string option
//       due: string option }
//
// let emptyArgs =
//     { id = ""
//       parent_id = None
//       due = None }
//
// type command =
//     { ``type``: string
//       uuid: string
//       args:  args } // todo: this should be a DU for each type of command, but can't be serialized into json without extra work
//
// let todayTasks = getTodayTasks ()
//
// let first = List.head todayTasks
// let others = List.tail todayTasks
//
// let commands =
//     others
//     |> List.collect (fun task ->
//         [
//           { ``type`` = "item_move"
//             uuid = task.id
//             args = { emptyArgs with id = task.id; parent_id = Some first.id } }
//           { ``type`` = "item_update"
//             uuid = task.id
//             args = { emptyArgs with id = task.id; due = Some "no date" } }
//         ])
//
// http {
//     POST "https://api.todoist.com/sync/v9/sync"
//     body
//     jsonSerialize {| commands = commands |}
// }
// |> Request.send
//
// let updateDtos =
//     others
//     |> List.map toUpdateDto
//     |> List.map (fun t -> { t with due_date = None; due_string = Some "no date"; due_datetime = None })
//     |> List.map updateTask
//     |> Async.Parallel
//     |> Async.RunSynchronously
//
//
