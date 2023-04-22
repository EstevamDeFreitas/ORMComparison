using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.TestInterfaces
{
    public interface ITestBase
    {
        public void RunInsertTest(int testAmount);
        public void RunUpdateTest(int testAmount);
        public void RunDeleteTest(int testAmount);
        public void RunGetTest(int testAmount);
    }
}
