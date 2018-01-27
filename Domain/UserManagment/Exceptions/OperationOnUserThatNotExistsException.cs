using Common.DomainSteroids;

namespace UserManagment.Exceptions
{
    public class OperationOnUserThatNotExistsException : DomainException
    {
        public OperationOnUserThatNotExistsException(string operationName)
        {
            FieldName = "DomainError";
            _operationName = operationName;
        }
        
        public override string Message => $"Trying To {_operationName} on user that not exists";
        private readonly string _operationName;
    }
}