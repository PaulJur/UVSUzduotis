using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UVSUzduotis.Model
{
    public class UVSUzduotisModel
    {
        [Key]
        public int ID { get; set; }
        public int ThreadID { get; set; }
        [Display(Name = "Time Created")]
        [DisplayFormat(DataFormatString = "{0:HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime TimeCreated {  get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date {  get; set; }
        [Display(Name = "Generated Symbols")]
        public string GeneratedSymbols {  get; set; }
    }
}
