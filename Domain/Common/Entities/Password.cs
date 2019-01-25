using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Common.Abstractions;
using Journalist;

namespace Common.Entities
{
    public class Password : AbstractIdentifyable
    {   
        public Password(string pass)
        {
            if (Regex.IsMatch(pass, "^.{8,18}$"))
            {
                Hash = CryptoHelper.GetMd5HexadecimalHash(pass);
            }
            else
            {
                throw new ArgumentException("Password does not satisfy security requirements");
            }
        }
        
        private Password() {}

        public virtual string Hash { get; protected set; }

        public static Password FromPlainString(string value)
        {
            Require.NotEmpty(value, nameof(value));
            
            return new Password {Hash = value};
        }

        public Password GetHashed()
        {
            return new Password(Hash);
        }

        private bool Equals(Password other)
        {
            return string.Equals(Hash, other.Hash);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Password) obj);
        }

        public override int GetHashCode()
        {
            return Hash != null ? Hash.GetHashCode() : 0;
        }

        public static bool IsStringCorrectPassword(string passwordToCheck)
        {
            return !string.IsNullOrEmpty(passwordToCheck)
                   && Regex.IsMatch(passwordToCheck, "^.{8,18}$");
        }
    }
}