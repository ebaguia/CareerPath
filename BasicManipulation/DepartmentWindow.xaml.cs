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
using System.Windows.Navigation;

namespace BasicManipulation
{
    /// <summary>
    /// Interaction logic for DepartmentWindow.xaml
    /// </summary>
    public partial class DepartmentWindow : Window
    {
        private MainWindow mainWindow = null;
        public Career selectedCareer { get; set; }
        public Course selectedCourse { get; set; }
        public Programme selectedProgramme { get; set; }
        public List<Career> careers { get; set; }
        public List<Programme> programmes { get; set; }
        public List<Course> courses { get; set; }
        public bool isSetCareer { get; set; }
        public bool isSetCourse { get; set; }
        public bool isSetProgramme { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DepartmentWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            initialise();
        }

        private void initialise()
        {
            // Main Page init
            //
            mainPageLabelLeft.Text = CommonInternals.ECE_HEADER1;
            mainPageLabelRight.Text = CommonInternals.ECE_HEADER2;
            mainPageTBLeft.Text = CommonInternals.ECE_LABEL1;
            mainPageTBRight.Text = CommonInternals.ECE_LABEL2;
            mainPageGrid.Visibility = Visibility.Visible;

            careerPageGrid.Visibility = Visibility.Collapsed;
            programmePageGrid.Visibility = Visibility.Collapsed;
            coursePageGrid.Visibility = Visibility.Collapsed;

            careers = new List<Career>();
            programmes = new List<Programme>();
            courses = new List<Course>();

            DatabaseConnection.readCareers(careers);
            DatabaseConnection.readProgrammes(programmes);
            DatabaseConnection.readCourses(courses);

            isSetCareer = false;
            isSetCourse = false;
            isSetProgramme = false;

            // The lable dynamically changes in every Career
            //
            departmentHeaderLabel.Text = CommonInternals.ECE_DEPARTMENT_NAME + "\n" + CommonInternals.ECE_CAREERS_HEADER_TXT;
        }

        /// <summary>
        /// Handles the clicking of the HOME button.
        /// </summary>
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            if (mainWindow != null)
            {
                mainWindow.Show();
                Close();
            }
        }

