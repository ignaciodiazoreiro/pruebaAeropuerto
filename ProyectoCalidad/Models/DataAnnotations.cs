using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoCalidad.Models
{
    public class Airport
    {
        [Display(Name = "Código de aeropuerto")]
        public string codigoAeropuertoPK { get; set; }

        [Display(Name = "Nombre de aeropuerto")]
        public string nombreAeropuerto { get; set; }

        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        [Display(Name = "Habilitado")]
        public string habilitado { get; set; }
    }

    public class Flights
    {
        [Display(Name = "Código de aeropuerto")]
        public string codigoAeropuertoFK { get; set; }

        [Display(Name = "Código del vuelo")]
        public string codigoVuelo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        public System.DateTime fecha { get; set; }

        [Display(Name = "Cantidad real de pasajeros")]
        public int cantidadRealPasajeros { get; set; }
    }

    public class Routes
    {
        [Display(Name = "LLegada/Salida")]
        public string arrivalDeparture { get; set; }
    }

    [MetadataType(typeof(Flights))]
    public partial class Vuelo
    {
    }

    [MetadataType(typeof(Airport))]
    public partial class Aeropuerto
    {
    }

    [MetadataType(typeof(Routes))]
    public partial class Ruta
    {
    }


}