module Console.Mapping

open Console.Types

let toUpdateDto (task: TodoistTask): UpdateTaskDto =
    { content = Some task.content
      id = task.id
      description = task.description
      labels = task.labels
      priority = task.priority
      assignee_id = task.assignee_id
      due_string = task.due |> Option.map _.string
      due_date = task.due |> Option.map _.date.ToString()
      due_datetime = task.due |> Option.bind (fun due -> due.datetime |> Option.map _.ToString())
      due_lang = task.due |> Option.bind _.timezone
      duration = task.duration |> Option.map _.amount
      duration_unit = task.duration |> Option.map _.unit }