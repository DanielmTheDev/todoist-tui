module TodoistAdapter.Types

open System

type Duration = {
    amount: int
    unit: string
}

type Due = {
    date: DateOnly
    is_recurring: bool
    string: string
    datetime: DateTime option
    timezone: string option
}

type TodoistTask = {
    content: string
    description: string option
    project_id: string option
    section_id: string option
    parent_id: string option
    order: int option
    labels: string array option
    priority: int option
    assignee_id: string option
    duration: Duration option
    due: Due option
    id: string
}

type CreateTaskDto = {
    content: string
    description: string option
    project_id: string option
    section_id: string option
    parent_id: string option
    order: int option
    labels: string array option
    priority: int option
    due_string: string option
    due_date: string option
    due_datetime: string option
    due_lang: string option
    assignee_id: string option
    duration: int option
    duration_unit: string option
}

type Label = {
    id: string
    name: string
    color: string
    order: int
    is_favorite: bool
}

type UpdateTaskDto = {
    id: string
    content: string option
    description: string option
    labels: string array option
    priority: int option
    due_string: string option
    due_date: string option
    due_datetime: string option
    due_lang: string option
    assignee_id: string option
    duration: int option
    duration_unit: string option
}

type SyncResponse = {
    full_sync: bool
    sync_token: string
    labels: Label list option
}

type args =
    { id: string
      parent_id: string option
      due: string option }

let emptyArgs =
    { id = ""
      parent_id = None
      due = None }

type command =
    { ``type``: string
      uuid: string
      args:  args } // todo: this should be a DU for each type of command, but can't be serialized into json without extra work

let emptyUpdateTaskDto =
    { content = None
      id = ""
      description = None
      labels = None
      priority = None
      due_string = None
      due_date = None
      due_datetime = None
      due_lang = None
      assignee_id = None
      duration = None
      duration_unit = None }

let emptyCreateTaskDto = {
    content = ""
    description = None
    project_id = None
    section_id = None
    parent_id = None
    order = None
    labels = None
    priority = None
    due_string = None
    due_date = None
    due_datetime = None
    due_lang = None
    assignee_id = None
    duration = None
    duration_unit = None
}

let emptyLabel = {
    id = "1"
    name = ""
    color = ""
    order = 0
    is_favorite = false
}
