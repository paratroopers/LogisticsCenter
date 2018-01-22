﻿using Akmii.Core.DataAccess;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using Logistics_Model;
using Logistics.Common;

namespace Logistics_DAL
{
    public class CustomerOrderStatus
    {
        public static OrderStatusSummaryView SelectOrderStatusByUserID(long userID, long TenantID = BusinessConstants.Admin.TenantID)
        {
            var result = new OrderStatusSummaryView();
            MySqlParameter[] parameters = {
                new MySqlParameter("@_TenantID",TenantID),
                new MySqlParameter("@_userID", userID),
            };

            var dbResult = AkmiiMySqlHelper.GetDataSet(ConnectionManager.GetWriteConn(), CommandType.StoredProcedure, Proc.CustomerOrderStatus.logistics_order_select_by_userid_summary, parameters);
            if (dbResult.Tables.Count > 0 && dbResult.Tables[0].Rows.Count > 0)
            {
                result = ConvertHelper<OrderStatusSummaryView>.DtToModel(dbResult.Tables[0]);
            }
            else
            {
                result = null;
            }

            return result;
        }


        public static bool Insert(logistics_customer_order_status model, AkmiiMySqlTransaction trans = null)
        {

            MySqlParameter[] parameters = {
                        new MySqlParameter("@_TenantID", model.TenantID),
                        new MySqlParameter("@_ID",model.ID),
                        new MySqlParameter("@_orderid", model.OrderID),
                        new MySqlParameter("@_currentstep",model.currentStep),
                        new MySqlParameter("@_currentStatus",model.currentStatus),
                        new MySqlParameter("@_CreatedBy",model.CreatedBy),
            };

            int result = 0;
            if (trans == null)
            {
                result = AkmiiMySqlHelper.ExecuteNonQuery(ConnectionManager.GetWriteConn(), CommandType.StoredProcedure, Proc.CustomerOrderStatus.logistics_customer_order_status_insert, parameters);
            }
            else
            {
                result = AkmiiMySqlHelper.ExecuteNonQuery(trans, CommandType.StoredProcedure, Proc.CustomerOrderStatus.logistics_customer_order_status_insert, parameters);
            }
            return result == 1;

        }
    }
}