        /// <summary>
        /// Lists all careers from the database to a TreeView.
        /// </summary>
        private void generateCareersTreeView()
        {
            TreeViewItem[] careerItems = new TreeViewItem[careers.Count];

            // Clear tree before repopulating
            //
            careerTreeView.Items.Clear();

            for (int i = 0; i < careers.Count; i++)
            {
                if(careerItems[i] == null)
                {
                    careerItems[i] = new TreeViewItem();
                }

                careerItems[i].IsExpanded = false;

                // create stack panel
                //
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                // create Image
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(BasicManipulation.Properties.Resources.careersIcon));

                // Label
                //
                Label lbl = new Label();
                lbl.Content = careers.ElementAt(i).name;

                // Add into stack
                stack.Children.Add(image);
                stack.Children.Add(lbl);

                // assign stack to header
                //
                careerItems[i].Header = stack;

                // Fields which define the career
                //
                List<String> fieldsOfWork = DatabaseConnection.readCareerJobs(careers.ElementAt(i));
                TreeViewItem[] careerFieldsItems = new TreeViewItem[fieldsOfWork.Count];
                for (int j = 0; j < fieldsOfWork.Count; j++)
                {
                    if(careerFieldsItems[j] == null)
                    {
                        careerFieldsItems[j] = new TreeViewItem();
                    }

                    StackPanel childStack = new StackPanel();
                    childStack.Orientation = Orientation.Horizontal;

                    Label bullet = new Label();
                    bullet.Content = "\u25A0";

                    // Label
                    //
                    Label childLbl = new Label();
                    childLbl.Content = "  " + fieldsOfWork.ElementAt(j);


                    // Add into stack
                    //
                    childStack.Children.Add(bullet);
                    childStack.Children.Add(childLbl);

                    careerFieldsItems[j].Header = childStack;
                    careerItems[i].Items.Add(careerFieldsItems[j]);
                }

                careerTreeView.Items.Add(careerItems[i]);
            }
        }

        /// <summary>
        /// Lists all programmes from the database to a TreeView.
        /// </summary>
        private void generateProgrammesTreeView()
        {
            TreeViewItem[] programmeItems = new TreeViewItem[programmes.Count];

            // Clear tree before repopulating
            //
            programmeTreeView.Items.Clear();

            for (int i = 0; i < programmes.Count; i++)
            {
                if (programmeItems[i] == null)
                {
                    programmeItems[i] = new TreeViewItem();
                }

                programmeItems[i].IsExpanded = false;
                
                // create stack panel
                //
                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                // create Image
                //
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(BasicManipulation.Properties.Resources.programmeIcon));

                // Label
                //
                Label lbl = new Label();
                lbl.Content = programmes.ElementAt(i).name;


                // Add into stack
                //
                stack.Children.Add(image);
                stack.Children.Add(lbl);

                programmeItems[i].Header = stack;

                programmeTreeView.Items.Add(programmeItems[i]);
            }
        }

        /// <summary>
        /// Handles the clicking of the Career button.
        /// </summary>
        private void careerPathwayButton_Click(object sender, RoutedEventArgs e)
        {
            careerPageGrid.Visibility = Visibility.Visible;

            mainPageGrid.Visibility = Visibility.Collapsed;
            programmePageGrid.Visibility = Visibility.Collapsed;
            coursePageGrid.Visibility = Visibility.Collapsed;
            
            generateCareersTreeView();
        }

        /// <summary>
        /// Handles the programme selection.
        /// </summary>
        private void programmeTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = (TreeViewItem)e.NewValue;

            if (tvi != null)
            {
                tvi.IsExpanded = !tvi.IsExpanded;

                StackPanel itemHeader = (StackPanel)((TreeViewItem)((TreeView)sender).SelectedItem).Header;

                var programmeLabel = itemHeader.Children.OfType<Label>().FirstOrDefault();

                foreach (var programme in programmes)
                {
                    if (programme.name == programmeLabel.Content.ToString())
                    {
                        programmePageTBRight.Text = DatabaseConnection.readProgrammeName(programme.id) + " (" + programme.id + ")" + "\n\n"
                            + DatabaseConnection.readProgrammeDescription(programme.id);

                        selectedProgramme = programme;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the career selection.
        /// </summary>
        private void careerTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = (TreeViewItem)e.NewValue;

            if (tvi != null)
            {
                tvi.IsExpanded = !tvi.IsExpanded;

                StackPanel itemHeader = (StackPanel)((TreeViewItem)((TreeView)sender).SelectedItem).Header;

                var careerLabel = itemHeader.Children.OfType<Label>().FirstOrDefault();

                foreach(var career in careers)
                {
                    if (career.name == careerLabel.Content.ToString())
                    {
                        selectedCareer = null;

                        // Set the final courses for this career
                        //
                        careerFinalCoursesGrid.Items.Clear();
                        List<Course> finalCoursesToTake = DatabaseConnection.readCareerFinalCourses(career);
                        foreach(Course course in finalCoursesToTake)
                        {
                            careerFinalCoursesGrid.Items.Add(new CareerInfoDataItem() { finalCourse = course.id + " - " + course.name });
                        }
                        // Workaround to fill the table
                        //
                        for (int i = 0; i < 5; i++)
                        {
                            careerFinalCoursesGrid.Items.Add(new CareerInfoDataItem() { finalCourse = "" });
                        }
                        //careerFinalCoursesGrid.Visibility = Visibility.Visible;

                        if (selectedCareer == null)
                        {
                            selectedCareer = new Career(career.id,
                                                    career.name,
                                                    career.description);
                        }
                        else
                        {
                            selectedCareer.id = career.id;
                            selectedCareer.name = career.name;
                            selectedCareer.description = career.description;
                        }

                        Logger.Info("selectedCareer.name: " + selectedCareer.name);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the clicking of the Programme button.
        /// </summary>
        private void programmesButton_Click(object sender, RoutedEventArgs e)
        {
            programmePageGrid.Visibility = Visibility.Visible;

            mainPageGrid.Visibility = Visibility.Collapsed;
            careerPageGrid.Visibility = Visibility.Collapsed;
            coursePageGrid.Visibility = Visibility.Collapsed;

            generateProgrammesTreeView();
        }

        /// <summary>
        /// Handles the clicking of the Lear More button of the Career page.
        /// </summary>
        private void learnMoreFromCareer_Click(object sender, RoutedEventArgs e)
        {
            isSetCareer = true;
            isSetCourse = false;
            isSetProgramme = false;

            if(careerFinalCoursesGrid.Items.Count != 0)
            {
                if (selectedCareer != null)
                {
                    showProgrammesWindow();
                }
            }
            else
            {
                Logger.Error("DepartmenWindow::learnMoreFromCareer_Click() A single Career was not selected.");
                MessageBox.Show("Please select a Career from the list.", "Career Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Shows the programmeWindow which will layout the Career Path.
        /// </summary>
        private void showProgrammesWindow()
        {
            ProgrammeWindow programmeWindow = new ProgrammeWindow(this);
            programmeWindow.Show();
            Hide();
        }

        /// <summary>
        /// When the Course button is clicked.
        /// </summary>
        private void coursesButton_Click(object sender, RoutedEventArgs e)
        {
            coursePageGrid.Visibility = Visibility.Visible;

            mainPageGrid.Visibility = Visibility.Collapsed;
            careerPageGrid.Visibility = Visibility.Collapsed;
            programmePageGrid.Visibility = Visibility.Collapsed;

            // Generate courses for each programme
            //
            generateCoursesTreeView();
        }

        /// <summary>
        /// Generates the Courses and place them in a Treeview. The information of the Courses are taken from the database.
        /// </summary>
        private void generateCoursesTreeView()
        {
            // Clear tree before repopulating
            //
            cseTreeView.Items.Clear();
            eeeTreeView.Items.Clear();
            seTreeView.Items.Clear();

            List<Course>[] coursesFromProgramme = new List<Course>[programmes.Count];
            foreach(Programme programme in programmes)
            {
                List<Course> tempCoursesFromProgramme = null;
                TreeView courseTreeView = null;
                if(programme.id.Equals(CommonInternals.PROG_CSE))
                {
                    coursesFromProgramme[(int)CommonInternals.Programmes.CSE] = DatabaseConnection.readCoursesFromProgramme(programme);
                    if(coursesFromProgramme[(int)CommonInternals.Programmes.CSE] != null)
                    {
                        tempCoursesFromProgramme = coursesFromProgramme[(int)CommonInternals.Programmes.CSE];
                    }
                    courseTreeView = cseTreeView;
                }
                else if (programme.id.Equals(CommonInternals.PROG_EEE))
                {
                    coursesFromProgramme[(int)CommonInternals.Programmes.EEE] = DatabaseConnection.readCoursesFromProgramme(programme);
                    if (coursesFromProgramme[(int)CommonInternals.Programmes.EEE] != null)
                    {
                        tempCoursesFromProgramme = coursesFromProgramme[(int)CommonInternals.Programmes.EEE];
                    }
                    courseTreeView = eeeTreeView;
                }
                else if (programme.id.Equals(CommonInternals.PROG_SE))
                {
                    coursesFromProgramme[(int)CommonInternals.Programmes.SE] = DatabaseConnection.readCoursesFromProgramme(programme);
                    if (coursesFromProgramme[(int)CommonInternals.Programmes.SE] != null)
                    {
                        tempCoursesFromProgramme = coursesFromProgramme[(int)CommonInternals.Programmes.SE];
                    }
                    courseTreeView = seTreeView;
                }

                for (int i = 0; i < tempCoursesFromProgramme.Count; i++)
                {
                    TreeViewItem[] coursesFromProgrammeItems = new TreeViewItem[tempCoursesFromProgramme.Count];
                    if (coursesFromProgrammeItems[i] == null)
                    {
                        coursesFromProgrammeItems[i] = new TreeViewItem();
                    }

                    coursesFromProgrammeItems[i].IsExpanded = false;

                    // create stack panel
                    //
                    StackPanel stack = new StackPanel();
                    stack.Orientation = Orientation.Horizontal;

                    // create Image
                    //
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri(BasicManipulation.Properties.Resources.programmeIcon));

                    // Label
                    //
                    Label lbl = new Label();
                    lbl.Content = tempCoursesFromProgramme.ElementAt(i).id + " - " + tempCoursesFromProgramme.ElementAt(i).name;


                    // Add into stack
                    //
                    stack.Children.Add(image);
                    stack.Children.Add(lbl);

                    coursesFromProgrammeItems[i].Header = stack;
                    courseTreeView.Items.Add(coursesFromProgrammeItems[i]);
                }
            }
        }

        /// <summary>
        /// When a CSE course from the TreeView is selected.
        /// </summary>
        private void cseTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            seTreeView_SelectedItemChanged(sender, e);
        }

        /// <summary>
        /// When an EEE course from the TreeView is selected.
        /// </summary>
        private void eeeTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            seTreeView_SelectedItemChanged(sender, e);
        }

        /// <summary>
        /// When a SE course from the TreeView is selected.
        /// </summary>
        private void seTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = (TreeViewItem)e.NewValue;

            if (tvi != null)
            {
                tvi.IsExpanded = !tvi.IsExpanded;

                StackPanel itemHeader = (StackPanel)((TreeViewItem)((TreeView)sender).SelectedItem).Header;

                var courseId = itemHeader.Children.OfType<Label>().FirstOrDefault();

                // Split the label [COURSEID - COURSENAME]
                //
                char[] delimiter = {'-'};
                String[] selectedCourseName = courseId.Content.ToString().Split(delimiter);
                if (selectedCourseName[0] != null)
                {
                    selectedCourse = DatabaseConnection.getCourse(selectedCourseName[0].Trim());

                    // Make sure we have a valid course
                    //
                    if (selectedCourse != null)
                    {
                        isSetCareer = false;
                        isSetCourse = true;
                        isSetProgramme = false;

                        showProgrammesWindow();
                    }
                    else
                    {
                        Logger.Error("DepartmentWindow::seTreeView_SelectedItemChanged() Unknown course ID: " + selectedCourseName[0]);
                    }
                }
                else
                {
                    Logger.Error("DepartmentWindow::seTreeView_SelectedItemChanged() Unable to resolve course ID: " + selectedCourseName[0]);
                }
            }
        }

        /// <summary>
        /// When the "Learn More" button in the Programme button event is clicked.
        /// </summary>
        private void learnMoreFromProgramme_Click(object sender, RoutedEventArgs e)
        {
            isSetCareer = false;
            isSetCourse = false;
            isSetProgramme = true;

            if (selectedProgramme != null)
            {
                showProgrammesWindow();
            }
            else
            {
                Logger.Error("DepartmenWindow::learnMoreFromProgramme_Click() A single Programme was not selected.");
                MessageBox.Show("Please select a Programme from the list.", "Programme Selection", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
