module TodoistAdapter.Sync.Args.ItemUpdateArgs

open TodoistAdapter.Types.Due

type UpdateItem =
    { id: string
      parent_id: string option
      labels: string list option
      priority: int
      due: Due option }

let defaultUpdateItem: UpdateItem =
    { id = ""
      parent_id = None
      labels = None
      priority = 4
      due = None }