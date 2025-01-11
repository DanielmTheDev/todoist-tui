module Tui.View

open System
open Terminal.Gui
open Terminal.Gui.Elmish
open Tui.Themes
open Tui.TreeView
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
                // darkTheme
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
                                prop.position.x.absolute 0
                                prop.position.y.absolute 5
                                prop.width.percent 20
                                prop.accept (fun _ -> dispatch AddTask)
                            ]
                            View.label [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 7
                                label.text "Label"
                            ]
                            View.comboBox [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 8
                                prop.canFocus true
                                prop.tabStop (Some TabBehavior.TabStop)
                                prop.width.fill 0
                                prop.height.absolute 10
                                comboBox.source ("No Label" :: (model.Labels |> List.map _.name))
                                comboBox.selectedItemChanged (fun args -> dispatch (SetAddTaskLabel (args.Value.ToString())))
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

                        prop.title "_Today's Tasks"
                        prop.children [
                            View.treeView<ITreeNode> [
                                prop.canFocus true
                                prop.position.x.absolute 0
                                prop.position.y.absolute 0
                                prop.width.fill 0
                                prop.height.fill 0
                                treeView<ITreeNode>.multiSelect true
                                treeView.treeBuilder (DelegateTreeBuilder<ITreeNode>(_.Children))
                                prop.ref (fun view ->
                                    let treeView = view :?> TreeView<ITreeNode>
                                    treeView.ClearObjects ()
                                    let nodes = createNodes model.Tasks
                                    match nodes with
                                    | [] -> ()
                                    | _ -> nodes |> List.iter treeView.AddObject)
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
                            View.label [
                                prop.position.x.absolute 1
                                prop.position.y.absolute 7
                                label.text ("❤ Home".Normalize())
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]
