using System;

namespace InmobiliariaApp.Models
{
    public class Inmueble
{
    public int IdInmueble { get; set; }
    public int DNIPropietario { get; set; }
    public string Tipo { get; set; } = "";
    public string Direccion { get; set; } = "";
    public int Ambientes { get; set; }
    public double Precio { get; set; }
    public string Estado { get; set; } = "";
    public string Uso { get; set; } = "";
    public string Longitud { get; set; } = "";
    public string Latitud { get; set; } = "";
    public string? Portada { get; set; } = "";
    // agrego campos especialas para la query que trae tambien datos del propietario
    public string NombrePropietario { get; set; } = "";
    public string ApellidoPropietario { get; set; } = "";
}

}