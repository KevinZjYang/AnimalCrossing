using System;
using System.Threading.Tasks;
using Windows.System;

namespace AnimalCrossing.Helpers
{
    public static class EmailHelper
    {
        public static async Task UniversallyEmail(string email, string subject, string messageBody)
        {
            messageBody = Uri.EscapeDataString(messageBody);
            string url = $"mailto:{email}?subject={subject}&body={messageBody}";
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
