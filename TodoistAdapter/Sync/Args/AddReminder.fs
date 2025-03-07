module TodoistAdapter.Sync.Args.ReminderAddArgs

type AddReminder =
    { id: string
      ``type``: string
      minute_offset: int
      notify_uid: int option
      item_id: string option }

let defaultAddReminder: AddReminder =
    { id = ""
      ``type`` = "relative"
      minute_offset = 0
      notify_uid = None
      item_id = None }