namespace AluraFlix.Core.Models.Requests
{
    public class VideoRequest
    {
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public long CategoriaId { get; set; }
    }
}
