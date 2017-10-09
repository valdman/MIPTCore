namespace Common
{
    public interface IDomainOptionsService
    {
        DomainOptions GetDomainOptions();
        void UpdateDomainOption(DomainOptions newOptions);
    }
}