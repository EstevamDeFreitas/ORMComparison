using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrmUtilities
{
    public interface ITestBase
    {

        //Specific Tests

        #region Student
        /*foreach Student
            create Address; 
            create Person;
                    add Address to Person
            create Student;
                    add Person to Student
            save Student;*/
        public void RunInsertStudent();

        /*get Students(with Person and Address)
            foreach Student
            change Address; update Student*/
        public void RunUpdateStudent();
        /*get Students and related entities;
        for each Student
        delete Address; delete Person; delete Students;
                end;*/
        public void RunDeleteStudent();
        /*get Students(including Person and Address);*/
        public void RunGetStudent();
        #endregion

        #region Teacher
        /*foreach teacher 
            create Address; 
            create Person;
                add Address;
            create Teacher;
                add Person;
            create Courses;
                add Courses to Teacher;
                save teacher;*/

        public void RunInsertTeacher();
        /*get Teachers;
        foreach Teacher get Courses;
        foreach Teacher;
        update Address;
                update Course.Description; update Teacher;*/
        public void RunUpdateTeacher();
        /*get Teachers and related entities;
        foreach Teacher
        delete Address; delete Person;
                delete Teacher; delete Courses(if any);
                end;*/
        public void RunDeleteTeacher();

        /*get Teachers(including Person and Address);
        get Courses;*/
        public void RunGetTeacher();
        #endregion

        public void Setup();
        public void Cleanup();
    }
}
