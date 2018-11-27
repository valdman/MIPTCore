using System;
using System.Linq;
using System.Text;
using Common;

namespace PaymentGateway
{
    public static class SignatureHelper
    {
        public static  string SignVia(params object[] @params)
        {
            var sense = "" + @params.Aggregate(new StringBuilder(), (builder, s) => builder.Append(s));
            var md5Hash = CryptoHelper.GetMd5HexadecimalHash(sense);
            var md5Bytes = Encoding.UTF8.GetBytes(md5Hash);
            return Convert.ToBase64String(md5Bytes);
        }
    }
}