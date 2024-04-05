using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class logs
    {
        [Key]
        public int id_logs { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje_ { get; set; }
        public string ip { get; set; }
        public string nombrefuncion { get; set; }
        public string Status { get; set; }
        public string Datos { get; set; }

    }
}
