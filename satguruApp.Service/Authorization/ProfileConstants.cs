using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace satguruApp.Service.Authorization
{
    public static class ClaimConstants
    {
        ///<summary>A claim that specifies the subject of an entity</summary>
        public const string Subject = "sub";



        ///<summary>A claim that specifies the permission of an entity</summary>
        public const string Permission = "permission";
    }


    public static class PropertyConstants
    {



        ///<summary>A property that specifies the full name of an entity</summary>
        public const string FullName = "fullname";



        ///<summary>A property that specifies the job title of an entity</summary>
        public const string JobTitle = "jobtitle";



        ///<summary>A property that specifies the configuration/customizations of an entity</summary>
        public const string Configuration = "configuration";



        ///<summary>A property that specifies the AppUserId of an entity</summary>
        public const string AppUserId = "appuserid";
        public const string DefaultDivisionID = "defaultdivisionid";



        public const string PhoneNumber = "phone_number";



        public const string Email = "email";
        public const string UserName = "userName";



        public const string CreatedDateTime = "createdDateTime";



        ///<summary>A property that specifies theUserId of an entity</summary>
        public const string UserId = "userId";



        public const string CompanyId = "companyId";
        public const string FaceRecognition = "faceRecognition";
        public const string PalmRecognition = "palmRecognition";
        public const string VoiceRecognition = "voiceRecognition";
        public const string SMSOTP = "smsOTP";
        public const string EmailOTP = "emailOTP";
        public const string Authenticator = "authenticator";



        public const string TouchId = "touchId";
        public const string AllowPassword = "allowPassword";
        public const string AllowTOTP = "allowTOTP";
        public const string AllowHOTP = "AllowHOTP";
        public const string GTCID = "Gtcid";
        public const string OtpSecret = "otpSecret";
        public const string Pin = "Pin";
    }





    public static class ScopeConstants
    {
        ///<summary>A scope that specifies the roles of an entity</summary>
        public const string Roles = "roles";
    }
}
