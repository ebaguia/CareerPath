using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicManipulation
{
    public class Programme
    {
        public Programme()
        {

        }

        public Programme(String id, String name, String description, String courseId)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.courseId = courseId;
        }

        public String id { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public String courseId { get; set; }
    }
}
