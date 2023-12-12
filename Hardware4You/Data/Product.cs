using Syncfusion.ExcelExport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware4You.Data
{
    public partial class Product
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public int Price { get; set; }

        public string Description { get; set; } = null!;
        
        //TODO: how to set image for each product differently db
        //public Image Image { get; set; };

        public string Specifications { get; set; } = null!;
    }
}