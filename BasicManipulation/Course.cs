using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Effects;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BasicManipulation
{
    public class Course
    {
        public Course()
        {

        }

        public Course(String id, 
                    int year,
                    int sem, 
                    String name, 
                    String description, 
                    int points, 
                    String academicOrg, 
                    String academicGroup, 
                    String courseComp, 
                    String gradingBasis, 
                    String typeOffered,
                    String restr = "",
                    String preReq = "",
                    String coReq = "",
                    String remarks = "",
                    String careerId = "",
                    CommonInternals.CourseType type=CommonInternals.CourseType.ELECTIVE)
        {
            this.id = id;
            this.year = year;
            this.sem = sem;
            this.name = name;
            this.description = description; 
            this.points = points;
            this.academicOrg = academicOrg;
            this.academicGroup = academicGroup;
            this.courseComp = courseComp;
            this.gradingBasis = gradingBasis;
            this.typeOffered = typeOffered;
            this.restr = restr;
            this.preReq = preReq;
            this.coReq = coReq;
            this.remarks = remarks;
            this.careerId = careerId;
            this.type = type;

            initialise();
        }

        private void  initialise()
        {
            courseButton = new Button();
            courseButton.Height = 80;
            courseButton.Width = 180;
            courseButton.Content = "Course: " + id + "\nSemester: " + sem + "\nPoints: " + points;
            courseButton.BorderBrush = Brushes.Black;
            courseButton.BorderThickness = new System.Windows.Thickness(0);
            courseButton.FontSize = 15;
            courseButton.FontFamily = new FontFamily("Century Gothic");
            courseButton.Click += buttonAction;

            courseButton.Effect = new DropShadowEffect
            {
                ShadowDepth = 5,
                Direction = 330,
                Color = Colors.Black,
                Opacity = 100,
                BlurRadius = 5
            };
        }

        private void buttonAction(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = (Button)e.Source;

            Logger.Debug("Course::buttonAction() Label = " + button.Content);
            Logger.Debug("Course::buttonAction() course ID = " + id);
            Logger.Debug("Course::buttonAction() course Name = " + name);

            if (programmeWindow != null)
            {
                Utilities.fillCourseInfoDataGrid(programmeWindow.courseInfoDataGrid, this);
            }
        }

        // Course items
        //
        public String id { get; set; }
        public int year { get; set; }
        public int sem { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public int points { get; set; }
        public String academicOrg { get; set; }
        public String academicGroup { get; set; }
        public String courseComp { get; set; }
        public String gradingBasis { get; set; }
        public String typeOffered { get; set; }
        public String restr { get; set; }
        public String preReq { get; set; }
        public String coReq { get; set; }
        public String remarks { get; set; }
        public String careerId { get; set; }

        private CommonInternals.CourseType type;

        public CommonInternals.CourseType getType()
        {
            return type;
        }

        public void setType(CommonInternals.CourseType type)
        {
            if(type == CommonInternals.CourseType.COMPULSORY)
            {
                if(courseButton != null)
                {
                    courseButton.Background = Brushes.OrangeRed;
                }
            }
            else
            {
                if(courseButton != null)
                {
                    courseButton.Background = Brushes.LightGreen;
                }
            }

            this.type = type;
        }

        // Controls
        //
        public Button courseButton { get; set; }
        public ProgrammeWindow programmeWindow { get; set; }
        public Point location { get; set; }
    }

    public class CourseInfoDataItem
    {
        public String item { get; set; }
        public String description { get; set; }
    }
}
