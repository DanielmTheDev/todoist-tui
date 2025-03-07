module TodoistAdapter.Sync.Commands.DeleteItem

open System
open TodoistAdapter.Sync.Args.Arg
open TodoistAdapter.Types.TodoistTask

let deleteItem (task: Task): Command =
    { ``type`` = "item_delete"
      uuid = (Guid.NewGuid ()).ToString()
      temp_id = None
      args = ItemDelete { id = task.id } }