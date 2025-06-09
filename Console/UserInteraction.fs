module Console.UserInteraction

open FsHttp
open Spectre.Console
open SpectreCoff
open TodoistAdapter.Types.TodoistTask

type UserInteraction =
    { ask: string -> string
      askSuggesting: string -> string -> string
      chooseFrom: string list -> string -> string
      chooseGroupedFromWith: GroupedSelectionPromptOptions -> ChoiceGroups<Task> -> string -> Task list
      chooseGroupedFrom: ChoiceGroups<Task> -> string -> Task list
      print: string -> unit
      spinner: string -> Async<Response> -> Async<Response>
      spinnerList: string -> Async<Response list> -> Async<Response list>
      spinnerArray: string -> Async<Response array> -> Async<Response array> }

let spectreCoffUi =
    { ask = ask
      askSuggesting = askSuggesting
      chooseFrom = chooseFrom
      chooseGroupedFromWith = chooseGroupedFromWith
      chooseGroupedFrom = chooseGroupedFrom
      print = printMarkedUp
      spinner = fun text fn ->
          let operation (_: StatusContext) = fn
          (SpectreCoff.Status.start text operation)
      spinnerList = fun text fn ->
          let operation _: Async<Response list> = fn
          SpectreCoff.Status.start text operation
      spinnerArray = fun text fn ->
          let operation _: Async<Response array> = fn
          SpectreCoff.Status.start text operation }

let defaultUserInteraction =
    { ask = fun _ -> failwith "Not implemented"
      askSuggesting = fun _ _ -> failwith "Not implemented"
      chooseFrom = fun _ _ -> failwith "Not implemented"
      chooseGroupedFromWith = fun _ _ _ -> failwith "Not implemented"
      chooseGroupedFrom = fun _ _ -> failwith "Not implemented"
      print = fun _ -> failwith "Not implemented"
      spinner = fun _ _ -> failwith "Not implemented"
      spinnerList = fun _ _ -> failwith "Not implemented"
      spinnerArray = fun _ _ -> failwith "Not implemented" }
