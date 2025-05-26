using System;
using System.Collections.Generic;
using System.Linq;
using CertUtils.Token.KeyFormats;

namespace CertUtils.Token
{
    public class PrivateKeyParserFactory
    {
        private readonly IEnumerable<IPrivateKeyParser> _parsers;

        public PrivateKeyParserFactory()
        {
            _parsers = new List<IPrivateKeyParser>
            {
                new Pkcs1KeyParser(),
                new Pkcs8KeyParser()
            };
        }

        public IPrivateKeyParser GetParser(string privateKey)
        {
            if (string.IsNullOrWhiteSpace(privateKey))
                throw new ArgumentException("Private key cannot be null or empty", nameof(privateKey));

            var parser = _parsers.FirstOrDefault(p => p.CanParse(privateKey));
            
            return parser ?? throw new NotSupportedException(
                "No suitable parser found for the provided private key format. " +
                "Supported formats: PKCS#1, PKCS#8");
        }
    }
}
