using System.Threading.Tasks;
using TelegramBotExample;

namespace TelegramBotExample
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await MyTelegramBot.RunBot();
        }
    }
}
