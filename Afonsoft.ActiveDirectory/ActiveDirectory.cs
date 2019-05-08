using System;
using System.DirectoryServices;
using Afonsoft.ActiveDirectory.Domain;

namespace Afonsoft.ActiveDirectory
{
    /// <summary>
    /// Classe para controle e acesso do ActiveDirectory
    /// Referencia da Class (http://www.codeproject.com/Articles/18102/Howto-Almost-Everything-In-Active-Directory-via-C#32)
    /// </summary>
    public sealed class ActiveDirectory : IDisposable
    {

        #region Variaveis

        /// <summary>
        /// LDAP = Url do LDAP Ex: LDAP://ocean.one.com/DC=ocean,DC=one,DC=com
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string LDAP { get; private set; }

        /// <summary>
        /// Usuário do LDAP
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string LDAP_USER { get; private set; } = "";

        /// <summary>
        /// Senha do usuário do LDAP
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string LDAP_PASS { get; private set; } = "";


        /// <summary>
        /// Default propertiesToLoad
        /// </summary>
        private string[] PropertiesToLoad { get; } = new string[] { "cn", "OU", "UserAccountControl", "WhenCreated", "DistinguishedName", "sAMAccountName" };


        #endregion

        #region ActiveDirectory

        /// <summary>
        /// ActiveDirectory
        /// </summary>
        /// <param name="ldap">LDAP - Default: DomainManager.RootPath</param>
        /// <param name="ldapUser">Usuário</param>
        /// <param name="ldapPass">Senha</param>
        public ActiveDirectory(string ldap, string ldapUser, string ldapPass)
        {
            LDAP = ldap;
            LDAP_USER = ldapUser;
            LDAP_PASS = ldapPass;

            if (string.IsNullOrEmpty(LDAP))
                LDAP = DomainManager.RootPath;

            if (string.IsNullOrEmpty(LDAP_USER))
                throw new ArgumentNullException(nameof(LDAP_USER));

            if (string.IsNullOrEmpty(LDAP_PASS))
                throw new ArgumentNullException(nameof(LDAP_PASS));

            if (!Login(LDAP_USER, LDAP_PASS))
                LDAP = DomainManager.RootPath;

            if (!Login(LDAP_USER, LDAP_PASS))
                throw new Exception("Não foi possivel efetuar uma conexão no Active Directory (AD) com o usuário informado. " + Environment.NewLine + " LDAP: " + LDAP + Environment.NewLine + " USER: " + LDAP_USER);
        }

        /// <summary>
        /// ActiveDirectory - LDAP: DomainManager.RootPath
        /// </summary>
        /// <param name="ldapUser">Usuário</param>
        /// <param name="ldapPass">Senha</param>
        public ActiveDirectory(string ldapUser, string ldapPass)
        {
            LDAP_USER = ldapUser;
            LDAP_PASS = ldapPass;
            LDAP = DomainManager.RootPath;

            if (string.IsNullOrEmpty(LDAP_USER))
                throw new ArgumentNullException(nameof(LDAP_USER));

            if (string.IsNullOrEmpty(LDAP_PASS))
                throw new ArgumentNullException(nameof(LDAP_PASS));

            if (!Login(LDAP_USER, LDAP_PASS))
                throw new Exception("Não foi possivel efetuar uma conexão no Active Directory (AD) com o usuário informado. " + Environment.NewLine + " LDAP: " + LDAP + Environment.NewLine + " USER: " + LDAP_USER);
        }
        /// <summary>
        /// Construdor da classe.
        /// </summary>
        public ActiveDirectory()
        {

            LDAP = DomainManager.RootPath;

            if (!Login(LDAP_USER, LDAP_PASS))
                throw new Exception("Não foi possivel efetuar uma conexão no Active Directory (AD) com o usuário informado. " + Environment.NewLine + " LDAP: " + LDAP + Environment.NewLine + " USER: " + LDAP_USER);
        }
        #endregion

        #region getDirectoryEntry

