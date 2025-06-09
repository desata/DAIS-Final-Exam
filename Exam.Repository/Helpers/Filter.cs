namespace Exam.Repository.Helpers
{
    public class Filter
    {
        public static Filter Empty => new Filter();

        public Dictionary<string, object> Clauses { get; set; } = new Dictionary<string, object>();

        public Filter AddClause(string field, object value)
        {
            Clauses.Add(field, value);
            return this;

        }
    }
}