namespace WebSharper.NetworkInformation

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let NetworkInformation =
        Class "NetworkInformation"
        |=> Inherits T<Dom.EventTarget>
        |+> Instance [
            "downlink" =? T<float> 
            "downlinkMax" =? T<float> 
            "effectiveType" =? T<string>
            "rtt" =? T<float> 
            "saveData" =? T<bool> 
            "type" =? T<string> 

            "onchange" =@ T<unit> ^-> T<unit>
            |> ObsoleteWithMessage "Use OnChange instead"
            "onchange" =@ T<Dom.Event> ^-> T<unit>
            |> WithSourceName "OnChange"
        ]

    let Assembly =
        Assembly [
            Namespace "WebSharper.NetworkInformation" [
                NetworkInformation
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
