module Tui.Types


type AddTaskModel =
    { Content: string
      Due: string }
type Model =
    { MenuItems: string list
      SelectedItem: string
      AddTask: AddTaskModel}

type Msg =
    | SelectItem of string
    | ExitApp
    | SetAddTaskContent of string
    | SetAddTaskDue of string
    | AddTask
    | TaskCreated