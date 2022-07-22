using Microsoft.VisualStudio.Imaging;

namespace Intcrementor
{
    internal static class ErrorHandler
    {
        public static async Task ShowErrorNotificationAsync(Exception ex)
        {
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            InfoBar infobar = await VS.InfoBar.CreateAsync(new InfoBarModel(new[] { new InfoBarTextSpan($"An error occurred with {appName}. See output window for more information.") }, KnownMonikers.StatusError, true));
            await infobar.TryShowInfoBarUIAsync();
            OutputWindowPane pane = await VS.Windows.CreateOutputWindowPaneAsync(appName);
            await pane.WriteLineAsync(ex.Message);
            await pane.WriteLineAsync(ex.StackTrace);
        }
    }
}
