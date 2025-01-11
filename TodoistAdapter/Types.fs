module TodoistAdapter.Types

open System

type SyncCommand = {
    ``type``: string
    uuid: string
    args: obj  // This is a simplification - we can make this more specific later if needed
}

type SyncCommandArgs = {
    id: string
    parent_id: string option
}

type Duration = {
    amount: int
    unit: string
}

type Due = {
    date: DateTime
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

type TodoistProject = {
    id: string
    name: string
    color: string
    parent_id: string option
    order: int
    comment_count: int
    is_shared: bool
    is_favorite: bool
    is_inbox_project: bool
    is_team_inbox: bool
    view_style: string
    url: string
}

type TodoistLabel = {
    id: string
    name: string
    color: string
    order: int
    is_favorite: bool
    is_deleted: bool
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
    sync_token: string
    full_sync: bool
    items: TodoistTask list
    projects: TodoistProject list
    labels: TodoistLabel list
}

type Args =
    { id: string
      parent_id: string option
      due: string option }

let emptyArgs =
    { id = ""
      parent_id = None
      due = None }

type Command =
    { ``type``: string
      uuid: string
      args:  Args } // todo: this should be a DU for each type of command, but can't be serialized into json without extra work

type Payload = {
    sync_token: string
    resource_types: string list
}

let defaultPayload =
    { sync_token = "*"
      resource_types = [] }

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
    is_deleted = false
}

