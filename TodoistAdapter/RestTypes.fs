module TodoistAdapter.RestTypes

open System
open System.Text.Json
open System.Text.Json.Serialization

type Duration = {
    amount: int
    unit: string
}

type DueDate =
    | TodoistDateTime of DateTime
    | TodoistDateOnly of DateOnly

type OptionDueDateConverter() =
    inherit JsonConverter<DueDate option>()

    override _.Read(reader, _, _) =
        if reader.TokenType = JsonTokenType.Null then
            None
        else
            let raw = reader.GetString()
            if String.IsNullOrWhiteSpace(raw) then None
            elif raw.Contains("T") then
                Some (DueDate.TodoistDateTime(DateTime.Parse raw))
            else
                Some (DueDate.TodoistDateOnly(DateOnly.Parse raw))

    override _.Write(writer, value, _) =
        match value with
        | None ->
            // You could write null, or skip, depending on your desired JSON
            writer.WriteNullValue()
        | Some (DueDate.TodoistDateTime dt) ->
            writer.WriteStringValue(dt.ToString("yyyy-MM-ddTHH:mm:ss"))
        | Some (DueDate.TodoistDateOnly d) ->
            writer.WriteStringValue(d.ToString("yyyy-MM-dd"))


type Due = {
    [<System.Text.Json.Serialization.JsonConverter(typeof<OptionDueDateConverter>)>]
    date: DueDate option
    timezone: string option
    is_recurring: bool
    string: string
    datetime: DateTime option
}

type TodoistTask = {
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

let emptyTodoistTask =
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
      id = "" }

let emptyLabel = {
    id = "1"
    name = ""
    color = ""
    order = 0
    is_favorite = false
    is_deleted = false
}

let emptyDue = {
    date = None
    timezone = None
    is_recurring = false
    string = ""
    datetime = None
}