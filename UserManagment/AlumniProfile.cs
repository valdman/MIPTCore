namespace UserManagment
{
    public class AlumniProfile
    {
        public int? YearOfGraduation { get; set; }
        public FacultyType? Faculty { get; set; }
        public DiplomaType? Diploma { get; set; }
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