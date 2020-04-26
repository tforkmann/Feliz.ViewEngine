module Tests.ViewEngine

open Feliz.ViewEngine
open Swensen.Unquote
open Xunit

[<Fact>]
let ``Simple text element is Ok``() =
    // Arrange / Act
    let result =
        Html.text "test"
        |> Render.htmlView

    // Assert
    test <@ result = "test" @>

[<Fact>]
let ``Simple text element is escaped Ok``() =
    // Arrange / Act
    let result =
        Html.text "te<st"
        |> Render.htmlView

    // Assert
    test <@ result = "te&lt;st" @>

[<Fact>]
let ``p element with text is Ok``() =
    // Arrange / Act
    let result =
        Html.p "test"
        |> Render.htmlView

    // Assert
    test <@ result = "<p>test</p>" @>

[<Fact>]
let ``p element with text is escaped Ok``() =
    // Arrange / Act
    let result =
        Html.p "te>st"
        |> Render.htmlView

    // Assert
    test <@ result = "<p>te&gt;st</p>" @>

[<Fact>]
let ``p element with text property is Ok``() =
    // Arrange / Act
    let result =
        Html.p [
            prop.text "test"
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<p>test</p>" @>

[<Fact>]
let ``p element with text property is escaped Ok``() =
    // Arrange / Act
    let result =
        Html.p [
            prop.text "tes&t"
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<p>tes&amp;t</p>" @>

[<Fact>]
let ``p element with text element is Ok``() =
    // Arrange / Act
    let result =
        Html.p [
            Html.text "test"
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<p>test</p>" @>

[<Fact>]
let ``p element with text element is escaped Ok``() =
    // Arrange / Act
    let result =
        Html.p [
            Html.text "t\"est"
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<p>t&quot;est</p>" @>

[<Fact>]
let ``Closed element Ok``() =
    // Arrange / Act
    let result =
        Html.div [
            Html.br []
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<div><br></div>" @>

[<Fact>]
let ``p element with text element and class property is Ok``() =
    // Arrange / Act
    let result =
        Html.p [
            prop.className "main"
            prop.children [
                Html.text "test"
            ]
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<p class=\"main\">test</p>" @>

[<Fact>]
let ``p element with text element and classes property is Ok``() =
    // Arrange / Act
    let result =
        Html.p [
            prop.classes ["c1"; "c2"]
            prop.children [
                Html.text "test"
            ]
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<p class=\"c1 c2\">test</p>" @>


[<Fact>]
let ``h1 element with text and style property is Ok``() =
    // Arrange / Act
    let result =
        Html.h1 [
            prop.style [ style.fontSize(100); style.color("#137373") ]
            prop.text "examples"
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<h1 style=\"font-size:100px;color:#137373\">examples</h1>" @>

[<Fact>]
let ``The order of properties for an element is preserved``() =
    // Arrange / Act
    let result =
        Html.link [
            prop.rel.stylesheet
            prop.type' "text/css"
            prop.href "main.css"
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<link rel=\"stylesheet\" type=\"text/css\" href=\"main.css\">" @>

[<Fact>]
let ``h1 element with text and style property with css unit is Ok``() =
    // Arrange / Act
    let result =
        Html.h1 [
            prop.style [ style.fontSize(length.em(100)) ]
            prop.text "examples"
        ]
        |> Render.htmlView

    // Assert
    test <@ result = "<h1 style=\"font-size:100em\">examples</h1>" @>

[<Fact>]
let ``Void tag in XML should be self closing tag`` () =
    let unary =  Html.br [] |> Render.xmlView
    Assert.Equal("<br />", unary)

[<Fact>]
let ``Void tag in HTML should be unary tag`` () =
    let unary =  Html.br [] |> Render.htmlView
    Assert.Equal("<br>", unary)

[<Fact>]
let ``Nested content should render correctly`` () =
    let nested =
        Html.div [
            Html.comment "this is a test"
            Html.h1 [ Html.text "Header" ]
            Html.p [
                Html.rawText "<br/>"
                Html.strong [ Html.text "Ipsum" ]
                Html.text " dollar"
            ]
        ]
    let html =
        nested
        |> Render.xmlView
    Assert.Equal("<div><!-- this is a test --><h1>Header</h1><p><br/><strong>Ipsum</strong> dollar</p></div>", html)