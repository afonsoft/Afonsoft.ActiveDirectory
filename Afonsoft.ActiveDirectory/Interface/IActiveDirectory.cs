using System;
using System.DirectoryServices;

namespace Afonsoft.ActiveDirectory.Interface
{
    /// <summary>
    /// IActiveDirectory
    /// </summary>
    public interface IActiveDirectory
    {

        /// <summary>
        /// Retrieve the value of the property.
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns></returns>
        object GetProperty(string propertyName);

        /// <summary>
        /// Retrieve the value of the property.
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <returns></returns>
        T GetProperty<T>(string propertyName);

        /// <summary>
        /// DirectoryEntry
        /// </summary>
        DirectoryEntry Entry { get; set; }

        /// <summary>
        /// Guid
        /// </summary>
        string Guid { get; }

        /// <summary>
        /// NativeGuid
        /// </summary>
        string NativeGuid { get; }

        /// <summary>
        /// Account
        /// </summary>
        string Dn { get;  }
        /// <summary>
        /// Account
        /// </summary>
        string Cn { get; }
        /// <summary>
        /// Account
        /// </summary>
        string Ou { get;  }

        /// <summary>
        /// Verifica se o objeto está ativo ou inativo
        /// </summary>
        bool IsActive { get;  }

        /// <summary>
        /// UserAccountControl
        /// </summary>
        int UserAccountControl { get; }

        /// <summary>
        /// Created - DateTime
        /// </summary>
        DateTime WhenCreated { get; }

        /// <summary>
        /// DistinguishedName
        /// </summary>
        string DistinguishedName { get; }
        
        /// <summary>
        /// SAMAccountName
        /// </summary>
        string SAMAccountName { get;  }

    }
}
