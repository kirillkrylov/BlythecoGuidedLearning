using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Web.Common;

namespace FINANCE
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SampleWebService : BaseService
    {
        #region Properties
        private SystemUserConnection _systemUserConnection;
        private SystemUserConnection SystemUserConnection
        {
            get
            {
                return _systemUserConnection ?? (_systemUserConnection = (SystemUserConnection)AppConnection.SystemUserConnection);
            }
        }
        #endregion
        
        #region Methods : REST
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public string PostMethodName(Guid bankLineId)
        {
            UserConnection userConnection = UserConnection ?? SystemUserConnection;

            return FindContactEmailByIdSelect(bankLineId, userConnection);
            //return FindContactEmailByIdESQ(bankLineId);
        }


        //http://k_krylov_nb:8010/0/rest/SampleWebService/CreateContact
        [OperationContract]
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public string CreateContact(string NameParameter, string EmailParameter)
        {
            UserConnection userConnection = UserConnection ?? SystemUserConnection;
            return CreateNewContact(NameParameter, EmailParameter, userConnection);
        }






        //http://k_krylov_nb:8010/0/SampleWebService/GetMethodname?bankLineId=ID_GOES_HERE
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public string GetMethodname(Guid bankLineId)
        {
            UserConnection userConnection = UserConnection ?? SystemUserConnection;
            return "Ok";
        }

        #endregion

        #region Methods : Private


        string FindContactEmailByIdSelect(Guid contactId, UserConnection userConnection) 
        {
            string result = string.Empty;

            Select select = new Select(userConnection) 
                .Column("Email")
                .Top(1)
                .From("Contact")
                .Where("Id").IsEqual(Column.Parameter(contactId)) as Select;

            using (DBExecutor executor = userConnection.EnsureDBConnection())
            {
                result = select.ExecuteScalar<string>(executor);
            }
                                 


            return result;
        }


        string CreateNewContact(string Name, string Email, UserConnection userConnection) 
        {
            string result = string.Empty;

            Guid Id = Guid.NewGuid();
            
            Insert insert = new Insert(userConnection) 
                .Into("Contact")
                .Set("Id",Column.Parameter(Id))
                .Set("Name",Column.Parameter(Name))
                .Set("Email",Column.Parameter(Email))               
                as Insert;


            int resuCount = insert.Execute();

            if (resuCount == 1) {
                return Id.ToString();
            }
            else return "Error";





            //return result;
        }


        #endregion
    }
}



