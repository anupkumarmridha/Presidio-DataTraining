namespace WebApplication1.Models.DTOs
{
    public class AddTodoDTO
    {
        public string Title { get; set; }
        public int userid { get; set; }
        public string Description { get; set; }
        public DateTime TargetDate { get; set; }
        public bool Status { get; set; }
    }
}
