using Afonsoft.ActiveDirectory.Interface;
using System;
using System.DirectoryServices;

namespace Afonsoft.ActiveDirectory
{
    /// <summary>
    /// Referencia da Class (http://www.codeproject.com/Articles/18102/Howto-Almost-Everything-In-Active-Directory-via-C#32)
    /// </summary>
    public abstract class ObjectActiveDirectory : IActiveDirectory, IDisposable
    {
        /// <summary>
        /// ObjectActiveDirectory
        /// </summary>
        /// <param name="entry">DirectoryEntry</param>
        protected ObjectActiveDirectory(DirectoryEntry entry)
        {
            Entry = entry;
        }

        /// <summary>
        /// DirectoryEntry
        /// </summary>
        public DirectoryEntry Entry { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        public string Guid => Entry != null ? Entry.Guid.ToString() : "";

        /// <summary>
        /// Guid
        /// </summary>
        public string NativeGuid => Entry != null ? Entry.NativeGuid : "";

        /// <summary>
        /// DN - Path
        /// </summary>
        public string Dn => Entry != null ? Entry.Path : "";

        /// <summary>
        /// Cn
        /// </summary>
        public string Cn
        {
            get
            {
                if (Entry == null) return "";
                if (Entry.Properties.Contains("cn"))
                {
                    return (string)Entry.Properties["cn"].Value;
                }
                return "";
            }
        }

        /// <summary>
        /// Ou
        /// </summary>
        public string Ou
        {
            get
            {
                if (Entry == null) return "";
                if (Entry.Properties.Contains("OU"))
                {
                    return (string)Entry.Properties["OU"].Value;
                }
                return "";
            }
        }

        /// <summary>
        /// Ou
        /// </summary>
        public int UserAccountControl
        {
            get
            {
                if (Entry == null) return 0;
                if (Entry.Properties.Contains("UserAccountControl"))
                {
                    return (int)Entry.Properties["UserAccountControl"].Value;
                }
                return 0;
            }
        }

        /// <summary>
        /// Verifica se o objeto está ativo ou inativo
        /// </summary>
        public bool IsActive
        {
            get
            {
                if (Entry == null) return false;
                if (Entry.Properties.Contains("userAccountControl"))
                {
                    int flags = (int)Entry.Properties["userAccountControl"].Value;
                    return !Convert.ToBoolean(flags & 0x0002);
                }
                return false;
            }
        }


        /// <summary>
        /// Date of Create object in AD
        /// </summary>
        public DateTime WhenCreated
        {
            get
            {
                if (Entry == null) return DateTime.MinValue;
                if (Entry.Properties.Contains("WhenCreated"))
                {
                    return (DateTime)Entry.Properties["WhenCreated"].Value;
                }
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// DistinguishedName
        /// </summary>
        public string DistinguishedName
        {
            get
            {
                if (Entry == null) return "";
                if (Entry.Properties.Contains("DistinguishedName"))
                {
                    return (string)Entry.Properties["DistinguishedName"].Value;
                }
                return "";
            }
        }

        /// <summary>
        /// SAMAccountName - Login
        /// </summary>
        public string SAMAccountName
        {
            get
            {
                if (Entry == null) return "";
                if (Entry.Properties.Contains("sAMAccountName"))
                {
                    return (string)Entry.Properties["sAMAccountName"].Value;
                }
                return "";
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Entry?.Dispose();
            Entry = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Retrieve the value of the property.
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns></returns>
        public object GetProperty(string propertyName)
        {
            if (Entry == null) return null;
            if (Entry.Properties.Contains(propertyName))
            {
                return Entry.Properties[propertyName].Value;
            }
            return null;
        }

        /// <summary>
        /// Retrieve the value of the property.
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns></returns>
        public T GetProperty<T>(string propertyName)
        {
            if (Entry == null) return default(T);
            if (Entry.Properties.Contains(propertyName))
            {
                return (T)Entry.Properties[propertyName].Value;
            }
            return default(T);
        }
    }
}
