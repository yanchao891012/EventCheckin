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
                        ImageName = reader["ImageName"].ToString()
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
        public static void InsertSalesMans(string name, string imagename)
        {
            string sql = "Insert into SalesMan_TB(Name,ImageName) values ('" + name + "','" + imagename + "')";
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
                        SalesManID = (int)reader["SalesManID"]
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
        public static void InsertCustom(string name, string phoneNum, long salesManID)
        {
            string sql = "Insert into Customer_TB(Name,PhoneNum,SalesManID) values ('" + name + "','" + phoneNum + "'," + salesManID + ")";
            CommandToTable(sql);
        }
        /// <summary>
        /// 删除客户
        /// </summary>
        /// <param name="condition"></param>
        public static void DeleteCustom(string condition="")
        {
            string sql = "Delete from Customer_TB";
            if (!string.IsNullOrEmpty(condition))
            {
                sql += " where " + condition;
            }
            CommandToTable(sql);
        }
    }
}
