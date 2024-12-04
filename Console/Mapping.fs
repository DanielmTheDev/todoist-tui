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
      due_date =
          match task.due with
          | Some due when due.string <> ""  -> None
          | Some due -> Some (due.date.ToString())
          | None -> None
      due_datetime =
          match task.due with
          | Some due when (due.string <> "" || due.datetime.IsSome) -> None
          | Some due -> due.datetime |> Option.map _.ToString()
          | None -> None
      due_lang = task.due |> Option.bind _.timezone
      duration = task.duration |> Option.map _.amount
      duration_unit = task.duration |> Option.map _.unit }