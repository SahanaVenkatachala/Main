using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using System.Runtime.Serialization;
using System.Diagnostics;
using Windows.System.Profile;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;

namespace storeEmpowerment
{
    [DataContract]
    class LoginData
    {
        public  LoginData(string userName, string password,string domain)
        {
            
            EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
            if (App.localSettings.Values["deviceID"] == null)
            {
                App.localSettings.Values["deviceID"] = Guid.NewGuid();
                
            }


            this.DeviceID =(Guid) App.localSettings.Values["deviceID"];
            this.Domain = domain;
            this.UserName = userName;
            this.Password = password;
            
           // this.AppVersion = "1.73";
            this.DeviceName = deviceInfo.FriendlyName;
            this.DevicePlatform = "Windows";
            this.DevicePlatformVersion = "8.1";
            

            
        }

        [DataMember]
        public string DevicePlatform { get; set; }
        [DataMember]
        public string Domain { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string DeviceName { get; set; }
        [DataMember]
        public string DevicePlatformVersion { get; set; }
        [DataMember]
        //public string AppVersion { get; set; }
        //[DataMember]
        public Guid DeviceID { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
}
