using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    #region "IsApprove"
    /*
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean IsApprove(string ApprStatus, int Step, int Appr, int NeededAppr)
    {
        int found = 0;

        if (ApprStatus.Length > 0)
        {
            string StepApprStatus = ApprStatus.Substring((Step - 1) * 10, 10);

            foreach (char c in StepApprStatus)
            {
                if (c.ToString().ToUpper() == "A")
                {
                    found++;
                }
            }
        }

        if (found == ((10 - Appr) + NeededAppr))
        {
            return new SqlBoolean(true);
        }
        else
        {
            return new SqlBoolean(false);
        }
    }
    */
    #endregion

    #region "IsHdrApprove"
    /*
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean IsHdrApprove(string ApprStatus, int Step)
    {
        if (ApprStatus.Length > 0)
        {
            string StepApprStatus = ApprStatus.Substring(Step - 1, 1);

            if (StepApprStatus.ToUpper() == "A")
            {
                return new SqlBoolean(true);
            }
            else
            {
                return new SqlBoolean(false);
            }
        }
        else
        {
            return new SqlBoolean(false);
        }
    }
    */
    #endregion

    #region "IsHdrPending"
    /*
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlBoolean IsHdrPending(string ApprStatus, int Step)
    {
        if (ApprStatus.Length > 0)
        {
            string StepApprStatus = ApprStatus.Substring(Step - 1, 1);

            if (StepApprStatus.ToUpper() == "_" || StepApprStatus.ToUpper() == "P")
            {
                return new SqlBoolean(true);
            }
            else
            {
                return new SqlBoolean(false);
            }
        }
        else
        {
            return new SqlBoolean(false);
        }
    }
     * */
    #endregion
};

