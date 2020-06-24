using System;
using System.Collections.Generic;
using System.Text;

namespace JZ.Core.Models
{
    /// <summary>
    /// 班级
    /// </summary>
    public class T_Class
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? F_ID { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public string F_ClassName { get; set; }
        /// <summary>
        /// 班主任
        /// </summary>

        public T_Teacher Teacher { get; set; }
}
}
