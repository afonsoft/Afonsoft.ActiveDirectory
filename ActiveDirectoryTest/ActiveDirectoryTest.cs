using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActiveDirectoryTest
{
    [TestClass]
    public class ActiveDirectoryTest
    {
        [TestMethod]
        public void TestActiveDirectory1()
        {
            Afonsoft.ActiveDirectory.ActiveDirectory ad = new Afonsoft.ActiveDirectory.ActiveDirectory();
            bool result = ad.CheckConnection();
            if (result)
            {

            }
        }
        [TestMethod]
        public void TestActiveDirectory2()
        {
            Afonsoft.ActiveDirectory.ActiveDirectory ad = new Afonsoft.ActiveDirectory.ActiveDirectory(o=> { o.LDAP = "sede.gol.com"; o.LDAP_USER = "mzt.adnfilho"; o.LDAP_PASS = "Senha#2019"; });
            bool result = ad.CheckConnection();
            if (result)
            {

            }
        }
    }
}
