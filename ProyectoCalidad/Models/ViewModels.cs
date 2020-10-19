using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoCalidad.Models
{
    public class ViewModels
    {
    }

    public class AirportsForDropdown
    {
        public string airportCode { get; set; }
        public string airportName { get; set; }
    }

    public class DaysForDropdown
    {
        public string weekDay { get; set; }

    }

    public class DatesForDropdown
    {
        public System.DateTime date { get; set; }
        public string dateToShow { get; set; }


    }

    public class FlightsForQueries
    {
        public int flightId { get; set; }
        public int routeCode { get; set; }
        public string airportCode { get; set; }
        public string flightCode { get; set; }
        public System.DateTime date { get; set; }
        public string dateToShow { get; set; }
        public int realPaasengers { get; set; }
        public string arrivalDeparture { get; set; }
        public string weekDay { get; set; }
        public string airportName { get; set; }



    }
}