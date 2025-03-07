module TodoistAdapter.Sync.Commands.AddReminder

open System
open TodoistAdapter.Sync.Args.Arg
open TodoistAdapter.Sync.Args.ReminderAddArgs

let reminderCommand (id: string) userId =
    let tempId = (Guid.NewGuid ()).ToString()
    { ``type`` = "reminder_add"
      uuid = (Guid.NewGuid ()).ToString()
      temp_id = Some tempId
      args =
        ReminderAdd
            { defaultAddReminder with
                  id = tempId
                  minute_offset = 0
                  notify_uid = Some userId
                  item_id = Some id } }