        /// <summary>
        /// Recuperar o Diretorio padrão da DN
        /// </summary>
        private DirectoryEntry GetDirectoryEntry()
        {
            return new DirectoryEntry(LDAP, LDAP_USER, LDAP_PASS, AuthenticationTypes.Secure);
        }


        /// <summary>
        /// Recuperar o Diretorio da DN
        /// </summary>
        public DirectoryEntry GetDirectoryEntry(string ldap)
        {
            return new DirectoryEntry(ldap, LDAP_USER, LDAP_PASS, AuthenticationTypes.Secure);
        }
        #endregion

        #region Search

        /// <summary>
        /// Localizar um objeto no AD
        /// </summary>
        /// <param name="filter">(&amp;(objectClass=user)(objectCategory=person))(!(userAccountControl:1.2.840.113556.1.4.803:=2))</param>
        /// <param name="rootSubPath"></param>
        /// <param name="propertiesToLoad">"cn", "OU", "UserAccountControl", "WhenCreated", "DistinguishedName", "sAMAccountName"</param>
        /// <returns>SearchResultCollection</returns>
        public SearchResultCollection Search(string filter, string rootSubPath, params string[] propertiesToLoad)
        {
            DomainManager.RootSubPath = rootSubPath;
            string rootPath = DomainManager.RootPath.Replace(DomainManager.DomainPath, "").Replace(",,", ",") + DomainManager.DomainPath;
            DirectoryEntry de;
            try
            {
                de = new DirectoryEntry(rootPath, LDAP_USER, LDAP_PASS, AuthenticationTypes.Secure);
                if (de.Guid == Guid.Empty)
                {
                    DomainManager.RootSubPath = "";
                    de = new DirectoryEntry(DomainManager.RootPath, LDAP_USER, LDAP_PASS, AuthenticationTypes.Secure);
                }
            }
            catch
            {
                DomainManager.RootSubPath = "";
                de = new DirectoryEntry(DomainManager.RootPath, LDAP_USER, LDAP_PASS, AuthenticationTypes.Secure);
            }

            if (de.Guid == Guid.Empty)
                de = GetDirectoryEntry(LDAP);

            de.RefreshCache();

            if (propertiesToLoad == null || propertiesToLoad.Length <= 0)
                propertiesToLoad = PropertiesToLoad;

            using (DirectorySearcher deSearch = new DirectorySearcher(de))
            {
                deSearch.Filter = filter;
                deSearch.PageSize = 20000;
                deSearch.CacheResults = false;
                deSearch.Asynchronous = true;
                deSearch.SearchScope = SearchScope.Subtree;
                deSearch.ClientTimeout = TimeSpan.FromMinutes(30);
                deSearch.ServerTimeLimit = TimeSpan.FromMinutes(30);
                deSearch.ServerPageTimeLimit = TimeSpan.FromMinutes(30);

                deSearch.PropertiesToLoad.AddRange(propertiesToLoad);
                return deSearch.FindAll();
            }
        }


        /// <summary>
        /// Localizar um objeto no AD
        /// </summary>
        /// <param name="filter">(&amp;(objectClass=user)(objectCategory=person))(!(userAccountControl:1.2.840.113556.1.4.803:=2))</param>
        /// <returns>SearchResultCollection</returns>
        public SearchResultCollection Search(string filter)
        {
            return Search(filter, null, PropertiesToLoad);
        }

        /// <summary>
        /// Localizar um objeto no AD
        /// </summary>
        /// <param name="filter">(&amp;(objectClass=user)(objectCategory=person))(!(userAccountControl:1.2.840.113556.1.4.803:=2))</param>
        /// <param name="propertiesToLoad">"cn", "OU", "UserAccountControl", "WhenCreated", "DistinguishedName", "sAMAccountName"</param>
        /// <returns>SearchResultCollection</returns>
        public SearchResultCollection Search(string filter, params string[] propertiesToLoad)
        {
            return Search(filter, null, propertiesToLoad);
        }

