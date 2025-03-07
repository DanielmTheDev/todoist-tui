module TodoistAdapter.Types.State

open TodoistAdapter.Types.Reminder
open TodoistAdapter.Types.TodoistFilter
open TodoistAdapter.Types.TodoistLabel
open TodoistAdapter.Types.TodoistProject
open TodoistAdapter.Types.TodoistTask
open TodoistAdapter.Types.User

type State = {
    sync_token: string
    full_sync: bool
    items: Task list
    projects: Project list
    labels: Label list
    reminders: Reminder list
    filters: Filter list
    user: User
}

let labelNames state =
    state.labels |> List.map _.name

let todaysItems state =
    state.items |> List.filter (fun t -> t |> isDueToday)

let itemWithId id state =
    state.items |> List.find (fun t -> t.id = id)

let itemsWithIds ids state =
    state.items |> List.filter (fun t -> ids |> List.contains t.id)

let defaultState = {
    sync_token = ""
    full_sync = false
    items = []
    projects = []
    labels = []
    filters = []
    reminders = []
    user = { id = 0 }
}