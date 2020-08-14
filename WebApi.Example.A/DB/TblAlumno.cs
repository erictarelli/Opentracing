using System;
using System.Collections.Generic;

namespace WebApi.Example.A.DB
{
    public partial class TblAlumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Email { get; set; }
    }
}
