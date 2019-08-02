using Afonsoft.ActiveDirectory.Interfaces;
using System;
using System.DirectoryServices;

namespace Afonsoft.ActiveDirectory.Models
{
    /// <summary>
    /// Referencia da Class (http://www.codeproject.com/Articles/18102/Howto-Almost-Everything-In-Active-Directory-via-C#32)
    /// </summary>
    public abstract class ObjectActiveDirectory : IActiveDirectory, IDisposable
    {
        private DirectoryEntry _entry;

        /// <summary>
        /// ObjectActiveDirectory
        /// </summary>
        /// <param name="entry">DirectoryEntry</param>
        protected ObjectActiveDirectory(DirectoryEntry entry)
        {
            _entry = entry;
            Cn = GetProperty<string>("CN");
            Ou = GetProperty<string>("OU");
            UserAccountControl = GetProperty<int>("UserAccountControl");
            WhenCreated = GetProperty<DateTime>("WhenCreated");
            DistinguishedName = GetProperty<string>("DistinguishedName");
            SAMAccountName = GetProperty<string>("sAMAccountName");

        }
        /// <summary>
        /// ObjectActiveDirectory
        /// </summary>
        protected ObjectActiveDirectory()
        {
            _entry = null;
        }

        /// <summary>
        /// DirectoryEntry
        /// </summary>
        public DirectoryEntry Entry
        {
            get
            {
                return _entry;
            }
        }

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
        public string Cn { get; set; }


        /// <summary>
        /// Ou
        /// </summary>
        public string Ou { get; set; }


        /// <summary>
        /// Ou
        /// </summary>
        public int UserAccountControl { get; set; }


        /// <summary>
        /// Verifica se o objeto está ativo ou inativo
        /// </summary>
        public bool IsActive
        {
            get
            {
                if (Entry == null) return false;
                return !Convert.ToBoolean(UserAccountControl & 0x0002);
            }
        }


        /// <summary>
        /// Date of Create object in AD
        /// </summary>
        public DateTime? WhenCreated { get; set; }


        /// <summary>
        /// DistinguishedName
        /// </summary>
        public string DistinguishedName { get; set; }


        /// <summary>
        /// SAMAccountName - Login
        /// </summary>
        public string SAMAccountName { get; set; }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Entry?.Dispose();
            _entry = null;
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
            if (Entry == null) return default;
            if (Entry.Properties.Contains(propertyName))
            {
                return (T)Entry.Properties[propertyName].Value;
            }
            return default;
        }

        /// <summary>
        /// CompareTo
        /// </summary>
        /// <param name="obj">ObjectActiveDirectory</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj is ObjectActiveDirectory e)
            {
                if (this.WhenCreated > e.WhenCreated) return -1;
                if (this.WhenCreated == e.WhenCreated) return 0;
                if (this.WhenCreated < e.WhenCreated) return 1;
            }
            throw new ArgumentException("obj is not an ObjectActiveDirectory.");
        }

        /// <summary>
        /// Compare
        /// </summary>
        /// <param name="x">ObjectActiveDirectory</param>
        /// <param name="y">ObjectActiveDirectory</param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if (x is ObjectActiveDirectory a && y is ObjectActiveDirectory b)
            {
                if (a.WhenCreated > b.WhenCreated) return -1;
                if (a.WhenCreated == b.WhenCreated) return 0;
                if (a.WhenCreated < b.WhenCreated) return 1;
            }

            throw new ArgumentException("x or y is not an ObjectActiveDirectory.");
        }
    }
}
