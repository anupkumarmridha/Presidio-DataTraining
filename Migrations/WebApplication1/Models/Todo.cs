namespace WebApplication1.Models
{
    public class Todo
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public DateTime TargetDate { get; set; }
        public bool Status { get; set; }

        // Parameterless constructor
        public Todo()
        {
        }

        // Constructor with all parameters including Id
        public Todo(long id, string title, string username, string description, DateTime targetDate, bool status)
        {
            Id = id;
            Title = title;
            Username = username;
            Description = description;
            TargetDate = targetDate;
            Status = status;
        }

        // Constructor without Id (useful for creating new Todo objects before saving)
        public Todo(string title, string username, string description, DateTime targetDate, bool status)
        {
            Title = title;
            Username = username;
            Description = description;
            TargetDate = targetDate;
            Status = status;
        }

        // Overriding Equals and GetHashCode methods
        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj == null || GetType() != obj.GetType())
                return false;

            Todo other = (Todo)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
