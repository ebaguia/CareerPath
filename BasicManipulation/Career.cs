using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicManipulation
{
    public class Career
    {
        public Career()
        {
        }

        public Career(String id, String name, String description)
        {
            this.id = id;
            this.name = name;
            this.description = description;
        }

        public String id { get; set; }
        public String name { get; set; }
        public String description { get; set; }
    }

    public class CareerInfoDataItem
    {
        public String finalCourse { get; set; }
    }
}
