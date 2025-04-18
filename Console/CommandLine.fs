module Console.CommandLine

open Argu
open Console.UserInteraction
open FsHttp
open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.Dtos.CreateTaskDto

type Arguments =
    | [<AltCommandLine("-a")>] Add of content: string * due: string
    | [<AltCommandLine("-at")>] AddToday of content: string
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Add _ -> "Add a new task with content and due date"
            | AddToday _ -> "Add a new task with content today"

let rec add ui content due =
    let addSynchronously =
        async {
            let task =
                { defaultCreateTask with
                    content = content
                    due_string = if System.String.IsNullOrEmpty due then Some "tod" else Some due }
            let! created = createTask task
            created |> Response.assert2xx |> ignore
            ui.print "Task added"
            return [created] }
    SpectreCoff.Status.start "Adding" (fun _ -> addSynchronously)

let runWithCommandArgs ui (results: ParseResults<Arguments>) =
    let r =
        match results.GetAllResults() with
        | [Add (content, due)] -> add ui content due
        | [AddToday content] -> add ui content "tod"
        | [] -> async.Return []
        | _ -> failwith "Multiple commands not supported"
    r

