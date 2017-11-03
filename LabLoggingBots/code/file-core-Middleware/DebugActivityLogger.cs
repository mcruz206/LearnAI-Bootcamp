namespace MiddlewareBot
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.History;
    using Microsoft.Bot.Connector;
    using System.IO;

    public class DebugActivityLogger : IActivityLogger
    {
        TextWriter tw;
        public DebugActivityLogger(TextWriter inputFile)
        {
            this.tw = inputFile;
        }

        public async Task LogAsync(IActivity activiy)
        {
            Debug.WriteLine($"From:{activiy.From.Id} - To:{activiy.Recipient.Id} - Message:{activiy.AsMessageActivity().Text}");
            tw.WriteLine($"From:{activiy.From.Id} - To:{activiy.Recipient.Id} - Message:{activiy.AsMessageActivity().Text}", true);
            tw.Flush();
        }
    }
}
