using Common;

namespace UserManagment
{
    public class AlumniProfile
    {
        public int Id { get; set; }

        public int YearOfGraduation { get; set; }
        public FacultyType Faculty { get; set; }
        public DiplomaType Diploma { get; set; }

        public User User { get; set; }
    }

    public enum DiplomaType
    {
        Bachelor,
        Master,
        EngeenerResarcher,
        Postgraduate
    }

    public enum FacultyType
    {
        //todo: Real faculty names
        Faculty1,
        Faculty2,
        Faculty3
    }
}