using Afonsoft.ActiveDirectory.Domain;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace Afonsoft.ActiveDirectory
{
   public class ActiveDirectoryOptions
    {

        private string _ldap = DomainManager.RootPath;
        /// <summary>
        /// The maximum number of objects the server can return in a paged search
        /// </summary>
        public int PageSize { get; set; } = 10000;
        /// <summary>
        /// true if the result is cached on the client computer; otherwise, false
        /// </summary>
        public bool CacheResults { get; set; } = true;
        /// <summary>
        /// true if the search is asynchronous; false otherwise.
        /// </summary>
        public bool Asynchronous { get; set; } = true;
        /// <summary>
        ///  Gets or sets a value indicating the scope of the search that is observed by the server.
        /// </summary>
        public SearchScope SearchScope { get; set; } = SearchScope.Subtree;
        /// <summary>
        /// Gets or sets the maximum amount of time that the client waits for the server
        //     to return results. If the server does not respond within this time, the search
        //     is aborted and no results are returned.
        /// </summary>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        ///  enumeration specifies the types of authentication used 
        /// </summary>
        public AuthenticationTypes AuthenticationTypes { get; set; } = AuthenticationTypes.Secure;

        /// <summary>
        /// LDAP = Url do LDAP Ex: LDAP://ocean.one.com/DC=ocean,DC=one,DC=com
        /// </summary>
        public string LDAP
        {
            get { return _ldap; }
            set { _ldap = FixLdap(value); }
        }

        /// <summary>
        /// Usuário do LDAP
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string LDAP_USER { get; set; } = "";

        /// <summary>
        /// Senha do usuário do LDAP
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string LDAP_PASS { get; set; } = "";

        private string FixLdap(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return DomainManager.RootPath;

            if (!value.Contains("LDAP://"))
            {
                DomainManager.DomainName = value;
                value = DomainManager.RootPath;
            }
            
            return value;
        }
    }
}
