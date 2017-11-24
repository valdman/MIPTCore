using Common;

namespace UserManagment
{
    public class AlumniProfile : AbstractIdentifyable
    {
        public int YearOfGraduation { get; set; }
        public string Faculty { get; set; }
    }
}