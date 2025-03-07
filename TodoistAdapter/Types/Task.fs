module TodoistAdapter.Types.TodoistTask

open System
open TodoistAdapter.Types.Due
open TodoistAdapter.Types.DueDate
open TodoistAdapter.Types.Duration

type Task = {
    id: string
    content: string
    description: string option
    project_id: string option
    section_id: string option
    parent_id: string option
    order: int option
    labels: string list option
    priority: int option
    assignee_id: string option
    duration: Duration option
    due: Due option
    ``checked``: bool
    is_deleted: bool
}

let defaultTask =
    { content = ""
      description = None
      project_id = None
      section_id = None
      parent_id = None
      order = None
      labels = None
      priority = None
      assignee_id = None
      duration = None
      due = None
      id = ""
      ``checked`` = false
      is_deleted = false }

let isDueToday (task: Task) =
    not task.``checked`` &&
    task.due
    |> Option.bind _.date
    |> Option.map dateOnlyOf
    |> Option.exists (fun d -> d = DateOnly.FromDateTime DateTime.Today)
