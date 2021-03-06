﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BasicManipulation
{
    /// <summary>
    /// Interaction logic for ProgrammeWindow.xaml
    /// </summary>
    public partial class ProgrammeWindow : Window
    {
        private DepartmentWindow departmentWindow = null;
        private List<Course> listOfTopCourses = new List<Course>();
        private List<double>[] collectionOflistOfY = new List<double>[CommonInternals.TOTAL_YEARS];

        public ProgrammeWindow(DepartmentWindow departmentWindow)
        {
            InitializeComponent();
            this.departmentWindow = departmentWindow;

            initialise();
        }

        private void initialise()
        {
            String eceHeaderText = CommonInternals.ECE_DEPARTMENT_NAME + "\n" + CommonInternals.ECE_CAREERS_HEADER_TXT;

            for (int i = 0; i < collectionOflistOfY.Length; i++)
            {
                collectionOflistOfY[i] = new List<double>();
            }

            if (departmentWindow.isSetCareer)
            {
                setSelectedCareer();
                eceHeaderText = CommonInternals.ECE_DEPARTMENT_NAME + "\n" + "\"" + departmentWindow.selectedCareer.name + "\"";
            }
            else if(departmentWindow.isSetCourse)
            {
                setSelectedCourse();
            }
            else if (departmentWindow.isSetProgramme)
            {
                setSelectedProgramme();
            }

            departmentHeaderLabel.Text = eceHeaderText;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            coursePathCanvas.Children.Clear();
            if (departmentWindow != null)
            {
                departmentWindow.Show();
                Close();
            }
        }

        private void setSelectedProgramme()
        {
            List<CourseProgrammePart> programmeParts = DatabaseConnection.readCourseProgrammePart(departmentWindow.selectedProgramme);

            // Clear tree before repopulating
            //
            treeView.Items.Clear();

            // Count the number of parts of the programme
            //
            List<String> listOfParts = new List<String>();
            foreach(CourseProgrammePart courseProgrammePart in programmeParts)
            {
                bool isPartFound = false;
                foreach(String part in listOfParts)
                {
                    if (courseProgrammePart.part == part)
                    {
                        isPartFound = true;
                        break;
                    }
                }

                if (!isPartFound)
                {
                    listOfParts.Add(courseProgrammePart.part);
                }
            }

            TreeViewItem[] partItems = new TreeViewItem[listOfParts.Count];
            for (int i = 0; i < partItems.Length; i++)
            {
                if (partItems[i] == null)
                {
                    partItems[i] = new TreeViewItem();
                }

                partItems[i].IsExpanded = false;

                // Create stack panel
                //
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;
                var background = new SolidColorBrush(Colors.White);
                stack.Background = background;
                background.Opacity = 100;

                // Create Image
                //
                //Image image = new Image();
                //image.Source = new BitmapImage(new Uri(BasicManipulation.Properties.Resources.programmeIcon));

                // Part Label
                //
                Label partLabel = new Label();
                partLabel.Content = "PART " + listOfParts.ElementAt(i);

                List<Course> programmeCourses = DatabaseConnection.readCoursesUsingProgrammePart(departmentWindow.selectedProgramme, listOfParts.ElementAt(i));
                foreach(Course programmeCourse in programmeCourses)
                {
                    TreeViewItem programmeCourseItem = new TreeViewItem();

                    programmeCourseItem.IsExpanded = false;

                    createCourseListButton(programmeCourseItem, BasicManipulation.Properties.Resources.programmeIcon, programmeCourse.id);

                    partItems[i].Items.Add(programmeCourseItem);
                }

                // Add into stack
                //stack.Children.Add(image);
                stack.Children.Add(partLabel);

                // assign stack to header
                partItems[i].Header = stack;

                treeView.Items.Add(partItems[i]);
            }
        }

        private void setSelectedCareer()
        {
            List<Course> finalCoursesToTake = DatabaseConnection.readCareerFinalCourses(departmentWindow.selectedCareer);
            TreeViewItem[] finalCoursesItems = new TreeViewItem[finalCoursesToTake.Count];

            // Clear tree before repopulating
            //
            treeView.Items.Clear();

            for (int i = 0; i < finalCoursesToTake.Count; i++)
            {
                if (finalCoursesItems[i] == null)
                {
                    finalCoursesItems[i] = new TreeViewItem();
                }

                // Select the first item
                //
                if(i == 0)
                {
                    finalCoursesItems[i].IsSelected = true;
                }

                finalCoursesItems[i].IsExpanded = false;

                createCourseListButton(finalCoursesItems[i], BasicManipulation.Properties.Resources.programmeIcon, finalCoursesToTake.ElementAt(i).id);

                treeView.Items.Add(finalCoursesItems[i]);
            }
        }

        private void setSelectedCourse()
        {
            // This should always be 1 item from the course selected in the previous window (department window)
            //
            TreeViewItem courseItem = new TreeViewItem();

            // Clear tree before repopulating
            //
            treeView.Items.Clear();

            courseItem.IsSelected = true;
            courseItem.IsExpanded = false;

            createCourseListButton(courseItem, BasicManipulation.Properties.Resources.programmeIcon, departmentWindow.selectedCourse.id);

            treeView.Items.Add(courseItem);
        }

        private void clearListOfY()
        {
            foreach (List<double> listOfY in collectionOflistOfY)
            {
                if (listOfY != null)
                {
                    listOfY.Clear();
                }
            }
        }

        private void courseItemAction(object sender, System.Windows.RoutedEventArgs e)
        {
            Button button = (Button)e.Source;

            // Refresh the course path canvas
            //
            coursePathCanvas.Children.Clear();
            coursePathCanvas.Children.Add(dashLine1);
            coursePathCanvas.Children.Add(dashLine2);
            coursePathCanvas.Children.Add(dashLine3);
            listOfTopCourses.Clear();
            clearListOfY();

            // Draw the course path
            //
            try
            {
                Course targetCourse = DatabaseConnection.getCourse(button.Content.ToString());
                if (targetCourse != null)
                {
                    Utilities.fillCourseInfoDataGrid(courseInfoDataGrid, targetCourse);

                    List<Course> relatedPreReq = DatabaseConnection.generateAllRelatedPreRequisites(targetCourse);
                    relatedPreReq.Insert(0, targetCourse);
                    foreach (Course course in relatedPreReq)
                    {
                        setCoursePath(course.id);
                    }
                }
                else
                {
                    Logger.Error("ProgrammeWindow::courseItemAction() Target Course could not be found in the database.");
                }
            }
            catch(Exception ex)
            {
                Logger.Error("ProgrammeWindow::courseItemAction() " + ex.Message);
            }
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = (TreeViewItem)e.NewValue;

            if (tvi != null)
            {
                tvi.IsExpanded = !tvi.IsExpanded;

                // This should be the label of the course ID
                //
                StackPanel itemHeader = (StackPanel)((TreeViewItem)((TreeView)sender).SelectedItem).Header;

                // Refresh the course path canvas
                //
                coursePathCanvas.Children.Clear();
                coursePathCanvas.Children.Add(dashLine1);
                coursePathCanvas.Children.Add(dashLine2);
                coursePathCanvas.Children.Add(dashLine3);
                listOfTopCourses.Clear();
                clearListOfY();

                // And the course id take from the label name
                //
                var courseButton = itemHeader.Children.OfType<Button>().FirstOrDefault();

                if (!departmentWindow.isSetProgramme)
                {
                    Course targetCourse = DatabaseConnection.getCourse(courseButton.Content.ToString());
                    if (targetCourse != null)
                    {
                        Utilities.fillCourseInfoDataGrid(courseInfoDataGrid, targetCourse);

                        // Draw the course path
                        //
                        List<Course> relatedPreReq = DatabaseConnection.generateAllRelatedPreRequisites(targetCourse);
                        relatedPreReq.Insert(0, targetCourse);
                        foreach (Course course in relatedPreReq)
                        {
                            setCoursePath(course.id);
                        }
                    }
                    else
                    {
                        Logger.Error("ProgrammeWindow::treeView_SelectedItemChanged() Target Course could not be found in the database.");
                    }
                }
            }
        }

        private CommonInternals.CourseType getCourseType(Course targetCourse)
        {
            CommonInternals.CourseType courseType = CommonInternals.CourseType.ELECTIVE;

            if (DatabaseConnection.isCourseCompulsory(targetCourse))
            {
                courseType = CommonInternals.CourseType.COMPULSORY;
            }
            else if (DatabaseConnection.isCourseElective(targetCourse))
            {
                courseType = CommonInternals.CourseType.ELECTIVE;
            }

            return courseType;
        }

        public void setCoursePath(String topCourseId)
        {
            Course topCourse = DatabaseConnection.getCourse(topCourseId);
            if(topCourse == null)
            {
                return;
            }

            topCourse.setType(getCourseType(topCourse));

            // First sem courses should be in the left
            //
            double year4StartXLocation1 = Canvas.GetLeft(dashLine3) + CommonInternals.COURSE_BUTTON_TOP_GAP;
            double year3StartXLocation1 = Canvas.GetLeft(dashLine2) + CommonInternals.COURSE_BUTTON_TOP_GAP;
            double year2StartXLocation1 = Canvas.GetLeft(dashLine1) + CommonInternals.COURSE_BUTTON_TOP_GAP;
            double year1StartXLocation1 = CommonInternals.COURSE_BUTTON_TOP_GAP;

            // Second sem courses should be in the right
            //
            double year4StartXLocation2 = coursePathCanvas.Width - CommonInternals.COURSE_BUTTON_TOP_GAP - topCourse.courseButton.Width;
            double year3StartXLocation2 = Canvas.GetLeft(dashLine3) - CommonInternals.COURSE_BUTTON_TOP_GAP - topCourse.courseButton.Width;
            double year2StartXLocation2 = Canvas.GetLeft(dashLine2) - CommonInternals.COURSE_BUTTON_TOP_GAP - topCourse.courseButton.Width;
            double year1StartXLocation2 = Canvas.GetLeft(dashLine1) - CommonInternals.COURSE_BUTTON_TOP_GAP - topCourse.courseButton.Width;

            // Course can be taken in any semester - place this in the center
            //
            double year4StartXLocation0 = Canvas.GetLeft(dashLine3) + ((coursePathCanvas.Width - Canvas.GetLeft(dashLine3)) / 2) - (topCourse.courseButton.Width / 2);
            double year3StartXLocation0 = Canvas.GetLeft(dashLine2) + ((Canvas.GetLeft(dashLine3) - Canvas.GetLeft(dashLine2)) / 2) - (topCourse.courseButton.Width / 2);
            double year2StartXLocation0 = Canvas.GetLeft(dashLine1) + ((Canvas.GetLeft(dashLine2) - Canvas.GetLeft(dashLine1)) / 2) - (topCourse.courseButton.Width / 2);
            double year1StartXLocation0 = (Canvas.GetLeft(dashLine1) / 2) - (topCourse.courseButton.Width / 2);

            // Making sure things will not mess up
            //
            if ((topCourse != null) && (topCourseId != null) && (topCourseId.Length != 0))
            {
                // Populating the top most course
                //
                double previousStartX = 0;
                double previousStartY = CommonInternals.COURSE_BUTTON_LEFT_GAP;

                Course existingCourse = getCourseFromCanvas(topCourse);
                if (existingCourse == null)
                {
                    if (topCourse.year == 4)
                    {
                        if (topCourse.sem == 1)
                        {
                            // Course should be taken in the 1st sem - place this in the left
                            //
                            previousStartX = year4StartXLocation1;
                        }
                        else if (topCourse.sem == 2)
                        {
                            // Course should be taken in the 2nd sem - place this in the right
                            //
                            previousStartX = year4StartXLocation2;
                        }
                        else
                        {
                            // Course can be taken in any semester - place this in the center
                            //
                            previousStartX = year4StartXLocation0;
                        }

                        for (int i = 0; isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.FOURTH_YEAR]); i++)
                        {
                            previousStartY += topCourse.courseButton.Height + CommonInternals.COURSE_BUTTON_LEFT_GAP;
                        }
                        collectionOflistOfY[(int)CommonInternals.CourseYear.FOURTH_YEAR].Add(previousStartY);
                    }
                    else if (topCourse.year == 3)
                    {
                        if (topCourse.sem == 1)
                        {
                            previousStartX = year3StartXLocation1;
                        }
                        else if (topCourse.sem == 2)
                        {
                            previousStartX = year3StartXLocation2;
                        }
                        else
                        {
                            // Course can be taken in any semester - place this in the center
                            //
                            previousStartX = year3StartXLocation0;
                        }

                        for (int i = 0; isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.THIRD_YEAR]) == true; i++)
                        {
                            previousStartY += topCourse.courseButton.Height + CommonInternals.COURSE_BUTTON_LEFT_GAP;
                        }
                        collectionOflistOfY[(int)CommonInternals.CourseYear.THIRD_YEAR].Add(previousStartY);
                    }
                    else if (topCourse.year == 2)
                    {
                        if (topCourse.sem == 1)
                        {
                            previousStartX = year2StartXLocation1;
                        }
                        else if (topCourse.sem == 2)
                        {
                            previousStartX = year2StartXLocation2;
                        }
                        else
                        {
                            // Course can be taken in any semester - place this in the center
                            //
                            previousStartX = year2StartXLocation0;
                        }

                        for (int i = 0; isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.SECOND_YEAR]) == true; i++)
                        {
                            previousStartY += topCourse.courseButton.Height + CommonInternals.COURSE_BUTTON_LEFT_GAP;
                        }
                        collectionOflistOfY[(int)CommonInternals.CourseYear.SECOND_YEAR].Add(previousStartY);
                    }
                    else if (topCourse.year == 1)
                    {
                        if (topCourse.sem == 1)
                        {
                            previousStartX = year1StartXLocation1;
                        }
                        else if (topCourse.sem == 2)
                        {
                            previousStartX = year1StartXLocation2;
                        }
                        else
                        {
                            // Course can be taken in any semester - place this in the center
                            //
                            previousStartX = year1StartXLocation0;
                        }

                        for (int i = 0; isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.FIRST_YEAR]) == true; i++)
                        {
                            previousStartY += topCourse.courseButton.Height + CommonInternals.COURSE_BUTTON_LEFT_GAP;
                        }
                        collectionOflistOfY[(int)CommonInternals.CourseYear.FIRST_YEAR].Add(previousStartY);
                    }

                    coursePathCanvas.Children.Add(topCourse.courseButton);
                    Canvas.SetLeft(topCourse.courseButton, previousStartX);
                    Canvas.SetTop(topCourse.courseButton, previousStartY);
                    listOfTopCourses.Add(topCourse);
                }
                else
                {
                    previousStartX = Canvas.GetLeft(existingCourse.courseButton);
                    previousStartY = Canvas.GetTop(existingCourse.courseButton);
                }

                topCourse.programmeWindow = this;

                Rect topcourseButtonRect = Utilities.GetRectOfObject(topCourse.courseButton);
                Point relativePointEllipse2Left = new Point(previousStartX, previousStartY + (topcourseButtonRect.Height / 2));
                Point relativePointEllipse2Right = new Point(previousStartX + (topcourseButtonRect.Width), previousStartY + (topcourseButtonRect.Height / 2));

                // Populating the course path
                //
                List<Course> courses = DatabaseConnection.getPrerequisiteCourses(topCourse.id);
                Line[] connector = new Line[courses.Count];

                for (int i = 0; i < courses.Count; i++)
                {
                    Course course = courses.ElementAt(i);
                    course.setType(getCourseType(course));

                    previousStartY = CommonInternals.COURSE_BUTTON_LEFT_GAP;
                    Course existingCourse2 = getCourseFromCanvas(course);
                    if (existingCourse2 == null)
                    {
                        if (course.year == 4)
                        {
                            // Set the X location
                            //
                            if (course.sem == 1)
                            {
                                previousStartX = year4StartXLocation1;
                            }
                            else if (course.sem == 2)
                            {
                                previousStartX = year4StartXLocation2;
                            }
                            else
                            {
                                // Course can be taken in any semester - place this in the center
                                //
                                previousStartX = year4StartXLocation0;
                            }

                            // Move new courses to the end of the list - descending
                            //
                            while (isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.FOURTH_YEAR]))
                            {
                                previousStartY += course.courseButton.Height + CommonInternals.COURSE_BUTTON_LEFT_GAP;
                            }
                            collectionOflistOfY[(int)CommonInternals.CourseYear.FOURTH_YEAR].Add(previousStartY);
                        }
                        else if (course.year == 3)
                        {
                            if (course.sem == 1)
                            {
                                previousStartX = year3StartXLocation1;
                            }
                            else if (course.sem == 2)
                            {
                                previousStartX = year3StartXLocation2;
                            }
                            else
                            {
                                // Course can be taken in any semester - place this in the center
                                //
                                previousStartX = year3StartXLocation0;
                            }

                            // Move new courses to the end of the list - descending
                            //
                            while (isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.THIRD_YEAR]))
                            {
                                previousStartY += course.courseButton.Height + CommonInternals.COURSE_BUTTON_LEFT_GAP;
                            }
                            collectionOflistOfY[(int)CommonInternals.CourseYear.THIRD_YEAR].Add(previousStartY);
                        }
                        else if (course.year == 2)
                        {
                            if (course.sem == 1)
                            {
                                previousStartX = year2StartXLocation1;
                            }
                            else if (course.sem == 2)
                            {
                                previousStartX = year2StartXLocation2;
                            }
                            else
                            {
                                // Course can be taken in any semester - place this in the center
                                //
                                previousStartX = year2StartXLocation0;
                            }

                            // Move new courses to the end of the list - descending
                            //
                            while (isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.SECOND_YEAR]))
                            {
                                previousStartY += course.courseButton.Height + CommonInternals.COURSE_BUTTON_LEFT_GAP;
                            }
                            collectionOflistOfY[(int)CommonInternals.CourseYear.SECOND_YEAR].Add(previousStartY);
                        }
                        else if (course.year == 1)
                        {
                            if (course.sem == 1)
                            {
                                previousStartX = year1StartXLocation1;
                            }
                            else if (course.sem == 2)
                            {
                                previousStartX = year1StartXLocation2;
                            }
                            else
                            {
                                // Course can be taken in any semester - place this in the center
                                //
                                previousStartX = year1StartXLocation0; ;
                            }

                            // Move new courses to the end of the list - descending
                            //
                            while (isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.FIRST_YEAR]))
                            {
                                previousStartY += course.courseButton.Height + CommonInternals.COURSE_BUTTON_LEFT_GAP;
                            }
                            collectionOflistOfY[(int)CommonInternals.CourseYear.FIRST_YEAR].Add(previousStartY);
                        }

                        coursePathCanvas.Children.Add(course.courseButton);
                        Canvas.SetLeft(course.courseButton, previousStartX);
                        Canvas.SetTop(course.courseButton, previousStartY);
                        course.programmeWindow = this;
                        listOfTopCourses.Add(course);
                    }
                    else
                    {
                        previousStartX = Canvas.GetLeft(existingCourse2.courseButton);
                        previousStartY = Canvas.GetTop(existingCourse2.courseButton);
                        course = existingCourse2;
                    }

                    // Sets the location where to start drawing the Line connecting the courses
                    //
                    Point relativePointEllipse1 = new Point(previousStartX + (topcourseButtonRect.Width), previousStartY + (topcourseButtonRect.Height / 2));

                    // For now, we do not need to create a course connecting line of both courses are in the same year.
                    //
                    if (topCourse.year != course.year)
                    {
                        var points = Utilities.CreateLineWithArrowPointCollection(new Point(relativePointEllipse1.X, relativePointEllipse1.Y),
                                                                        new Point(relativePointEllipse2Left.X, relativePointEllipse2Left.Y),
                                                                        1.25);

                        var polygon = new Polygon();
                        polygon.Points = points;
                        polygon.Fill = Brushes.Black;

                        coursePathCanvas.Children.Add(polygon);
                    }
                    /*else
                    {
                        // This is when the pre-requisite course is within the same year level as the target course
                        //
                        var courseConnectorePolyline = new Polyline();
                        courseConnectorePolyline.Stroke = Brushes.Black;
                        courseConnectorePolyline.StrokeThickness = 1;

                        PointCollection polygonPoints = new PointCollection();

                        if(course.sem == 1)
                        {
                            // 3 points for pre-req which is in the previous sem
                            //
                            Point[] points = new Point[3];

                            points[0] = new Point(relativePointEllipse1.X, relativePointEllipse1.Y);
                            points[1] = new Point(relativePointEllipse1.X + (course.courseButton.Width / 4) - 25, relativePointEllipse1.Y);
                            points[2] = new Point(Canvas.GetLeft(topCourse.courseButton) + (topCourse.courseButton.Width / 2), Canvas.GetTop(topCourse.courseButton) + topCourse.courseButton.Height);
                            //points[3] = new Point(Canvas.GetLeft(topCourse.courseButton) + topCourse.courseButton.Width, Canvas.GetTop(topCourse.courseButton) + (topCourse.courseButton.Height/2));

                            polygonPoints.Add(points[0]);
                            polygonPoints.Add(points[1]);
                            polygonPoints.Add(points[2]);
                            //polygonPoints.Add(points[3]);
                        }
                        else if (course.sem == 2)
                        {
                            // 4 points for pre-req which is in the previous sem
                            //
                            Point[] points = new Point[2];

                            points[0] = new Point(previousStartX + (course.courseButton.Width / 2), previousStartY + topcourseButtonRect.Height);
                            points[1] = new Point(relativePointEllipse1.X + (topCourse.courseButton.Width / 2), relativePointEllipse1.Y);
                            polygonPoints.Add(points[0]);
                            polygonPoints.Add(points[1]);
                        }

                        courseConnectorePolyline.Points = polygonPoints;

                        coursePathCanvas.Children.Add(courseConnectorePolyline);
                    }
                    */
                }
            }
        }

        private Course getCourseFromCanvas(Course course)
        {
            Course exsitingCourse = null;
            foreach (Course tempCourse in listOfTopCourses)
            {
                if (tempCourse.id == course.id)
                {
                    exsitingCourse = tempCourse;
                    break;
                }
            }

            return exsitingCourse;
        }

        private bool isCoursePointYExist(double Y, List<double> listOfY)
        {
            bool isYExist = false;            
            foreach (double tempY in listOfY)
            {
                if (tempY == Y)
                {
                    isYExist = true;
                    break;
                }
            }

            return isYExist;
        }

        private void createCourseListButton(TreeViewItem courseItem, String imageStringLocation, String buttonStringContent)
        {
            // create stack panel
            //
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            
            var background = new SolidColorBrush(Colors.White);
            background.Opacity = 100;
            stack.Background = background;

            // create Image
            //
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(imageStringLocation));

            // Course Button
            //
            Button btn = new Button();
            btn.Height = CommonInternals.TREE_COURSE_BUTTON_HEIGHT;
            btn.Width = CommonInternals.TREE_COURSE_BUTTON_WIDTH;
            btn.Content = buttonStringContent;
            btn.Click += courseItemAction;

            RadialGradientBrush gradRadial = new RadialGradientBrush();
            gradRadial.RadiusX = 0.25;
            gradRadial.GradientOrigin = new Point(0.498, 0.526);
            GradientStopCollection gradStopCollection = new GradientStopCollection();
            gradStopCollection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFF3F3F3"), 0));
            gradStopCollection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFEBEBEB"), 0.5));
            gradStopCollection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FFDDDDDD"), 0.5));
            gradStopCollection.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF94C4E0"), 1));
            gradRadial.GradientStops = gradStopCollection;

            btn.Background = gradRadial;

            // Add into stack
            //
            stack.Children.Add(image);
            stack.Children.Add(btn);

            // assign stack to header
            //
            courseItem.Header = stack;

            courseItem.Margin = new Thickness(5);
        }
    }
}
