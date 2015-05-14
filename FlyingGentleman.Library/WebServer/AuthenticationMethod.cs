using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FlyingGentleman.Library.WebServer
{
    /// <summary>
    /// The authentication method for the web application.
    /// It can be either Forms Authentication, Windows Authentication, Basic Authentication, BasicAuthenticationAndWindowsAuthentication or None.
    /// It can not be both (Challenge based auth and login redirect based cannot be used simultaneously).
    /// </summary>
    [DataContract]
    public enum AuthenticationMethod
    {
        /// <summary>
        /// Disable Forms and Windows Authentication.
        /// </summary>
        [EnumMember]
        None,
        /// <summary>
        /// Enable Forms Authentication for the web application.
        /// </summary>
        [EnumMember]
        FormsAuthentication,
        /// <summary>
        /// Enable Windows authentication for the web application.
        /// </summary>
        [EnumMember]
        WindowsAuthentication,
        /// <summary>
        /// Enable Basic authentication for the web application.
        /// </summary>
        [EnumMember]
        BasicAuthentication,
        /// <summary>
        /// Enable Basic authentication and Windows Authentication for the web application.
        /// </summary>    
        [EnumMember]
        BasicAuthenticationAndWindowsAuthentication
    }
}
