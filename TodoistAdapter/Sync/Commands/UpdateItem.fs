module TodoistAdapter.Sync.Commands.UpdateItem

open System
open TodoistAdapter.Sync.Args.Arg
open TodoistAdapter.Sync.Args.ItemUpdateArgs
open TodoistAdapter.Sync.Commands.AddReminder
open TodoistAdapter.Types.TodoistTask

let itemUpdateCommand (task: Task) userId =
    let updateCommand =
        { ``type`` = "item_update"
          uuid = (Guid.NewGuid ()).ToString()
          temp_id = None
          args =
            ItemUpdate
                { defaultUpdateItem with
                   id = task.id
                   priority = Option.defaultValue 4 task.priority
                   labels = task.labels
                   due = task.due |> Option.map
                       (fun due ->
                           { date = due.date
                             is_recurring = due.is_recurring
                             string = Option.defaultValue "" (task.due |> Option.map _.string)
                             datetime = None
                             timezone = None } ) } }
    match task.due with
    | Some _ -> [updateCommand; reminderCommand task.id userId]
    | None -> [updateCommand]