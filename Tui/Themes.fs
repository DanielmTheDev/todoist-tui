module Tui.Themes

open Terminal.Gui
open Terminal.Gui.Elmish

let darkTheme =
    let deepBlue = Color(red = 10, green = 20, blue = 60)
    let skyBlue = Color(red = 70, green = 130, blue = 180)
    let darkGray = Color(red = 30, green = 30, blue = 30)
    let gray = Color(red = 80, green = 80, blue = 80)
    let white = Color.White

    let normal = Attribute(&deepBlue, &darkGray)
    let focused = Attribute(&skyBlue, &darkGray)
    let hotNormal = Attribute(&gray, &darkGray)
    let disabled = Attribute(&gray, &deepBlue)
    let hotFocus = Attribute(&white, &skyBlue)

    prop.colorScheme (ColorScheme(normal, focused, hotNormal, disabled, hotFocus))