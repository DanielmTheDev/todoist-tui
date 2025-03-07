module TodoistAdapter.Types.TodoistFilter

type Filter = {
    id: string
    name: string
    query: string
    color: string
    item_order: int
    is_deleted: bool
    is_favorite: bool
}