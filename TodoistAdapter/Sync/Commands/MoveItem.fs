module TodoistAdapter.Sync.Commands.MoveItem

open System
open TodoistAdapter.Sync.Args.Arg
open TodoistAdapter.Sync.Args.ItemUpdateArgs

let moveItem (parentId: string) childId: Command =
    { ``type`` = "item_move"
      temp_id = None
      uuid = (Guid.NewGuid ()).ToString()
      args = ItemUpdate { defaultUpdateItem with id = childId; parent_id = Some parentId } }