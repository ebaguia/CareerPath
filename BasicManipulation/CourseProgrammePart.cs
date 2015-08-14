using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicManipulation
{
    public class CourseProgrammePart
    {
        public String courseId { get; set; }
        public String part { get; set; }
        public String programmeId { get; set; }

        public CourseProgrammePart(String courseId, String part, String programmeId)
        {
            this.courseId = courseId;
            this.part = part;
            this.programmeId = programmeId;
        }
    }
}
