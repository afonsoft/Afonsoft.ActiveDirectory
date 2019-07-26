using System;
using System.Collections.Generic;
using System.DirectoryServices;
using Afonsoft.ActiveDirectory.Times;

namespace Afonsoft.ActiveDirectory.Models
{
    /// <summary>
    /// User of AD
    /// </summary>
    public class UserActiveDirectory : ObjectActiveDirectory
    {
        private DirectoryEntry _entry;

        /// <summary>
        /// UserActiveDirectory
        /// </summary>
        /// <param name="entry">DirectoryEntry</param>
        public UserActiveDirectory(DirectoryEntry entry) : base(entry)
        {
            _entry = entry;
        }
        /// <summary>
        /// UserActiveDirectory
        /// </summary>
        public UserActiveDirectory() : base()
        {
            _entry = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public new DirectoryEntry Entry
        {

            get
            {
                return _entry;
            }
            set
            {
                _entry = value;
                Cn = GetProperty<string>("CN");
                Ou = GetProperty<string>("OU");
                UserAccountControl = GetProperty<int>("UserAccountControl");
                WhenCreated = GetProperty<DateTime>("WhenCreated");
                DistinguishedName = GetProperty<string>("DistinguishedName");
                SAMAccountName = GetProperty<string>("sAMAccountName");
                //Preencher os valores das propriedades.
                Company = GetProperty<string>("company");
                Description = GetProperty<string>("description");
                DisplayName = GetProperty<string>("displayName");
                GivenName = GetProperty<string>("givenName");
                Mail = GetProperty<string>("mail");
                SurName = GetProperty<string>("sn");
                TelephoneNumber = GetProperty<string>("telephoneNumber");
                Title = GetProperty<string>("title");
                Url = GetProperty<string>("url");

                UserPrincipalName = GetProperty<string>("userPrincipalName");
                WWWHomePage = GetProperty<string>("wWWHomePage");

                PhysicalDeliveryOfficeName = GetProperty<string>("physicalDeliveryOfficeName");
                Department = GetProperty<string>("department");
                Manager = GetProperty<string>("manager");
                ProfilePath = GetProperty<string>("profilePath");
                ScriptPath = GetProperty<string>("scriptPath");
                HomeDirectory = GetProperty<string>("homeDirectory");
                HomeDrive = GetProperty<string>("homeDrive");
                HomePhone = GetProperty<string>("homePhone");
                OtherHomePhone = GetProperty<string>("otherHomePhone");
                Pager = GetProperty<string>("pager");
                OtherPager = GetProperty<string>("otherPager");
                Mobile = GetProperty<string>("mobile");
                OtherMobile = GetProperty<string>("otherMobile");
                FacsimileTelephoneNumber = GetProperty<string>("facsimileTelephoneNumber");
                OtherFacsimileTelephoneNumber = GetProperty<string>("otherFacsimileTelephoneNumber");
                IpPhone = GetProperty<string>("ipPhone");
                OtherIpPhone = GetProperty<string>("otherIpPhone");
                Info = GetProperty<string>("info");
                StreetAddress = GetProperty<string>("streetAddress");
                PostalCode = GetProperty<string>("postalCode");
                PostOfficeBox = GetProperty<string>("postOfficeBox");
                St = GetProperty<string>("st");

                LogonHours = GetProperty<byte[]>("logonHours");
                LogonTimes = PermittedLogonTimes.GetLogonTimes(GetProperty<byte[]>("logonHours"));
            }
        }

        //General
        /// <summary>
        /// General
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// General
        /// </summary>
        public string TelephoneNumber { get; set; }
        /// <summary>
        /// General
        /// </summary>
        public string GivenName { get; set; }
        /// <summary>
        /// General
        /// </summary>
        public string SurName { get; set; }
        /// <summary>
        /// General
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// General
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// General
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// General
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string WWWHomePage { get; set; }
        /// <summary>
        /// General
        /// </summary>
        public string PhysicalDeliveryOfficeName { get; set; }

        //Organization
        /// <summary>
        /// Organization
        /// </summary>
        public string Department { get; set; }
        /// <summary>
        /// Organization - Manager CN Path
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Organization
        /// </summary>
        public string Company { get; set; }

        //Account
        /// <summary>
        /// Account
        /// </summary>
        public string UserPrincipalName { get; set; }
        
        //Profile
        /// <summary>
        /// Profile
        /// </summary>
        public string ProfilePath { get; set; }
        /// <summary>
        /// Profile
        /// </summary>
        public string ScriptPath { get; set; }
        /// <summary>
        /// Profile
        /// </summary>
        public string HomeDirectory { get; set; }
        /// <summary>
        /// Profile
        /// </summary>
        public string HomeDrive { get; set; }


        //Telephones
        /// <summary>
        ///Telephones 
        /// </summary>
        public string HomePhone { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string OtherHomePhone { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string Pager { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string OtherPager { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string OtherMobile { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string FacsimileTelephoneNumber { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string OtherFacsimileTelephoneNumber { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string IpPhone { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string OtherIpPhone { get; set; }
        /// <summary>
        ///Telephones 
        /// </summary>
        public string Info { get; set; }

        //Address
        /// <summary>
        ///street Address 
        /// </summary>
        public string StreetAddress { get; set; }
        /// <summary>
        ///postal Code 
        /// </summary>
        public string PostalCode { get; set; }
        /// <summary>
        ///post Office Box 
        /// </summary>
        public string PostOfficeBox { get; set; }
        /// <summary>
        ///State / province
        /// </summary>
        public string St { get; set; }

        /// <summary>
        /// LogonHours
        /// https://github.com/anlai/Permitted-Logon-Times-for-Active-Directory
        /// https://anlai.wordpress.com/2010/09/07/active-directory-permitted-logon-times-with-c-net-3-5-using-system-directoryservices-accountmanagement/
        /// <code>
        ///     Liberado para todos os horarios
        ///     LogonHours = null 
        /// </code>
        /// <code>
        ///     todos os byte 0 bloqueados em todos os horarios
        ///     LogonHours = byte[21] 
        /// </code>
        /// </summary>
        public byte[] LogonHours { get; set; }

        /// <summary>
        /// LogonTimes
        /// </summary>
        public List<LogonTime> LogonTimes { get; set; }
    }
}
