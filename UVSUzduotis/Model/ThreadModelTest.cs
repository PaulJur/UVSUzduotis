using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVSUzduotis.Model
{
    public class ThreadModelTest
    {
        [Key]
        public int ID { get; set; }
        public int ThreadID { get; set; }
        public string GeneratedString { get; set; }

    }
}
