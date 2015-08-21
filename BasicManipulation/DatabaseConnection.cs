using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BasicManipulation
{
    public static class DatabaseConnection
    {
        private static String dbConnectioName = null;
        private static SQLiteConnection dbConnection = null;
        private static SQLiteCommand sqlCommand = null;
        private static SQLiteDataReader dataReader = null;

        static DatabaseConnection()
        {
            try
            {
                dbConnectioName = "Data Source=" + CommonInternals.DB_NAME + ";Version=3;";
                dbConnection = new SQLiteConnection(dbConnectioName);
            }
            catch (Exception ex)
            {
                Logger.Error("DatabaseConnection::DatabaseConnection() " + ex.Message);
            }
        }

        public static void executeQuery(string queryStatement)
        {
            try
            {
                dbConnection.Open();
                sqlCommand = dbConnection.CreateCommand();
                sqlCommand.CommandText = queryStatement;
                sqlCommand.ExecuteNonQuery();
                dbConnection.Close();
            }
            catch(Exception ex)
            {
                Logger.Error("DatabaseConnection::executeQuery() " + ex.Message);
            }
        }

        public static void readCareers(List<Career> careers)
        {
            String readCareerTable = "SELECT * FROM Career";

            try
            {
                careers.Clear();
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readCareerTable, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    Career career = new Career(dataReader["ID"].ToString(), dataReader["NAME"].ToString(), dataReader["DESC"].ToString());
                    careers.Add(career);
                    Logger.Info("ID: " + career.id + " NAME: " + career.name + " DESC: " + career.description);
                }
                dbConnection.Close();
            }
            catch(Exception ex)
            {
                Logger.Error("DatabaseConnection::readCareers() " + ex.Message);
            }
        }

        public static List<String> readCareerJobs(Career career)
        {
            List<String> jobs = new List<string>();
            String readJobsTable = "SELECT NAME FROM Job WHERE CAREERID ='" + career.id + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readJobsTable, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    String job = dataReader["NAME"].ToString();
                    jobs.Add(job);
                    Logger.Debug("DatabaseConnection::readCareerJobs() career = " + career.id + " job = " + job);
                }
                dbConnection.Close();
            }
            catch(Exception e)
            {
                Logger.Error("DatabaseConnection::readCareerJobs() " +  e.Message);
            }

            return jobs;
        }

        public static List<Course> readCareerFinalCourses(Career career)
        {
            List<Course> courses = new List<Course>();
            String readJobsTable = "SELECT COURSEID FROM FinalCourse WHERE CAREERID ='" + career.id + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readJobsTable, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    String finalCourseId = dataReader["COURSEID"].ToString();
                    String readFinalCourse = "SELECT * FROM Course WHERE ID = '" + finalCourseId + "'";

                    SQLiteCommand sqlCommandTemp = new SQLiteCommand(readFinalCourse, dbConnection);
                    SQLiteDataReader dataReaderTemp = sqlCommandTemp.ExecuteReader();
                    Course finalCourse = null;
                    while (dataReaderTemp.Read())
                    {
                        finalCourse = new Course(dataReaderTemp["ID"].ToString(),
                                (int)dataReaderTemp["YR"],
                                (int)dataReaderTemp["SEM"],
                                dataReaderTemp["NAME"].ToString(),
                                dataReaderTemp["DESC"].ToString(),
                                (int)dataReaderTemp["POINTS"],
                                dataReaderTemp["ACADEMICORG"].ToString(),
                                dataReaderTemp["ACADEMICGROUP"].ToString(),
                                dataReaderTemp["COURSECOMP"].ToString(),
                                dataReaderTemp["GRADINGBASIS"].ToString(),
                                dataReaderTemp["TYPOFFERED"].ToString(),
                                dataReaderTemp["REMARKS"].ToString(),
                                dataReaderTemp["CAREERID"].ToString());
                    }
                    if (finalCourse != null)
                    {
                        Logger.Info("[DatabaseConnection::readCareerFinalCourses()] PreRequisite Course Information:");
                        Logger.Info("[DatabaseConnection::readCareerFinalCourses()]" + "\nID: " + finalCourse.id + "\nNAME: " + finalCourse.name + "\nDESC: " + finalCourse.description);
                        courses.Add(finalCourse);
                    }
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::readCareerFinalCourses() " + e.Message);
            }

            return courses;
        }

        public static void readProgrammes(List<Programme> programmes)
        {
            String readProgrammeStatement = "SELECT * FROM Programme";

            try
            {
                programmes.Clear();
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readProgrammeStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    Programme programme = new Programme(dataReader["ID"].ToString(), dataReader["NAME"].ToString(), dataReader["DESC"].ToString(), dataReader["COURSEID"].ToString());
                    programmes.Add(programme);
                    Logger.Info("ID: " + programme.id + " NAME: " + programme.name + " DESC: " + programme.description + " COURSEID: " + programme.courseId);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::readProgrammes() " + e.Message);
            }
        }

        public static String readProgrammeName(String programmeId)
        {
            String progName = null;
            String readProgrammeStatement = "SELECT NAME FROM Programme WHERE ID = '" + programmeId + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readProgrammeStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();
                progName = dataReader.Read() ? dataReader.GetString(0) : "";
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::readProgrammeName() " + e.Message);
            }

            return progName;
        }

        public static String readProgrammeDescription(String programmeId)
        {
            String progName = null;
            String readProgrammeStatement = "SELECT DESC FROM Programme WHERE ID = '" + programmeId + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readProgrammeStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();
                progName = dataReader.Read() ? dataReader.GetString(0) : "";
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::readProgrammeDescription() " + e.Message);
            }

            return progName;
        }

        public static void readCourses(List<Course> courses)
        {
            String readCourseTable = "SELECT * FROM Course";

            courses.Clear();

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readCourseTable, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    Course course = new Course(dataReader["ID"].ToString(),
                        (int)dataReader["YR"],
                        (int)dataReader["SEM"],
                        dataReader["NAME"].ToString(),
                        dataReader["DESC"].ToString(),
                        (int)dataReader["POINTS"],
                        dataReader["ACADEMICORG"].ToString(),
                        dataReader["ACADEMICGROUP"].ToString(),
                        dataReader["COURSECOMP"].ToString(),
                        dataReader["GRADINGBASIS"].ToString(),
                        dataReader["TYPOFFERED"].ToString(),
                        dataReader["REMARKS"].ToString(),
                        dataReader["CAREERID"].ToString());
                    courses.Add(course);
                    Logger.Info("ID: " + course.id + " NAME: " + course.name + " DESC: " + course.description);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::readCourses() " + e.Message);
            }
        }

        public static Course getCourse(String courseId)
        {
            Course course = null;
            String readCourseTable = "SELECT * FROM Course WHERE ID  = '" + courseId + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readCourseTable, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    course = new Course(dataReader["ID"].ToString(),
                        (int)dataReader["YR"],
                        (int)dataReader["SEM"],
                        dataReader["NAME"].ToString(),
                        dataReader["DESC"].ToString(),
                        (int)dataReader["POINTS"],
                        dataReader["ACADEMICORG"].ToString(),
                        dataReader["ACADEMICGROUP"].ToString(),
                        dataReader["COURSECOMP"].ToString(),
                        dataReader["GRADINGBASIS"].ToString(),
                        dataReader["TYPOFFERED"].ToString(),
                        dataReader["REMARKS"].ToString(),
                        dataReader["CAREERID"].ToString());
                    Logger.Info("[DatabaseConnection::getCourse()]" + "\nID: " + course.id + "\nNAME: " + course.name + "\nDESC: " + course.description);
                }

                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::getCourse() " + e.Message);
            }

            return course;
        }

        public static List<Course> generateAllRelatedPreRequisites(Course course)
        {
            List<Course> preRequisiteCourses = new List<Course>();
            String queryStatement = @"WITH RECURSIVE
  pre_req_courses(n) AS (
    VALUES('" + course.id + @"')
    UNION
    SELECT COURSEID FROM PreRequisite, pre_req_courses
     WHERE PreRequisite.FOLLOWID=pre_req_courses.n
  )
SELECT COURSEID FROM PreRequisite
 WHERE PreRequisite.FOLLOWID IN pre_req_courses";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(queryStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    String preReqCourseId = dataReader["COURSEID"].ToString();
                    String readPreReqCourse = "SELECT * FROM Course WHERE ID = '" + preReqCourseId + "'";

                    SQLiteCommand sqlCommandTemp = new SQLiteCommand(readPreReqCourse, dbConnection);
                    SQLiteDataReader dataReaderTemp = sqlCommandTemp.ExecuteReader();
                    Course preReq = null;
                    while (dataReaderTemp.Read())
                    {
                        preReq = new Course(dataReaderTemp["ID"].ToString(),
                                (int)dataReaderTemp["YR"],
                                (int)dataReaderTemp["SEM"],
                                dataReaderTemp["NAME"].ToString(),
                                dataReaderTemp["DESC"].ToString(),
                                (int)dataReaderTemp["POINTS"],
                                dataReaderTemp["ACADEMICORG"].ToString(),
                                dataReaderTemp["ACADEMICGROUP"].ToString(),
                                dataReaderTemp["COURSECOMP"].ToString(),
                                dataReaderTemp["GRADINGBASIS"].ToString(),
                                dataReaderTemp["TYPOFFERED"].ToString(),
                                dataReaderTemp["REMARKS"].ToString(),
                                dataReaderTemp["CAREERID"].ToString());
                    }
                    if (preReq != null)
                    {
                        Logger.Info("[DatabaseConnection::getPrerequisiteCourse()] PreRequisite Course Information:");
                        Logger.Info("[DatabaseConnection::getPrerequisiteCourse()]" + "\nID: " + preReq.id + "\nNAME: " + preReq.name + "\nDESC: " + preReq.description);
                        preRequisiteCourses.Add(preReq);
                    }
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::generateAllRelatedPreRequisites() " + e.Message);
            }

            return preRequisiteCourses;
        }

        public static List<Course> getPrerequisiteCourses(String courseId)
        {
            List<Course> preRequisiteCourses = new List<Course>();
            String readPreReqs = "SELECT * FROM Course WHERE ID = '" + courseId + "'";
            Course targetCourse = null;

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readPreReqs, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    targetCourse = new Course(dataReader["ID"].ToString(),
                        (int)dataReader["YR"],
                        (int)dataReader["SEM"],
                        dataReader["NAME"].ToString(),
                        dataReader["DESC"].ToString(),
                        (int)dataReader["POINTS"],
                        dataReader["ACADEMICORG"].ToString(),
                        dataReader["ACADEMICGROUP"].ToString(),
                        dataReader["COURSECOMP"].ToString(),
                        dataReader["GRADINGBASIS"].ToString(),
                        dataReader["TYPOFFERED"].ToString(),
                        dataReader["REMARKS"].ToString(),
                        dataReader["CAREERID"].ToString());

                    Logger.Info("[DatabaseConnection::getPrerequisiteCourse()] Target Course Information:");
                    Logger.Info("[DatabaseConnection::getPrerequisiteCourse()]" + "\nID: " + targetCourse.id + "\nNAME: " + targetCourse.name + "\nDESC: " + targetCourse.description);
                }

                if (targetCourse != null)
                {
                    String readPreReqInfo = "SELECT * FROM PreRequisite WHERE FOLLOWID = '" + targetCourse.id + "'";

                    sqlCommand = new SQLiteCommand(readPreReqInfo, dbConnection);
                    dataReader = sqlCommand.ExecuteReader();

                    while (dataReader.Read())
                    {
                        String preReqCourseId = dataReader["COURSEID"].ToString();
                        String readPreReqCourse = "SELECT * FROM Course WHERE ID = '" + preReqCourseId + "'";

                        SQLiteCommand sqlCommandTemp = new SQLiteCommand(readPreReqCourse, dbConnection);
                        SQLiteDataReader dataReaderTemp = sqlCommandTemp.ExecuteReader();

                        while (dataReaderTemp.Read())
                        {
                            Course preReq = new Course(dataReaderTemp["ID"].ToString(),
                                (int)dataReaderTemp["YR"],
                                (int)dataReaderTemp["SEM"],
                                dataReaderTemp["NAME"].ToString(),
                                dataReaderTemp["DESC"].ToString(),
                                (int)dataReaderTemp["POINTS"],
                                dataReaderTemp["ACADEMICORG"].ToString(),
                                dataReaderTemp["ACADEMICGROUP"].ToString(),
                                dataReaderTemp["COURSECOMP"].ToString(),
                                dataReaderTemp["GRADINGBASIS"].ToString(),
                                dataReaderTemp["TYPOFFERED"].ToString(),
                                dataReaderTemp["REMARKS"].ToString(),
                                dataReaderTemp["CAREERID"].ToString());

                            Logger.Info("[DatabaseConnection::getPrerequisiteCourse()] PreRequisite Course Information:");
                            Logger.Info("[DatabaseConnection::getPrerequisiteCourse()]" + "\nID: " + preReq.id + "\nNAME: " + preReq.name + "\nDESC: " + preReq.description);
                            preRequisiteCourses.Add(preReq);
                        }
                    }
                }
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("DatabaseConnection::getPrerequisiteCourse() " + ex.Message);
            }

            return preRequisiteCourses;
        }

        public static List<Course> readCoursesFromProgramme(Programme programme)
        {
            List<Course> courses = new List<Course>();
            String readCSECoursesStatement = "SELECT COURSEID FROM CourseProgrammePart WHERE PROGRAMMEID = '" + programme.id + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readCSECoursesStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    String cseCourseId = dataReader["COURSEID"].ToString();
                    String readCSECourse = "SELECT * FROM Course WHERE ID = '" + cseCourseId + "'";

                    SQLiteCommand sqlCommandTemp = new SQLiteCommand(readCSECourse, dbConnection);
                    SQLiteDataReader dataReaderTemp = sqlCommandTemp.ExecuteReader();

                    while (dataReaderTemp.Read())
                    {
                        Course cseCourse = new Course(dataReaderTemp["ID"].ToString(),
                                (int)dataReaderTemp["YR"],
                                (int)dataReaderTemp["SEM"],
                                dataReaderTemp["NAME"].ToString(),
                                dataReaderTemp["DESC"].ToString(),
                                (int)dataReaderTemp["POINTS"],
                                dataReaderTemp["ACADEMICORG"].ToString(),
                                dataReaderTemp["ACADEMICGROUP"].ToString(),
                                dataReaderTemp["COURSECOMP"].ToString(),
                                dataReaderTemp["GRADINGBASIS"].ToString(),
                                dataReaderTemp["TYPOFFERED"].ToString(),
                                dataReaderTemp["REMARKS"].ToString(),
                                dataReaderTemp["CAREERID"].ToString());

                        Logger.Info("[DatabaseConnection::readCoursesFromProgramme()] PreRequisite Course Information:");
                        Logger.Info("[DatabaseConnection::readCoursesFromProgramme()]" + "\nID: " + cseCourse.id + "\nNAME: " + cseCourse.name + "\nDESC: " + cseCourse.description);
                        courses.Add(cseCourse);
                    }
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::readCoursesFromProgramme() " + e.Message);
            }

            return courses;
        }

        public static List<Course> readCoursesUsingProgrammePart(Programme programme, String part)
        {
            List<Course> courses = new List<Course>();
            String readCoursesStatement = "SELECT COURSEID FROM CourseProgrammePart WHERE PROGRAMMEID = '" + programme.id + "' AND PART = '" + part + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readCoursesStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    String cseCourseId = dataReader["COURSEID"].ToString();
                    String readCSECourse = "SELECT * FROM Course WHERE ID = '" + cseCourseId + "'";

                    SQLiteCommand sqlCommandTemp = new SQLiteCommand(readCSECourse, dbConnection);
                    SQLiteDataReader dataReaderTemp = sqlCommandTemp.ExecuteReader();

                    while (dataReaderTemp.Read())
                    {
                        Course cseCourse = new Course(dataReaderTemp["ID"].ToString(),
                                (int)dataReaderTemp["YR"],
                                (int)dataReaderTemp["SEM"],
                                dataReaderTemp["NAME"].ToString(),
                                dataReaderTemp["DESC"].ToString(),
                                (int)dataReaderTemp["POINTS"],
                                dataReaderTemp["ACADEMICORG"].ToString(),
                                dataReaderTemp["ACADEMICGROUP"].ToString(),
                                dataReaderTemp["COURSECOMP"].ToString(),
                                dataReaderTemp["GRADINGBASIS"].ToString(),
                                dataReaderTemp["TYPOFFERED"].ToString(),
                                dataReaderTemp["REMARKS"].ToString(),
                                dataReaderTemp["CAREERID"].ToString());

                        Logger.Info("[DatabaseConnection::readCoursesUsingProgrammePart()] PreRequisite Course Information:");
                        Logger.Info("[DatabaseConnection::readCoursesUsingProgrammePart()]" + "\nID: " + cseCourse.id + "\nNAME: " + cseCourse.name + "\nDESC: " + cseCourse.description);
                        courses.Add(cseCourse);
                    }
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::readCoursesUsingProgrammePart() " + e.Message);
            }

            return courses;
        }

        public static List<CourseProgrammePart> readCourseProgrammePart(Programme programme)
        {
            List<CourseProgrammePart> courseProgrammePartList = new List<CourseProgrammePart>();
            String readCSECoursesStatement = "SELECT * FROM CourseProgrammePart WHERE PROGRAMMEID = '" + programme.id + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readCSECoursesStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    CourseProgrammePart courseProgrammePart = new CourseProgrammePart(dataReader["COURSEID"].ToString(), dataReader["PART"].ToString(), dataReader["PROGRAMMEID"].ToString());
                    courseProgrammePartList.Add(courseProgrammePart);
                }
                dbConnection.Close();
            }
            catch (Exception e)
            {
                Logger.Error("DatabaseConnection::readCourseProgrammePart() " + e.Message);
            }

            return courseProgrammePartList;
        }

        public static List<Course> getRestrictionCourses(String courseId)
        {
            List<Course> restrictionList = new List<Course>();
            String readPreReqs = "SELECT RESTRCOURSEID FROM Restrictions WHERE COURSEID = '" + courseId + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(readPreReqs, dbConnection);
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    String restrCourseId = dataReader["RESTRCOURSEID"].ToString();
                    String readCourse = "SELECT * FROM Course WHERE ID = '" + restrCourseId + "'";

                    SQLiteCommand sqlCommandTemp = new SQLiteCommand(readCourse, dbConnection);
                    SQLiteDataReader dataReaderTemp = sqlCommandTemp.ExecuteReader();

                    while (dataReaderTemp.Read())
                    {
                        Course restrCourse = new Course(dataReaderTemp["ID"].ToString(),
                                (int)dataReaderTemp["YR"],
                                (int)dataReaderTemp["SEM"],
                                dataReaderTemp["NAME"].ToString(),
                                dataReaderTemp["DESC"].ToString(),
                                (int)dataReaderTemp["POINTS"],
                                dataReaderTemp["ACADEMICORG"].ToString(),
                                dataReaderTemp["ACADEMICGROUP"].ToString(),
                                dataReaderTemp["COURSECOMP"].ToString(),
                                dataReaderTemp["GRADINGBASIS"].ToString(),
                                dataReaderTemp["TYPOFFERED"].ToString(),
                                dataReaderTemp["REMARKS"].ToString(),
                                dataReaderTemp["CAREERID"].ToString());

                        Logger.Info("[DatabaseConnection::getRestrictionCourses()] Restricted Course Information:");
                        Logger.Info("[DatabaseConnection::getRestrictionCourses()]" + "\nID: " + restrCourse.id + "\nNAME: " + restrCourse.name + "\nDESC: " + restrCourse.description);
                        restrictionList.Add(restrCourse);
                    }
                }
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("DatabaseConnection::getRestrictionCourses() " + ex.Message);
            }

            return restrictionList;
        }

        public static bool isCourseCompulsory(Course targetCourse)
        {
            bool isCompulsory = false;

            String queryStatement = "SELECT * FROM CompulsoryCourse WHERE COURSEID = '" + targetCourse.id + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(queryStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();
                isCompulsory = dataReader.Read() ? true : false;
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("DatabaseConnection::isCourseCompulsory() " + ex.Message);
            }

            return isCompulsory;
        }

        public static bool isCourseElective(Course targetCourse)
        {
            bool isElective = false;

            String queryStatement = "SELECT * FROM ElectiveCourse WHERE COURSEID = '" + targetCourse.id + "'";

            try
            {
                dbConnection.Open();
                sqlCommand = new SQLiteCommand(queryStatement, dbConnection);
                dataReader = sqlCommand.ExecuteReader();
                isElective = dataReader.Read() ? true : false;
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("DatabaseConnection::isCourseElective() " + ex.Message);
            }

            return isElective;
        }
    }
}
