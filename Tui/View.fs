module Tui.View

open Terminal.Gui
open Terminal.Gui.Elmish
open Tui.Themes
open Tui.Types

let view (model: Model) (dispatch: Msg -> unit) =
    View.topLevel [
        prop.children [
            View.menuBar [
                menuBar.menus [
                    MenuBarItem("Application", [|
                        MenuItem ("Item 1", "", fun () -> dispatch (SelectItem "Item 1"))
                        MenuItem ("Item 2", "", fun () -> dispatch (SelectItem "Item 2"))
                        MenuItem ("Quit", "", fun () -> dispatch (SelectItem "Quit"))
                    |])
                ]
            ]
            View.window [
                darkTheme
                prop.position.x.absolute 0
                prop.position.y.absolute 0
                prop.width.fill 0
                prop.height.fill 0
                prop.title "Todoist Tui"
                prop.children [
                    View.frameView [
                        prop.title "_Add Task"
                        prop.tabStop (Some TabBehavior.TabStop)
                        prop.canFocus true
                        prop.position.x.absolute 0
                        prop.position.y.absolute 0
                        prop.width.percent 30
                        prop.height.percent 30

                        prop.children [
                            View.label [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 1
                                label.text "Content"
                            ]
                            View.textField [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 2
                                prop.width.fill 0
                                textField.text model.AddTask.Content
                                textField.textChanging (fun args -> dispatch (SetAddTaskContent args.NewValue))
                            ]
                            View.label [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 3
                                label.text "Due"
                            ]
                            View.textField [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 4
                                prop.width.fill 0
                                textField.text model.AddTask.Due
                                textField.textChanging (fun args -> dispatch (SetAddTaskDue args.NewValue))
                            ]
                            View.button [
                                button.text "Add"
                                prop.position.x.absolute 3
                                prop.position.y.absolute 5
                                prop.width.percent 20
                                prop.accept (fun _ -> dispatch AddTask)
                            ]
                        ]
                    ]
                    View.frameView [
                        prop.tabStop (Some TabBehavior.TabStop)
                        prop.canFocus true
                        prop.position.x.absolute 0
                        prop.position.y.percent 30
                        prop.width.percent 30
                        prop.height.fill 2

                        prop.title "_Dummy"
                        prop.children [
                            View.listView [
                                prop.canFocus true
                                prop.position.x.absolute 0
                                prop.position.y.absolute 0
                                prop.width.fill 0
                                prop.height.fill 0
                                listView.openSelectedItem (fun args -> dispatch (SelectItem (string args.Item)))
                                listView.source ["1"; "2"]
                            ]
                        ]
                    ]
                    View.frameView [
                        prop.tabStop (Some TabBehavior.TabStop)
                        prop.canFocus true
                        prop.position.x.percent 30
                        prop.position.y.absolute 0
                        prop.width.fill 0
                        prop.height.fill 0

                        prop.title "_Content"
                        prop.children [
                            View.button [
                                button.text "_Test"
                                prop.canFocus true
                                prop.position.x.absolute 0
                                prop.position.y.absolute 1
                                prop.width.absolute 30
                                prop.height.fill 0
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]