module Tui.TreeView

open TodoistAdapter.Types
open Tui.Types

let createNodes (tasks: TodoistTask list) =
    tasks
    |> List.groupBy (fun task ->
        match task.labels with
        | Some labels -> String.concat ", " (Array.sort labels)
        | None -> "No labels")
    |> List.map (fun (labelGroup, tasksInGroup) ->
        let parentNode = TodoistTreeNode(labelGroup)

        tasksInGroup
        |> List.iter (fun task ->
            let contentWithoutEmojis = task.content
            let childNode = TodoistTreeNode(contentWithoutEmojis)
            childNode.Id <- Some task.id
            parentNode.Children.Add(childNode))

        parentNode)