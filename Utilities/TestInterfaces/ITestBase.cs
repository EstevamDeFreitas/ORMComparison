using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrmUtilities
{
    public interface ITestBase
    {

        //Specific Tests
        public void RunInsertStudent();
        public void RunUpdateStudent();
        public void RunDeleteStudent();
        public void RunGetStudent();

        public void RunInsertTeacher();
        public void RunUpdateTeacher();
        public void RunDeleteTeacher();
        public void RunGetTeacher();

        public void Setup();
        public void Cleanup();
    }
}
