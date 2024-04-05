using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.User
{
    public class UserTDO
    {

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public int Edad { get; set; }
        public int Numero { get; set; }
    }
}
