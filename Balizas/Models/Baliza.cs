using System;
using System.Collections.Generic;
using System.Text;

namespace Balizas.Models
{
    class Baliza
    {
        public String id { set; get; }
        public String name { set; get; }
        public String nameEus { set; get; }
        public String municipality { set; get; }
        public String province { set; get; }
        public double altitude { set; get; }
        public double x { set; get; }
        public double y { set; get; }
        public String stationType { set; get; }
    }
}
