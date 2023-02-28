namespace PocketClient.Desktop.Helpers;

public static class ReadModeHelper
{
    public static string RenderArticle(string articleContent)
    {
        var installedLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
        var commonCss = Path.Combine(installedLocation.Path, "Assets", "ArticleCss", "Common.css");

        return @$"
<html>
<head>
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
<link rel=""stylesheet"" type=""text/css"" href=""file:///{commonCss}"" />
</head>
<body>
{articleContent}
</body>
</html>
";
    }
}
