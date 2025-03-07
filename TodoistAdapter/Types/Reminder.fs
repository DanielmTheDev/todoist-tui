module TodoistAdapter.Types.Reminder

open TodoistAdapter.Types.Due

type Reminder = {
    id: string
    notify_uid: string
    item_id: string
    ``type``: string
    due: Due
    minute_offset: int
    is_deleted: bool
}