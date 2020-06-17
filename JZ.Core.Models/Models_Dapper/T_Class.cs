using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.Core.Models
{
    public class T_Class
    {
        public int? F_ID { get; set; }
        public string F_ClassName { get; set; }

        public T_Teacher Teacher { get; set; }
}
}
