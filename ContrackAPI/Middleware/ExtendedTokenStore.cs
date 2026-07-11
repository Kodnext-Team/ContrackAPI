using System;
using System.Collections.Concurrent;

namespace ContrackAPI
{
    public static class ExtendedTokenStore
    {
        private static readonly ConcurrentDictionary<string, DateTime> _extendedTokens = new ConcurrentDictionary<string, DateTime>();

        public static void ExtendToken(string token, DateTime newExpiry)
        {
            _extendedTokens[token] = newExpiry;
        }

        public static bool TryGetExpiry(string token, out DateTime expiry)
        {
            return _extendedTokens.TryGetValue(token, out expiry);
        }
    }
}
