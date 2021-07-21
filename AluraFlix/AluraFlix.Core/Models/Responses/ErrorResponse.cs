namespace AluraFlix.Core.Models.Responses
{
    public class ErrorResponse
    {
        public ErrorEnum Code { get; set; }
        public string Description { get; set; }
    }
}
