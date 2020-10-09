using Test.Framework.Logging;
using Task_5.PageObjects;
namespace Task_5.Steps
{
    public class StepsDashboardPage
    {
        public static bool IsOpenDashboardPage()
        {
            DashboardPage dashboardPage = new DashboardPage();
            dashboardPage.Login.GetElement();
            Log.Info(5, "Checking Dashboard Page is opened.");
            if (dashboardPage.Login.IsNull)
                return false;
            else
                return true;
        }
        public static void ClickMainMenuDownload()
        {
            DashboardPage dashboardPage = new DashboardPage();
            dashboardPage.MainMenuForm.Download.GetElement();
            Log.Info(6, "Click \"Download\" button in \"Main menu\".");
            dashboardPage.MainMenuForm.Download.Click();
        }
    }
}
