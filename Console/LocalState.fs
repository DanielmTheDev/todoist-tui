module Console.LocalState

open TodoistAdapter.CommunicationRestApi
open TodoistAdapter.CommunicationSyncApi

let mutable labels: string list = []

let init () = // todo: this and the other init can be done with a "do" statement at start of module
    init ()
    labels <- [""]@requestLabels ()