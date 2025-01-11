module Tui.Types


type AddTaskModel =
    { Content: string
      Due: string
      Label: string }

type Model =
    { MenuItems: string list
      SelectedItem: string
      AddTask: AddTaskModel
      Tasks: TodoistAdapter.Types.TodoistTask list
      Projects: TodoistAdapter.Types.TodoistProject list
      Labels: TodoistAdapter.Types.TodoistLabel list }

type Msg =
    | SelectItem of string
    | ExitApp
    | SetAddTaskContent of string
    | SetAddTaskDue of string
    | AddTask
    | TaskCreated
    | FullSyncCompleted of TodoistAdapter.Types.SyncResponse
    | SetAddTaskLabel of string

type TodoistTreeNode(text: string) =
    inherit Terminal.Gui.TreeNode(text)

    member val Id: string option = None with get, set