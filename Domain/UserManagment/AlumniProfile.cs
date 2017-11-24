using Common;

namespace UserManagment
{
    public class AlumniProfile : AbstractIdentifyable
    {
        public int YearOfGraduation { get; set; }
        public FacultyType Faculty { get; set; }
    }

    public enum FacultyType
    {
        //todo: Real faculty names
        Faculty1,
        Faculty2,
        Faculty3
    }
}