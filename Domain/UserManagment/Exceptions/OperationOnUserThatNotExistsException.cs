using Common;

namespace UserManagment.Exceptions
{
    public class OperationOnUserThatNotExistsException : DomainException
    {
        public OperationOnUserThatNotExistsException(string operationName)
        {
            FieldName = "DomainError";
            _operationName = operationName;
        }
        
        public override string Message => $"Trying to {_operationName} on user that not exists";
        private readonly string _operationName;
    }
}