        /// <summary>
        /// Localizar um objeto no AD
        /// </summary>
        /// <param name="filter">(&amp;(objectClass=user)(objectCategory=person))(!(userAccountControl:1.2.840.113556.1.4.803:=2))</param>
        /// <param name="rootSubPath">OU=Equipe TI,DC=ocean,DC=one,DC=com</param>
        /// <param name="propertiesToLoad">"cn", "OU", "UserAccountControl", "WhenCreated", "DistinguishedName", "sAMAccountName"</param>
        /// <returns>SearchResult</returns>
        public SearchResult SearchOne(string filter, string rootSubPath, params string[] propertiesToLoad)
        {
            DomainManager.RootSubPath = rootSubPath;
            string rootPath = DomainManager.RootPath.Replace(DomainManager.DomainPath, "").Replace(",,", ",") + DomainManager.DomainPath;
            DirectoryEntry de;
            try
            {
                de = new DirectoryEntry(rootPath, LDAP_USER, LDAP_PASS, AuthenticationTypes.Secure);
                if (de.Guid == Guid.Empty)
                {
                    DomainManager.RootSubPath = "";
                    de = new DirectoryEntry(DomainManager.RootPath, LDAP_USER, LDAP_PASS, AuthenticationTypes.Secure);
                }
            }
            catch
            {
                DomainManager.RootSubPath = "";
                de = new DirectoryEntry(DomainManager.RootPath, LDAP_USER, LDAP_PASS, AuthenticationTypes.Secure);
            }

            if (de.Guid == Guid.Empty)
                de = GetDirectoryEntry(LDAP);

            de.RefreshCache();

            if (propertiesToLoad == null || propertiesToLoad.Length <= 0)
                propertiesToLoad = PropertiesToLoad;

            using (DirectorySearcher deSearch = new DirectorySearcher(de))
            {
                deSearch.Filter = filter;
                deSearch.PageSize = 1000;
                deSearch.CacheResults = false;
                deSearch.Asynchronous = true;
                deSearch.SearchScope = SearchScope.Subtree;
                deSearch.ClientTimeout = TimeSpan.FromMinutes(30);
                deSearch.ServerTimeLimit = TimeSpan.FromMinutes(30);
                deSearch.ServerPageTimeLimit = TimeSpan.FromMinutes(30);

                deSearch.PropertiesToLoad.AddRange(propertiesToLoad);
                return deSearch.FindOne();
            }
        }

        /// <summary>
        /// Localizar um objeto no AD
        /// </summary>
        /// <param name="filter">(&amp;(objectClass=user)(objectCategory=person))(!(userAccountControl:1.2.840.113556.1.4.803:=2))</param>
        /// <param name="propertiesToLoad">"cn", "OU", "UserAccountControl", "WhenCreated", "DistinguishedName", "sAMAccountName"</param>
        /// <returns>SearchResult</returns>
        public SearchResult SearchOne(string filter, params string[] propertiesToLoad)
        {
            return SearchOne(filter, null, propertiesToLoad);
        }

         /// <summary>
        /// Localizar um objeto no AD
        /// </summary>
        /// <param name="filter">(&amp;(objectClass=user)(objectCategory=person))(!(userAccountControl:1.2.840.113556.1.4.803:=2))</param>
        /// <returns>SearchResult</returns>
        public SearchResult SearchOne(string filter)
        {
            return SearchOne(filter, null, PropertiesToLoad);
        }

        #endregion

        #region Login AD
        /// <summary>
        /// Efetuar o login do usuario do AD
        /// </summary>
        public bool Login(string login, string senha)
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry(LDAP, login, senha);
                if (de.Guid != Guid.Empty)
                {
                    if (de.Username == login)
                        return true;
                    return false;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Destruir a classe.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        bool _disposed;
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    LDAP = null;
                    LDAP_USER = null;
                    LDAP_PASS = null;

                }
                _disposed = true;
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Dispose
        /// </summary>
        ~ActiveDirectory()
        {
            Dispose(false);
        }

        #endregion
    }
}
