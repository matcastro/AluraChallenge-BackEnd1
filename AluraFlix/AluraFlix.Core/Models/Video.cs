namespace AluraFlix.Core.Models
{
    public class Video
    {
        public long Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Url { get; set; }
        public long CategoriaId { get; set; }
    }
}
