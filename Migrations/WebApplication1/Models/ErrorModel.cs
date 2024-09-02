namespace WebApplication1.Models
{
    internal class ErrorModel
    {
        private int v;
        private string message;

        public ErrorModel(int v, string message)
        {
            this.v = v;
            this.message = message;
        }
    }
}