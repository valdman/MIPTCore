namespace Common.Infrastructure
{
    public interface IDomainOptionsRepository
    {
        DomainOptions GetDomainOptions();
        void UpdateDomainOptions(DomainOptions newOptions);
    }
}