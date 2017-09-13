using UserManagment;

namespace MIPTCore.Models
{
    public class AlumniProfileModel
    {
        public int YearOfGraduation { get; set; }
        public FacultyType Faculty { get; set; }
        public DiplomaType Diploma { get; set; }
    }
}