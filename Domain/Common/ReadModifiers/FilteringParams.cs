using Journalist.Extensions;

namespace Common.ReadModifiers
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
                    return $"{FilterField}.Date = {EqualTo}.Date";

                return $"{FilterField}.Date >= {From}.Date && " +
                       $"{FilterField}.Date <= {To}.Date";
            }

            if (EqualTo.IsNotNullOrEmpty())
                return $"{FilterField} = {EqualTo}";

            return $"{FilterField} >= {From} && {FilterField} <= {To}";
        }
    }
}