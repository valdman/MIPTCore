using System;

namespace UserManagment.Exceptions
{
    public class WrongPasswordException : Exception
    {
        public override string Message => "Trying to login with wrong password";
    }
}