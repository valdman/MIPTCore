using System;

namespace UserManagment.Exceptions
{
    public class WrongPasswordException : Exception
    {
        public override string Message => "Trying To login with wrong password";
    }
}