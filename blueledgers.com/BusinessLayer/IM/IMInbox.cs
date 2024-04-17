using System;
using System.Data;
using Blue.DAL;

// ReSharper disable once CheckNamespace
namespace Blue.BL.IM
{
    public class IMInbox : DbHandler
    {
        #region "Attributies"

        private readonly IMSent _sentItems = new IMSent();

        #endregion

        #region "Operations"

        public IMInbox()
        {
            SelectCommand = "select * from IM.Inbox";
            TableName = "Inbox";
        }

        public bool GetList(DataSet dsInbox, string strConn)
        {
            return DbRetrieve("IM.GetInboxList", dsInbox, null, TableName, strConn);
        }

        /// <summary>
        ///     Get Schema of Inbox
        /// </summary>
        /// <returns></returns>
        public bool GetSchema(DataSet dsInbox, string connStr)
        {
            return DbRetrieveSchema("IM.GetInboxList", dsInbox, null, TableName, connStr);
        }

        public bool GetMessage(DataSet dsInbox, string inboxNo, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@InboxNo", inboxNo);

            return DbRetrieve("IM.GetInboxRead", dsInbox, dbParams, TableName, strConn);
        }

        public bool GetSearch(DataSet dsSearch, string search, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@Search", search);

            return DbRetrieve("IM.GetInboxSearch", dsSearch, dbParams, TableName, strConn);
        }

        public bool GetUserList(DataSet dsUserList, string LoginUser, string strConn)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginUser", LoginUser);

            return DbRetrieve("IM.GetInboxUserList", dsUserList, dbParams, TableName, strConn);
        }

        /// <summary>
        ///     Get all inbox items related to LoginName.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetList(string LoginName, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginUser", LoginName);

            return DbRead("IM.GetInboxUserList", dbParams, ConnStr);
        }

        /// <summary>
        ///     Get Mail Unread.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public DataTable GetCountUnRead(string LoginName, string ConnStr)
        {
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginUser", LoginName);

            return DbRead("IM.GetCountUnRead", dbParams, ConnStr);
        }

        /// <summary>
        ///     Count Mail UnRead.
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="ConnStr"></param>
        /// <returns></returns>
        public int GetCountByUnRead(string LoginName, string ConnStr)
        {
            // Create parameters
            var dbParams = new DbParameter[1];
            dbParams[0] = new DbParameter("@LoginUser", LoginName);

            return DbReadScalar("IM.GetCountUnRead", dbParams, ConnStr);
        }

        public bool Save(DataSet dsSaved, string strConn)
        {
            var dbSaveSource = new DbSaveSource[1];
            dbSaveSource[0] = new DbSaveSource(dsSaved, SelectCommand, TableName);

            return DbCommit(dbSaveSource, strConn);
        }

        public bool Save(DataSet dsSaved, IMSent SentObject, string ConnStr)
        {
            var dbSaveSource = new DbSaveSource[2];
            dbSaveSource[0] = new DbSaveSource(dsSaved, SelectCommand, TableName);
            dbSaveSource[1] = new DbSaveSource(dsSaved, SentObject.SelectCommand, SentObject.TableName);

            return DbCommit(dbSaveSource, ConnStr);
        }

        public int GetCount(DataSet dsInbox, string strConn)
        {
            return DbReadScalar("IM.GetInboxCountUnRead", null, strConn);
        }

        public int GetNewNo(string strConn)
        {
            return DbReadScalar("IM.GetInBoxNewNo", null, strConn);
        }

        public bool SendMessage(string Sender, string Receiver, string CC, string Subject, DateTime SendDate,
            bool IsImportance, bool IsRequest, string Message, string ConnStr)
        {
            var dsIM = new DataSet();
            var NewID = GetNewNo(ConnStr);

            // Get Structure of Inbox            
            var getInboxSchema = GetSchema(dsIM, ConnStr);

            if (!getInboxSchema)
            {
                return false;
            }

            // Create Inbox Message by Receiver and CC.
            string[] allReceiver;

            if (string.IsNullOrEmpty(CC))
            {
                allReceiver = Receiver.Split(';');
            }
            else
            {
                allReceiver = (Receiver + ";" + CC).Split(';');
            }

            foreach (var receiver in allReceiver)
            {
                var drInbox = dsIM.Tables[TableName].NewRow();

                drInbox["InboxNo"] = NewID;
                drInbox["Sender"] = Sender;
                drInbox["Reciever"] = receiver;
                drInbox["Subject"] = Subject;
                drInbox["Date"] = SendDate;
                drInbox["CC"] = CC;
                drInbox["Importance"] = IsImportance;
                drInbox["Request"] = IsRequest;
                drInbox["Message"] = GnxLib.EnDecryptString(Message, GnxLib.EnDeCryptor.EnCrypt);
                drInbox["IsRead"] = false;
                drInbox["IsForward"] = false;
                drInbox["IsReply"] = false;

                dsIM.Tables[TableName].Rows.Add(drInbox);

                NewID++;
            }

            // Get Structure of SentItems
            var getSentSchema = _sentItems.GetSchema(dsIM, ConnStr);

            if (!getSentSchema)
            {
                return false;
            }

            // Create SentItems Message
            var drSent = dsIM.Tables[_sentItems.TableName].NewRow();

            drSent["SentNo"] = _sentItems.GetNewNo(ConnStr);
            drSent["Sender"] = Sender;
            drSent["Reciever"] = Receiver;
            drSent["Subject"] = Subject;
            drSent["Date"] = SendDate;
            drSent["CC"] = CC;
            drSent["Importance"] = IsImportance;
            drSent["Request"] = IsRequest;
            drSent["Message"] = GnxLib.EnDecryptString(Message, GnxLib.EnDeCryptor.EnCrypt);
            drSent["IsForward"] = false;
            drSent["IsReply"] = false;

            dsIM.Tables[_sentItems.TableName].Rows.Add(drSent);

            // SaveChanged
            return Save(dsIM, _sentItems, ConnStr);
        }

        public int CountUnread(string LoginName, string ConnStr)
        {
            return DbReadScalar("IM.IMInbox_CountUnRead", null, ConnStr);
        }

        #endregion
    }
}