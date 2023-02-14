namespace PocketClient.Desktop.Helpers;

public static class ReadModeHelper
{
    public static string RenderArticle(string articleContent)
    {
        return @$"
<html>
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<style>
body {{
    display: flex;
    justify-content: center;
    font-size: 16px;
}}

.page {{
    padding: 12px 20px;
    max-width: 1024px;
    margin-left: 0;
    margin-right: 0;
}}

label {{
    display: inline-block;
    margin-bottom: .5rem;
}}

article,aside,figcaption,figure,footer,header,hgroup,main,nav,section {{
    display: block;
}}

p {{
    margin-bottom: 1rem;
    margin-top: 0;
}}

p+h2, p+h3, p+h4, p+h5, p+h6 {{
    margin-top: 1.7em;
}}

img {{
    max-width: 100%;
    height: auto;
}}

h1,h2,h3,h4,h5,h6 {{
    margin-bottom: .5rem;
    margin-top: 0;
}}

h1 {{
    font-size: 3rem;
    line-height: 125%;
    margin: 0 0 2.5rem;
}}

h2 {{
    font-size: 2.5rem;
}}

h2,h3 {{
    line-height: 120%;
    margin: 0 0 1.5rem;
}}

h3 {{
    font-size: 2rem;
}}

h4 {{
    font-size: 1.75rem;
    line-height: 128%;
    margin: 0 0 1.5rem;
}}

h5 {{
    font-size: 1.5rem;
    line-height: 122%;
}}

h5,h6 {{
    margin: 0 0 1rem;
}}

h6 {{
    line-height: 126%;
}}

.h6,h6,ol,p,ul {{
    font-size: 1.25rem;
}}

ol,p,ul {{
    margin: 0 0 1.5rem;
}}

ol,ul {{
    padding: 0 0 0 1.5em;
}}

pre {{
    margin-bottom: 1rem;
    margin-top: 0;
    overflow: auto;
}}

code,kbd,pre,samp {{
    font-family: Monaco,Consolas,Liberation Mono,Courier New,monospace;
    font-size: 1em;
}}

figure {{
    margin: 0 0 1rem;
}}

img {{
    border-style: none;
}}

img,svg {{
    vertical-align: middle;
}}

svg {{
    overflow: hidden;
}}

@media screen and (max-width: 1024px) {{
    .page {{
        width: 100%; /* The width is 100%, when the viewport is 800px or smaller */
    }}
}}
</style>
<body>
{articleContent}
</body>
</html>
";
    }
}
