using System;
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
            Hide();
            if (departmentWindow != null)
            {
                departmentWindow.Show();
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
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(BasicManipulation.Properties.Resources.programmeIcon));

                // Part Label
                //
                Label partLabel = new Label();
                partLabel.Content = "Part " + listOfParts.ElementAt(i);

                List<Course> programmeCourses = DatabaseConnection.readCoursesUsingProgrammePart(departmentWindow.selectedProgramme, listOfParts.ElementAt(i));
                foreach(Course programmeCourse in programmeCourses)
                {
                    TreeViewItem programmeCourseItem = new TreeViewItem();

                    programmeCourseItem.IsExpanded = false;

                    // Create stack panel
                    //
                    StackPanel innerStack = new StackPanel();
                    innerStack.Orientation = Orientation.Horizontal;
                    innerStack.Background = background;
                    background.Opacity = 100;

                    // Course button
                    //
                    Button btn = new Button();
                    btn.Height = CommonInternals.TREE_COURSE_BUTTON_HEIGHT;
                    btn.Width = CommonInternals.TREE_COURSE_BUTTON_WIDTH;
                    btn.Content = programmeCourse.id;
                    btn.Click += courseItemAction;

                    // Create Image
                    //
                    Image innerImage = new Image();
                    innerImage.Source = new BitmapImage(new Uri(BasicManipulation.Properties.Resources.courseIcon));

                    // Add into stack
                    //
                    innerStack.Children.Add(innerImage);
                    innerStack.Children.Add(btn);

                    // assign stack to header
                    //
                    programmeCourseItem.Header = innerStack;

                    partItems[i].Items.Add(programmeCourseItem);
                }

                // Add into stack
                stack.Children.Add(image);
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

                // create stack panel
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;
                var background = new SolidColorBrush(Colors.White);
                stack.Background = background;
                background.Opacity = 100;

                // create Image
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(BasicManipulation.Properties.Resources.courseIcon));

                // Button
                Button btn = new Button();
                btn.Height = CommonInternals.TREE_COURSE_BUTTON_HEIGHT;
                btn.Width = CommonInternals.TREE_COURSE_BUTTON_WIDTH;
                btn.Content = finalCoursesToTake.ElementAt(i).id;
                btn.Click += courseItemAction;


                // Add into stack
                stack.Children.Add(image);
                stack.Children.Add(btn);

                // assign stack to header
                finalCoursesItems[i].Header = stack;

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

            // create stack panel
            //
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            var background = new SolidColorBrush(Colors.White);
            stack.Background = background;
            background.Opacity = 100;

            // create Image
            //
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(BasicManipulation.Properties.Resources.courseIcon));

            // Course Button
            //
            Button btn = new Button();
            btn.Height = CommonInternals.TREE_COURSE_BUTTON_HEIGHT;
            btn.Width = CommonInternals.TREE_COURSE_BUTTON_WIDTH;
            btn.Content = departmentWindow.selectedCourse.id;
            btn.Click += courseItemAction;


            // Add into stack
            //
            stack.Children.Add(image);
            stack.Children.Add(btn);

            // assign stack to header
            //
            courseItem.Header = stack;
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
            topCourse.setType(getCourseType(topCourse));

            // First sem courses should be in the left
            //
            double year4StartXLocation1 = coursePathCanvas.Width - ((coursePathCanvas.Width / 4) - 100) - 35;
            double year3StartXLocation1 = coursePathCanvas.Width - (((coursePathCanvas.Width / 4) * 2) - 100) - 35;
            double year2StartXLocation1 = coursePathCanvas.Width - (((coursePathCanvas.Width / 4) * 3) - 100) - 35;
            double year1StartXLocation1 = coursePathCanvas.Width - (coursePathCanvas.Width - 100) - 35;

            // Second sem courses should be in the left
            //
            double year4StartXLocation2 = coursePathCanvas.Width - ((coursePathCanvas.Width / 4) - 100) + 25;
            double year3StartXLocation2 = coursePathCanvas.Width - (((coursePathCanvas.Width / 4) * 2) - 100) + 25;
            double year2StartXLocation2 = coursePathCanvas.Width - (((coursePathCanvas.Width / 4) * 3) - 100) + 25;
            double year1StartXLocation2 = coursePathCanvas.Width - (coursePathCanvas.Width - 100) + 25;

            // Making sure things will not mess up
            //
            if ((topCourse != null) && (topCourseId != null) && (topCourseId.Length != 0))
            {
                // Populating the top most course
                //
                double previousStartX = 0;
                double previousStartY = CommonInternals.COURSE_BUTTON_GAP;
                Course existingCourse = getCourseFromCanvas(topCourse);

                // Start from the previous and existing course
                //
                if (existingCourse != null)
                {
                    previousStartY = Canvas.GetLeft(existingCourse.courseButton);
                }

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
                        previousStartX = year4StartXLocation2 - 25;
                    }

                    for (int i = 0; isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.FOURTH_YEAR]); i++)
                    {
                        previousStartY += topCourse.courseButton.Height + CommonInternals.COURSE_BUTTON_GAP;
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
                        previousStartX = year3StartXLocation2 - 25;
                    }

                    for (int i = 0; isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.THIRD_YEAR]) == true; i++)
                    {
                        previousStartY += topCourse.courseButton.Height + CommonInternals.COURSE_BUTTON_GAP;
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
                        previousStartX = year2StartXLocation2 - 25;
                    }

                    for (int i = 0; isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.SECOND_YEAR]) == true; i++)
                    {
                        previousStartY += topCourse.courseButton.Height + CommonInternals.COURSE_BUTTON_GAP;
                    }
                    collectionOflistOfY[(int)CommonInternals.CourseYear.SECOND_YEAR].Add(previousStartY);
                }
                else if(topCourse.year == 1)
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
                        previousStartX = year1StartXLocation2 - 25;
                    }

                    for (int i = 0; isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.FIRST_YEAR]) == true; i++)
                    {
                        previousStartY += topCourse.courseButton.Height + CommonInternals.COURSE_BUTTON_GAP;
                    }
                    collectionOflistOfY[(int)CommonInternals.CourseYear.FIRST_YEAR].Add(previousStartY);
                }

                if (existingCourse == null)
                {
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

                    previousStartY = CommonInternals.COURSE_BUTTON_GAP;
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

                            // Move new courses to the end of the list - descending
                            //
                            while (isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.FOURTH_YEAR]))
                            {
                                previousStartY += course.courseButton.Height + CommonInternals.COURSE_BUTTON_GAP;
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

                            foreach(double y in collectionOflistOfY[(int)CommonInternals.CourseYear.THIRD_YEAR])
                            {
                                Logger.Debug("ACHTUNG listOfY: " + y);
                            }

                            // this course goes below
                            //
                            while (isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.THIRD_YEAR]))
                            {
                                previousStartY += course.courseButton.Height + CommonInternals.COURSE_BUTTON_GAP;
                            }
                            Logger.Debug("ACHTUNG PreReqCourse Y ADD[" + course.id + "] Y: " + previousStartY);
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

                            // this course goes below
                            //
                            while (isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.SECOND_YEAR]))
                            {
                                previousStartY += course.courseButton.Height + CommonInternals.COURSE_BUTTON_GAP;
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

                            // this course goes below
                            //
                            while (isCoursePointYExist(previousStartY, collectionOflistOfY[(int)CommonInternals.CourseYear.FIRST_YEAR]))
                            {
                                previousStartY += course.courseButton.Height + CommonInternals.COURSE_BUTTON_GAP;
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

                    Logger.Debug("ACHTUNG PreReqCourse[" + course.id + "] Y: " + previousStartY);
                    Point relativePointEllipse1 = new Point(previousStartX + (topcourseButtonRect.Width), previousStartY + (topcourseButtonRect.Height / 2));

                    if (topCourse.year != course.year)
                    {
                        var points = Utilities.CreateLineWithArrowPointCollection(new Point(relativePointEllipse1.X, relativePointEllipse1.Y),
                                                                        new Point(relativePointEllipse2Left.X, relativePointEllipse2Left.Y),
                                                                        1);

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
                            Point[] points = new Point[4];

                            points[0] = new Point(relativePointEllipse1.X, relativePointEllipse1.Y);
                            points[1] = new Point(relativePointEllipse1.X + (course.courseButton.Width / 3), relativePointEllipse1.Y);
                            points[2] = new Point(Canvas.GetLeft(topCourse.courseButton) + topCourse.courseButton.Width + (course.courseButton.Width / 3), Canvas.GetTop(topCourse.courseButton) + (topCourse.courseButton.Height / 2));
                            points[3] = new Point(Canvas.GetLeft(topCourse.courseButton) + topCourse.courseButton.Width, Canvas.GetTop(topCourse.courseButton) + (topCourse.courseButton.Height/2));

                            polygonPoints.Add(points[0]);
                            polygonPoints.Add(points[1]);
                            polygonPoints.Add(points[2]);
                            polygonPoints.Add(points[3]);
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
    }
}
