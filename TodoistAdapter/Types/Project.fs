module TodoistAdapter.Types.TodoistProject

type Project = {
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