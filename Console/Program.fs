module Todoist.Console

open Console
open FSharp.SystemCommandLine
open Spectre.Console
open SpectreCoff
open Tasks

module Commands = 
    let AddTodo = 
        command "addTodo" {
            description "Adds a Todo-item"
            setHandler addTask
        }

    let UpdateTodo = 
        command "updateTodo" {
            description "Updates a Todo-item"
            setHandler manipulateToday
        }

    let Interactive = 
        let repeat = Input.Option<bool>("--repeat", false)

        let doWhile f condition =
            f ()
            while condition do f ()

        let handlerLogic () = 
            AnsiConsole.Clear |> ignore
            let choice = chooseFrom ["Add Task"; "Manipulate Today"] "What do you want to do?"
            match choice with
            | "Add Task" -> addTask ()
            | "Manipulate Today" -> manipulateToday ()
            | _ -> failwith "Choice does not exist"

        let handler ctx = 
            doWhile handlerLogic (repeat.GetValue ctx)
            ()

        command "interactive" {
            description "Walks you through the choices"
            inputs (Input.Context())
            setHandler handler
        }
    
    let Root argv = 
        let noSubcommnadHandler () = 
            C "Please specify a command!" |> toConsole

        rootCommand argv {
            description "Todoist root"
            setHandler noSubcommnadHandler
            addCommand Interactive
            addCommand AddTodo
            addCommand UpdateTodo
        }

[<EntryPoint>]
let main argv = 
    LocalState.init ()
    Commands.Root argv    
