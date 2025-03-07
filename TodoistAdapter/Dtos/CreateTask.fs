module TodoistAdapter.Dtos.CreateTaskDto

type CreateTask = {
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

let defaultCreateTask = {
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