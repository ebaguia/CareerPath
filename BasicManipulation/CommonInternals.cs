using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicManipulation
{
    public class CommonInternals
    {
        public enum Departments
        {
            ES,
            CME,
            ECE,
            FOE
        };

        public enum CourseType
        {
            COMPULSORY,
            ELECTIVE
        };

        public enum CourseYear
        {
            FIRST_YEAR,
            SECOND_YEAR,
            THIRD_YEAR,
            FOURTH_YEAR
        };
        
        // Should be updated from the database
        //
        public enum Programmes
        {
            CSE,
            EEE,
            SE
        };

        public const String PROG_CSE = "CSE";
        public const String PROG_EEE = "EEE";
        public const String PROG_SE = "SE";

        public const String OUR_FACULTY_DETAILS_TXT = "The Faculty of Engineering has more than 3,900 students: 2,900 at undergraduate level, 430 in taught postgraduate programmes and over 500 research postgraduate students.\n\nResearch is  a key activity at the faculty, with large scale research activities and collaborations with other institutions and members of the industry. Much of the work undertaken is of a world-class level, while in many areas, the research programmes, and academics and students involved, are clearly leading the world in their discoveries and developments.\n\nStudent life is vibrant within the faculty. The Engineering Atrium, which includes an oval-shaped 250-seat lecture theatre, four new computer laboratories and a café, is the hub for student activities. The atrium and its surroundings provide a relaxed environment for students that can be used for both study and socialising.\n\nOur illustrious alumni include former University of Oxford Vice-Chancellor, Dr John Hood, the most successful yachtsman in America's Cup history, Russell Coutts, and Chris Liddell, previously Chief Financial Officer of both Microsoft and General Motors.";
        public const String OUR_FACULTY_HEADER_TXT = "Our Faculty";
        public const String CAREER_TXT = "Careers";
        public const String PATH_TXT = "Path";
        public const String MORE_TXT = "More>>>";
        public const String PREV_TXT = "<<<Prev";
        public const String CAREER_PATH_TXT = CAREER_TXT + PATH_TXT;
        public const String ECE_CAREERS_HEADER_TXT = "ECE - Using technology for humanity";
        public const String DB_NAME = "CareerPath.db";
        public const String ECE_HEADER1 = "Why study ECE at The University of Auckland?";
        public const String ECE_HEADER2 = "What is Electrical and Computer Engineering?";
        public const String ECE_HEADER3 = "Want to study in these exciting programmes?";
        public const String ECE_HEADER4 = "Want to work in these exciting fields?";
        public const String ECE_HEADER5 = "Study this ECE degree now!";

        public const String ECE_LABEL1 = "The Department of Electrical and Computer Engineering at The University of Auckland will equip positive, proactive people with the skills they require to embark on successful professional careers.\n\nElectrical, computer and software engineers have been responsible for the creation of electric power, electronics, computers, electronic communication systems, modern flight controllers, automated manufacturing, and medical diagnostic tools.\n\nThe Department of Electrical and Computer Engineering (ECE) drives industry partnerships that enable cutting edge learning. We build on our experience and the innovation of our researchers to provide the very best standards in research and learning. We equip students with the skills required to embark on successful professional careers.\n\nAuckland is New Zealand’s largest financial and industrial centre so there’s no shortage of career prospects. Students benefit from exposure to projects similar to those in industry. Graduates are in demand by industry recruiters, and many have found senior positions with New Zealand’s foremost companies.";
        public const String ECE_LABEL2 = "The field of engineering that deals with the study and application of electricity, electronics and electromagnetism, computer systems, signal processing, control systems and software design.";

        // ECE
        //
        public const String ECE_SLOGAN = "We are committed to research and dedicated to the pursuit of execellence in teaching, innovation and knowledge.";

        public const int COURSE_BUTTON_GAP = 10;

        public const int TOTAL_YEARS = 4;

        public const int TREE_COURSE_BUTTON_HEIGHT = 40;
        public const int TREE_COURSE_BUTTON_WIDTH = 210;
    }
}
