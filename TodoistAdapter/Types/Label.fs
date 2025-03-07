module TodoistAdapter.Types.TodoistLabel

type Label = {
    id: string
    name: string
    color: string
    order: int
    is_favorite: bool
    is_deleted: bool
}

let defaultLabel = {
    id = "1"
    name = ""
    color = ""
    order = 0
    is_favorite = false
    is_deleted = false
}