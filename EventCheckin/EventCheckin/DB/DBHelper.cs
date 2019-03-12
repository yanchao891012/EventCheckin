using EventCheckin.Model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventCheckin.DB
{
    class DBHelper
    {
        static SQLiteConnection db_Connection;
        /// <summary>
        /// 连接数据库
        /// </summary>
        /// <returns></returns>
        static bool ConnectionToDataBase()
        {
            db_Connection = new SQLiteConnection("Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\RegisterDB.db;Version=3");
            if (db_Connection.State != System.Data.ConnectionState.Open)
            {
                db_Connection.Open();
            }
            return true;
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        static void CommandToTable(string sql)
        {
            try
            {
                if (ConnectionToDataBase())
                {
                    SQLiteCommand db_Command = new SQLiteCommand(sql, db_Connection);
                    db_Command.ExecuteNonQuery();
                    db_Command.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }
        /// <summary>
        /// 查询业务员
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static List<SalesManEntity> SelectSalesMan(string condition = "")
        {
            try
            {
                ConnectionToDataBase();
                List<SalesManEntity> list = new List<SalesManEntity>();
                string sql = "Select * From SalesMan_TB";
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }
                SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SalesManEntity salesMan = new SalesManEntity
                    {
                        ID = (long)reader["ID"],//注意：数据库里是Integer类型，对应C#里是Long，如果用Int会出错
                        Name = reader["Name"].ToString(),
                        ImageName = reader["ImageName"].ToString(),
                        TableNo = reader["TableNo"].ToString()
                    };
                    list.Add(salesMan);
                }
                command.Dispose();
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }
        /// <summary>
        /// 插入业务员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="imagename"></param>
        public static void InsertSalesMans(string name, string imagename, string tableno)
        {
            string sql = "Insert into SalesMan_TB(Name,ImageName,TableNo) values ('" + name + "','" + imagename + "','" + tableno + "')";
            CommandToTable(sql);
        }
        /// <summary>
        /// 更新业务员
        /// </summary>
        /// <param name="name"></param>
        /// <param name="imagename"></param>
        /// <param name="id"></param>
        public static void UpdateSalesMans(string name, string imagename, int id)
        {
            string sql = "Update SalesMan_TB set Name='" + name + "',ImageName='" + imagename + "' where ID=" + id + "";
            CommandToTable(sql);
        }
        /// <summary>
        /// 删除业务员
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteSalesMans(long id)
        {
            if (ConnectionToDataBase())
            {
                using (SQLiteTransaction trans = db_Connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(db_Connection))
                    {
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.CommandText = "update Customer_TB set SalesManID=-1 where SalesManID=" + id + "";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "Delete From SalesMan_TB where ID=" + id + "";
                            cmd.ExecuteNonQuery();

                            trans.Commit();
                            cmd.Dispose();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw (ex);
                        }
                        finally
                        {
                            db_Connection.Close();
                            db_Connection.Dispose();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 查询顾客
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static List<CustomEntity> SelectCustom(string condition = "")
        {
            try
            {
                ConnectionToDataBase();
                List<CustomEntity> list = new List<CustomEntity>();
                string sql = "Select * from Customer_TB";
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }
                SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CustomEntity custom = new CustomEntity()
                    {
                        ID = (long)reader["ID"],
                        Name = reader["Name"].ToString(),
                        PhoneNum = reader["PhoneNum"].ToString(),
                        SalesManID = (int)reader["SalesManID"],
                        TableNo = reader["TableNo"].ToString()
                    };
                    list.Add(custom);
                }
                command.Dispose();
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }
        /// <summary>
        /// 插入客户
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phoneNum"></param>
        /// <param name="salesManID"></param>
        public static void InsertCustom(string name, string phoneNum, long salesManID, string tableNo)
        {
            string sql = "Insert into Customer_TB(Name,PhoneNum,SalesManID,TableNo) values ('" + name + "','" + phoneNum + "'," + salesManID + ",'" + tableNo + "')";
            CommandToTable(sql);
        }
        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="condition"></param>
        public static void DeleteCustom(string condition = "")
        {
            string sql = "Delete from Customer_TB";
            if (!string.IsNullOrEmpty(condition))
            {
                sql += " where " + condition;
            }
            CommandToTable(sql);
        }
        /// <summary>
        /// 添加活动信息
        /// </summary>
        /// <param name="companyName"></param>
        /// <param name="eventName"></param>
        public static void InsertActivityInfo(string companyName, string eventName)
        {
            string sql = "Insert into ActivityInfo_TB(ComponyName,EventName) values ('" + companyName + "','" + eventName + "')";
            CommandToTable(sql);
        }
        /// <summary>
        /// 启用活动
        /// </summary>
        /// <param name="id"></param>
        public static void UpdateActivityInfo(long id)
        {
            if (ConnectionToDataBase())
            {
                using (SQLiteTransaction trans = db_Connection.BeginTransaction())
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(db_Connection))
                    {
                        cmd.Transaction = trans;

                        try
                        {
                            cmd.CommandText = "update ActivityInfo_TB set Enabled=False";
                            cmd.ExecuteNonQuery();
                            cmd.CommandText = "update ActivityInfo_TB set Enabled=True where ID=" + id + "";
                            cmd.ExecuteNonQuery();

                            trans.Commit();
                            cmd.Dispose();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw (ex);
                        }
                        finally
                        {
                            db_Connection.Close();
                            db_Connection.Dispose();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 删除活动消息
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteActivityInfo(long id)
        {
            string sql = "Delete From ActivityInfo_TB where ID=" + id + "";
            CommandToTable(sql);
        }
        /// <summary>
        /// 查询活动信息
        /// </summary>
        public static List<ActivityInfoEntity> SelectActivityInfo(string condition = "")
        {
            try
            {
                ConnectionToDataBase();
                List<ActivityInfoEntity> list = new List<ActivityInfoEntity>();
                string sql = "Select * From ActivityInfo_TB ";
                if (!string.IsNullOrEmpty(condition))
                {
                    sql += " where " + condition;
                }
                SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ActivityInfoEntity activityInfoEntity = new ActivityInfoEntity()
                    {
                        ID = (long)reader["ID"],
                        ComponyName = reader["ComponyName"].ToString(),
                        EventName = reader["EventName"].ToString(),
                        Enabled = (bool)reader["Enabled"]
                    };
                    list.Add(activityInfoEntity);
                }
                command.Dispose();
                reader.Close();
                return list;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }
        /// <summary>
        /// 查询桌上有多少个客户
        /// </summary>
        /// <param name="tableno"></param>
        /// <returns></returns>
        public static int SelectCountTableNoCustoms(string tableno)
        {
            try
            {
                ConnectionToDataBase();
                int num = -1;
                string sql = "Select Count(*) as num from Customer_TB Where TableNo='" + tableno + "'";
                SQLiteCommand command = new SQLiteCommand(sql, db_Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    num = Int32.Parse(reader["num"].ToString());
                }
                command.Dispose();
                reader.Close();
                return num;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                db_Connection.Close();
                db_Connection.Dispose();
            }
        }
    }
}
