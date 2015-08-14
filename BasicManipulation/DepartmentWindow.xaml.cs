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

        public DepartmentWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;

            // Let's hide the components on the right for now
            //
            careerLabel.Text = CommonInternals.ECE_HEADER1;
            programmeToStudy.Text = CommonInternals.ECE_HEADER2;
            eceWelcomeTextBlock.Text = CommonInternals.ECE_LABEL1;
            programmeToStudyTextBlock.Text = CommonInternals.ECE_LABEL2;
            learnMoreFromCareer.Visibility = Visibility.Collapsed;
            learnMoreFromProgramme.Visibility = Visibility.Collapsed;
            careerTreeView.Visibility = Visibility.Collapsed;
            programmeTreeView.Visibility = Visibility.Collapsed;
            coursesGrid.Visibility = Visibility.Collapsed;
            cseLabel.Visibility = Visibility.Collapsed;
            eeeLabel.Visibility = Visibility.Collapsed;
            seLabel.Visibility = Visibility.Collapsed;
            
            careers = new List<Career>();
            programmes = new List<Programme>();
            courses = new List<Course>();

            DatabaseConnection.readCareers(careers);
            DatabaseConnection.readProgrammes(programmes);
            DatabaseConnection.readCourses(courses);

            isSetCareer = false;
            isSetCourse = false;
            isSetProgramme = false;
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            if (mainWindow != null)
            {
                mainWindow.Show();
            }
        }

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
                image.Source = new BitmapImage(new Uri("C:\\Users\\ebag753\\Documents\\Visual Studio 2013\\Projects\\BasicManipulation\\BasicManipulation\\bin\\Debug\\careers_icon.png", UriKind.RelativeOrAbsolute));

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
                image.Source = new BitmapImage(new Uri("C:\\Users\\ebag753\\Documents\\Visual Studio 2013\\Projects\\BasicManipulation\\BasicManipulation\\bin\\Debug\\prog_icon.png", UriKind.RelativeOrAbsolute));

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

        private void showCareerComponents(bool show)
        {
            if(show == true)
            {
                careerLabel.Visibility = Visibility.Visible;
                careerTreeView.Visibility = Visibility.Visible;
                programmeToStudy.Visibility = Visibility.Visible;
                programmeToStudyCanvass.Visibility = Visibility.Visible;
            }
            else
            {
                careerLabel.Visibility = Visibility.Collapsed;
                careerTreeView.Visibility = Visibility.Collapsed;
                programmeToStudy.Visibility = Visibility.Collapsed;
                programmeToStudyCanvass.Visibility = Visibility.Collapsed;
            }
        }

        private void careerPathwayButton_Click(object sender, RoutedEventArgs e)
        {
            careerLabel.Text = CommonInternals.ECE_HEADER4;
            programmeToStudyTextBlock.Visibility = Visibility.Collapsed;
            eceWelcomeTextBlock.Visibility = Visibility.Collapsed;
            programmeToStudy.Text = CommonInternals.ECE_HEADER5;
            cseLabel.Visibility = Visibility.Collapsed;
            eeeLabel.Visibility = Visibility.Collapsed;
            seLabel.Visibility = Visibility.Collapsed;
            coursesGrid.Visibility = Visibility.Collapsed;
            learnMoreFromCareer.Visibility = Visibility.Collapsed;
            learnMoreFromProgramme.Visibility = Visibility.Collapsed;
            programmeTreeView.Visibility = Visibility.Collapsed;

            careerTreeView.Visibility = Visibility.Visible;
            careerLabel.Visibility = Visibility.Visible;
            programmeToStudy.Visibility = Visibility.Visible;
            
            generateCareersTreeView();
        }

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
                    if (programme.name == programmeLabel.Content)
                    {
                        programmeToStudyTextBlock.Visibility = Visibility.Visible;
                        learnMoreFromCareer.Visibility = Visibility.Collapsed;
                        learnMoreFromProgramme.Visibility = Visibility.Visible;
                        programmeToStudyTextBlock.Text = DatabaseConnection.readProgrammeName(programme.id) + " (" + programme.id + ")" + "\n\n"
                            + DatabaseConnection.readProgrammeDescription(programme.id);

                        selectedProgramme = programme;
                        break;
                    }
                }
            }
        }

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
                    if (career.name == careerLabel.Content)
                    {
                        selectedCareer = null;
                        learnMoreFromCareer.Visibility = Visibility.Visible;
                        programmeToStudyTextBlock.Visibility = Visibility.Visible;
                        programmeToStudyTextBlock.Text = career.description + "\n\n\nFinal courses to take:\n";

                        List<String> finalCoursesToTake = DatabaseConnection.readCareerFinalCourses(career);
                        for (int i = 0; i < finalCoursesToTake.Count; i++)
                        {
                            programmeToStudyTextBlock.Text += finalCoursesToTake.ElementAt(i) + "\n";
                        }

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

        private void programmesButton_Click(object sender, RoutedEventArgs e)
        {
            careerLabel.Text = CommonInternals.ECE_HEADER3;
            programmeToStudyTextBlock.Visibility = Visibility.Collapsed;
            eceWelcomeTextBlock.Visibility = Visibility.Collapsed;
            learnMoreFromCareer.Visibility = Visibility.Collapsed;
            cseLabel.Visibility = Visibility.Collapsed;
            eeeLabel.Visibility = Visibility.Collapsed;
            seLabel.Visibility = Visibility.Collapsed;
            coursesGrid.Visibility = Visibility.Collapsed;
            careerTreeView.Visibility = Visibility.Collapsed;
            learnMoreFromCareer.Visibility = Visibility.Collapsed;
            learnMoreFromProgramme.Visibility = Visibility.Collapsed;
            programmeToStudy.Text = "";

            programmeTreeView.Visibility = Visibility.Visible;
            careerLabel.Visibility = Visibility.Visible;
            programmeToStudy.Visibility = Visibility.Visible;
            generateProgrammesTreeView();
        }

        private void learnMoreFromCareer_Click(object sender, RoutedEventArgs e)
        {
            isSetCareer = true;
            isSetCourse = false;
            isSetProgramme = false;

            showProgrammesWindow();
        }

        private void showProgrammesWindow()
        {
            ProgrammeWindow programmeWindow = new ProgrammeWindow(this);
            Hide();
            programmeWindow.Show();
        }

        private void coursesButton_Click(object sender, RoutedEventArgs e)
        {
            programmeToStudyTextBlock.Visibility = Visibility.Collapsed;
            eceWelcomeTextBlock.Visibility = Visibility.Collapsed;
            careerLabel.Visibility = Visibility.Collapsed;
            programmeToStudy.Visibility = Visibility.Collapsed;
            careerTreeView.Visibility = Visibility.Collapsed;
            programmeTreeView.Visibility = Visibility.Collapsed;
            learnMoreFromCareer.Visibility = Visibility.Collapsed;
            learnMoreFromProgramme.Visibility = Visibility.Collapsed;

            cseLabel.Visibility = Visibility.Visible;
            eeeLabel.Visibility = Visibility.Visible;
            seLabel.Visibility = Visibility.Visible;
            coursesGrid.Visibility = Visibility.Visible;

            // Generate course for each programme
            //
            generateCoursesTreeView();
        }

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
                    image.Source = new BitmapImage(new Uri("C:\\Users\\ebag753\\Documents\\Visual Studio 2013\\Projects\\BasicManipulation\\BasicManipulation\\bin\\Debug\\prog_icon.png", UriKind.RelativeOrAbsolute));

                    // Label
                    //
                    Label lbl = new Label();
                    lbl.Content = tempCoursesFromProgramme.ElementAt(i).id;


                    // Add into stack
                    //
                    stack.Children.Add(image);
                    stack.Children.Add(lbl);

                    coursesFromProgrammeItems[i].Header = stack;
                    courseTreeView.Items.Add(coursesFromProgrammeItems[i]);
                }
            }
        }

        private void cseTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            seTreeView_SelectedItemChanged(sender, e);
        }

        private void eeeTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            seTreeView_SelectedItemChanged(sender, e);
        }

        private void seTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem tvi = (TreeViewItem)e.NewValue;

            if (tvi != null)
            {
                tvi.IsExpanded = !tvi.IsExpanded;

                StackPanel itemHeader = (StackPanel)((TreeViewItem)((TreeView)sender).SelectedItem).Header;

                var courseId = itemHeader.Children.OfType<Label>().FirstOrDefault();
                selectedCourse = DatabaseConnection.getCourse(courseId.Content.ToString());

                isSetCareer = false;
                isSetCourse = true;
                isSetProgramme = false;

                showProgrammesWindow();
            }
        }

        private void learnMoreFromProgramme_Click(object sender, RoutedEventArgs e)
        {
            isSetCareer = false;
            isSetCourse = false;
            isSetProgramme = true;

            showProgrammesWindow();
        }
    }
}
