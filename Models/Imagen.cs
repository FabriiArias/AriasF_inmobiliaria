namespace InmobiliariaApp.Models{
    public class Imagen
    {
        public int IdImagen { get; set; }
        public int IdInmueble { get; set; }
        public string Url { get; set; } = string.Empty;
        public IFormFile? Archivo { get; set; } = null;
        
    }

}