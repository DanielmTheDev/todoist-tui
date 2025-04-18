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
      // todo: maybe I need specific methods for the other types (state etc). in the end, I should not use the underlying library anymore anywhere
      spinner: string -> Async<Response> -> Async<Response>
      spinnerMany: string -> Async<Response list> -> Async<Response list> }

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
      spinnerMany = fun text fn ->
          let operation _: Async<Response list> = fn
          SpectreCoff.Status.start text operation }

let defaultUserInteraction =
    { ask = fun _ -> failwith "Not implemented"
      askSuggesting = fun _ _ -> failwith "Not implemented"
      chooseFrom = fun _ _ -> failwith "Not implemented"
      chooseGroupedFromWith = fun _ _ _ -> failwith "Not implemented"
      chooseGroupedFrom = fun _ _ -> failwith "Not implemented"
      print = fun _ -> failwith "Not implemented"
      spinner = fun _ _ -> failwith "Not implemented"
      spinnerMany = fun _ _ -> failwith "Not implemented" }
