module Console.UserInteraction

open SpectreCoff
open TodoistAdapter.Types.TodoistTask

type UserInteraction =
    { ask: string -> string
      askSuggesting: string -> string -> string
      chooseFrom: string list -> string -> string
      chooseGroupedFromWith: GroupedSelectionPromptOptions -> ChoiceGroups<Task> -> string -> Task list
      chooseGroupedFrom: ChoiceGroups<Task> -> string -> Task list }

let spectreCoffUi =
    { ask = ask
      askSuggesting = askSuggesting
      chooseFrom = chooseFrom
      chooseGroupedFromWith = chooseGroupedFromWith
      chooseGroupedFrom = chooseGroupedFrom }

let defaultUserInteraction =
    { ask = fun _ -> failwith "Not implemented"
      askSuggesting = fun _ _ -> failwith "Not implemented"
      chooseFrom = fun _ _ -> failwith "Not implemented"
      chooseGroupedFromWith = fun _ _ _ -> failwith "Not implemented"
      chooseGroupedFrom = fun _ _ -> failwith "Not implemented" }
