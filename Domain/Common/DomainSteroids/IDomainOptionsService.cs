namespace Common.DomainSteroids
{
    public interface IDomainOptionsService
    {
        DomainOptions GetDomainOptions();
        void UpdateDomainOption(DomainOptions newOptions);
    }
}