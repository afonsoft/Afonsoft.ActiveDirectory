using System;
using System.DirectoryServices.ActiveDirectory;
using System.Text;

namespace Afonsoft.ActiveDirectory.Domain
{
    /// <summary>
    /// Recuperar informações do DomainName (LDAP)
    /// </summary>
    public static class DomainManager
    {
        static DomainManager()
        {
            RootSubPath = "";
            try
            {
                ComputerName = Environment.MachineName;
                System.DirectoryServices.ActiveDirectory.Domain domain = System.DirectoryServices.ActiveDirectory.Domain.GetCurrentDomain();
                DomainName = domain.Name;
                DomainController domainController = domain.PdcRoleOwner;
                DomainControllerName = domainController.Name.Split('.')[0];

                domain.Dispose();
                domainController.Dispose();
            }
            catch
            {
                DomainName = "ocean.one.com";
                DomainControllerName = "DC=ocean,DC=one,DC=com";
            }
        }

        /// <summary>
        /// Nome do controle do dominio
        /// </summary>
        public static string DomainControllerName { get; }

        /// <summary>
        /// Nome do Computador (OCSP1TI25)
        /// </summary>
        public static string ComputerName { get; }

        /// <summary>
        /// Nome do dominio (ocean.one.com)
        /// </summary>
        public static string DomainName { get; }

        /// <summary>
        /// Path do Dominio (DC=ocean,DC=one,DC=com)
        /// </summary>
        public static string DomainPath
        {
            get
            {
                bool bFirst = true;
                StringBuilder sbReturn = new StringBuilder(300);
                string[] strlstDc = DomainName.Split('.');
                foreach (string strDc in strlstDc)
                {
                    if (bFirst)
                    {
                        sbReturn.Append("DC=");
                        bFirst = false;
                    }
                    else
                        sbReturn.Append(",DC=");

                    sbReturn.Append(strDc);
                }
                return sbReturn.ToString();
            }
        }
        /// <summary>
        /// Incluir OU/CN antes do DC
        /// </summary>
        public static string RootSubPath { get; set; }
        /// <summary>
        /// Recuperar o caminho correto do DomainName/DomainPath
        /// string.Format("LDAP://{0}/{1}{2}", DomainName, (string.IsNullOrEmpty(RootSubPath) ? RootSubPath : RootSubPath + ","), DomainPath);
        /// LDAP://ocean.one.com/DC=ocean,DC=one,DC=com
        /// </summary>
        public static string RootPath
        {
            get
            {
                try
                {
                    return $"LDAP://{DomainName}/{(string.IsNullOrEmpty(RootSubPath) ? RootSubPath : RootSubPath + ",")}{DomainPath}";
                }
                catch
                {
                    return "LDAP://ocean.one.com/DC=ocean,DC=one,DC=com";
                }
            }
        }
    }
}