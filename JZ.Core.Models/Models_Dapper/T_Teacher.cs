using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JZ.Core.Models
{
    //[Display(Name = "T_Teacher")]
    /// <summary>
    /// 老师
    /// </summary>
    public class T_Teacher
    {
        /// <summary>
        /// ID
        /// </summary>
        public int? F_ID { get; set; }
        /// <summary>
        /// 老师名称
        /// </summary>
        public string F_TeacherName { get; set; }
    }
}
