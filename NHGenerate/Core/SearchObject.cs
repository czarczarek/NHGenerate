using System.Collections.Generic;

namespace NHGenerate.Core
{
    public class SearchObject
    {
        public string Name { get; set; }
        public List<Occurence> Occurences;

        #region Equals

        public bool Equals(SearchObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(SearchObject)) return false;
            return Equals((SearchObject)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        #endregion
    }
}
