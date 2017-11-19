namespace PaymentGateway
{
    public enum PaymentErrorCode
    {
        NoError = 0,
        DuplicateOrderNumber = 1,
        UnnownCurrency = 3,
        RequiredParameterNotProvided = 4,
        IvalidParameterValue = 5,
        SystemError = 7
    }
}