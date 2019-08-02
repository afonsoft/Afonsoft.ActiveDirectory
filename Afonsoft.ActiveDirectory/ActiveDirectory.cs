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

        private ActiveDirectoryOptions _options;
        /// <summary>
        /// ActiveDirectoryOptions
        /// </summary>
        public ActiveDirectoryOptions Options
        {
            get
            {
                return _options;
            }
            set
            {
                _options = value;
            }
        }


        /// <summary>
        /// Default propertiesToLoad
        /// </summary>
        private string[] PropertiesToLoad { get; } = new string[] { "cn", "OU", "UserAccountControl", "WhenCreated", "DistinguishedName", "sAMAccountName" };


        #endregion

        #region ActiveDirectory

        private static ActiveDirectoryOptions Build(Action<ActiveDirectoryOptions> options)
        {
            var opt = new ActiveDirectoryOptions();
            options(opt);
            return opt;
        }

        /// <summary>
        /// ActiveDirectory
        /// </summary>
        public ActiveDirectory(Action<ActiveDirectoryOptions> options)
        {
            _options = Build(options);
        }
        /// <summary>
        /// ActiveDirectory
        /// </summary>
        public ActiveDirectory()
        {
            _options = new ActiveDirectoryOptions();
        }


        public bool CheckConnection()
        {
            if (string.IsNullOrEmpty(_options.LDAP))
                _options.LDAP = DomainManager.RootPath;

            if (!Login(_options.LDAP_USER, _options.LDAP_PASS))
                return false;

            return true;
        }

        #endregion

        #region getDirectoryEntry

        /// <summary>
        /// Recuperar o Diretorio padrão da DN
        /// </summary>
        private DirectoryEntry GetDirectoryEntry()
        {
            return new DirectoryEntry(_options.LDAP, _options.LDAP_USER, _options.LDAP_PASS, _options.AuthenticationTypes);
        }


        /// <summary>
        /// Recuperar o Diretorio da DN
        /// </summary>
        public DirectoryEntry GetDirectoryEntry(string ldap)
        {
            return new DirectoryEntry(ldap, _options.LDAP_USER, _options.LDAP_PASS, _options.AuthenticationTypes);
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
            try
            {
                DomainManager.RootSubPath = rootSubPath;
                string rootPath = DomainManager.RootPath.Replace(DomainManager.DomainPath, "").Replace(",,", ",") + DomainManager.DomainPath;
                DirectoryEntry de;
                try
                {
                    de = new DirectoryEntry(rootPath, _options.LDAP_USER, _options.LDAP_PASS, _options.AuthenticationTypes);
                    if (de.Guid == Guid.Empty)
                    {
                        DomainManager.RootSubPath = "";
                        de = new DirectoryEntry(DomainManager.RootPath, _options.LDAP_USER, _options.LDAP_PASS, _options.AuthenticationTypes);
                    }
                }
                catch
                {
                    DomainManager.RootSubPath = "";
                    de = new DirectoryEntry(DomainManager.RootPath, _options.LDAP_USER, _options.LDAP_PASS, _options.AuthenticationTypes);
                }

                if (de.Guid == Guid.Empty)
                    de = GetDirectoryEntry(_options.LDAP);

                de.RefreshCache();

                if (propertiesToLoad == null || propertiesToLoad.Length <= 0)
                    propertiesToLoad = PropertiesToLoad;

                using (DirectorySearcher deSearch = new DirectorySearcher(de))
                {
                    deSearch.Filter = filter;
                    deSearch.PageSize = _options.PageSize;
                    deSearch.CacheResults = _options.CacheResults;
                    deSearch.Asynchronous = _options.Asynchronous;
                    deSearch.SearchScope = _options.SearchScope;
                    deSearch.ClientTimeout = _options.Timeout;
                    deSearch.ServerTimeLimit = _options.Timeout;
                    deSearch.ServerPageTimeLimit = _options.Timeout;

                    deSearch.PropertiesToLoad.AddRange(propertiesToLoad);
                    return deSearch.FindAll();
                }
            }
            catch (Exception ex)
            {
                throw new ActiveDirectoryException(ex.Message, ex);
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
            try
            {
                DomainManager.RootSubPath = rootSubPath;
                string rootPath = DomainManager.RootPath.Replace(DomainManager.DomainPath, "").Replace(",,", ",") + DomainManager.DomainPath;
                DirectoryEntry de;
                try
                {
                    de = new DirectoryEntry(rootPath, _options.LDAP_USER, _options.LDAP_PASS, _options.AuthenticationTypes);
                    if (de.Guid == Guid.Empty)
                    {
                        DomainManager.RootSubPath = "";
                        de = new DirectoryEntry(DomainManager.RootPath, _options.LDAP_USER, _options.LDAP_PASS, _options.AuthenticationTypes);
                    }
                }
                catch
                {
                    DomainManager.RootSubPath = "";
                    de = new DirectoryEntry(DomainManager.RootPath, _options.LDAP_USER, _options.LDAP_PASS, _options.AuthenticationTypes);
                }

                if (de.Guid == Guid.Empty)
                    de = GetDirectoryEntry(_options.LDAP);

                de.RefreshCache();

                if (propertiesToLoad == null || propertiesToLoad.Length <= 0)
                    propertiesToLoad = PropertiesToLoad;

                using (DirectorySearcher deSearch = new DirectorySearcher(de))
                {
                    deSearch.Filter = filter;
                    deSearch.PageSize = _options.PageSize;
                    deSearch.CacheResults = _options.CacheResults;
                    deSearch.Asynchronous = _options.Asynchronous;
                    deSearch.SearchScope = _options.SearchScope;
                    deSearch.ClientTimeout = _options.Timeout;
                    deSearch.ServerTimeLimit = _options.Timeout;
                    deSearch.ServerPageTimeLimit = _options.Timeout;

                    deSearch.PropertiesToLoad.AddRange(propertiesToLoad);
                    return deSearch.FindOne();
                }
            }
            catch (Exception ex)
            {
                throw new ActiveDirectoryException(ex.Message, ex);
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
                DirectoryEntry de = new DirectoryEntry(_options.LDAP, login, senha, _options.AuthenticationTypes);

                if (de == null || de.Guid == Guid.Empty)
                {
                    DomainManager.RootSubPath = "";
                    de = new DirectoryEntry(DomainManager.RootPath, login, senha, _options.AuthenticationTypes);
                }

                if (de != null && de.Guid != Guid.Empty)
                {
                    if (de.Username == login)
                        return true;
                    return false;
                }

                return false;
            }
            catch (DirectoryServicesCOMException ex)
            {
                throw new ActiveDirectoryException(ex.Message);
            }
            catch (Exception ex)
            {
                throw new ActiveDirectoryException(ex.Message, ex);
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

        private void Dispose(bool disposing)
        {

            if (disposing)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.SuppressFinalize(this);
            }
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