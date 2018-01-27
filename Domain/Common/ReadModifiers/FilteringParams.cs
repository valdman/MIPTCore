using Journalist.Extensions;

namespace Common.Entities.Entities.ReadModifiers
{
    public class FilteringParams
    {
        public string FilterField { get; set; }

        public string EqualTo { get; set; }
        
        public string From { get; set; }
        public string To { get; set; }

        public bool IsEmpty() => FilterField.IsNullOrEmpty();

        public string Linq()
        {
            if (IsEmpty())
                return string.Empty;
            
            //crutches.cs
            if (FilterField.ToLower().Equals("creatingtime"))
            {
                if (EqualTo.IsNotNullOrEmpty())
                    return $"{FilterField}.Year = {EqualTo}.Year && " +
                           $"{FilterField}.Month = {EqualTo}.Month && " +
                           $"{FilterField}.Day = {EqualTo}.Day";

                return $"{FilterField}.Year >= {From}.Year && {FilterField}.Year <= {To}.Year && " +
                       $"{FilterField}.Month >= {From}.Month && {FilterField}.Month <= {To}.Month && " +
                       $"{FilterField}.Day >= {From}.Day && {FilterField}.Day <= {To}.Day";
            }

            if (EqualTo.IsNotNullOrEmpty())
                return $"{FilterField} = {EqualTo}";

            return $"{FilterField} >= {From} && {FilterField} <= {To}";
        }
    }
}