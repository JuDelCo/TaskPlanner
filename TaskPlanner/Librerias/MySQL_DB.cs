using System;
using System.Data;
using System.Windows.Forms;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace TaskPlanner
{
    public class MySQL_DB
    {
        #region Variables
            private MySqlConnection mysql_connection = new MySqlConnection();
            private MySqlDataAdapter data_adapter = new MySqlDataAdapter();
            private DataSet dataSet = null;
            private bool Connected = false;
            private string tableName, primaryKey;
            private string selectStr, insertStr, updateStr, deleteStr;

            public bool debug = false;
        #endregion

        #region Constructor
            public MySQL_DB() {}
        #endregion

        #region Métodos
            public bool Conectar()
            {
                try
                {
                    if (!isConnected())
                    {
                        mysql_connection.Open();
                        dataSet = new DataSet("DataSet");
                        data_adapter = new MySqlDataAdapter(selectStr, mysql_connection);

                        if (!string.IsNullOrEmpty(tableName)) data_adapter.Fill(dataSet, tableName);
                        if (!string.IsNullOrEmpty(primaryKey)) dataSet.Tables[tableName].PrimaryKey = new DataColumn[] { dataSet.Tables[tableName].Columns[primaryKey] };
                        if (!string.IsNullOrEmpty(insertStr)) data_adapter.InsertCommand = new MySqlCommand(insertStr, mysql_connection);
                        if (!string.IsNullOrEmpty(updateStr)) data_adapter.UpdateCommand = new MySqlCommand(updateStr, mysql_connection);
                        if (!string.IsNullOrEmpty(deleteStr)) data_adapter.DeleteCommand = new MySqlCommand(deleteStr, mysql_connection);

                        Connected = true;
                    }
                    else
                    {
                        Desconectar();
                        return Conectar();
                    }
                }
                catch (Exception ex)
                {
                    if(debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible conectar con la base de datos");
                }
                return Connected;
            }
            public void Desconectar()
            {
                try
                {
                    if (isConnected())
                    {
                        mysql_connection.Close();
                        Connected = false;
                    }
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible desconectarse de la base de datos");
                }
            }
            public bool isConnected()
            {
                return this.Connected;
            }
            public bool testConnection()
            {
                try
                {
                    if (!isConnected())
                    {
                        mysql_connection.Open();
                        mysql_connection.Close();
                    }
                    return true;
                }
                catch (Exception) { }
                return false;
            }
            public void ChangeSelect(string selectStr)
            {
                this.selectStr = selectStr;
            }
            public void ChangeTable(string tableName, string primaryKey)
            {
                this.tableName = tableName;
                this.primaryKey = primaryKey;
            }
            public void SetConnection(string server, string port, string user, string password, string database)
            {
                try
                {
                    if (string.IsNullOrEmpty(server)) server = "localhost";
                    if (string.IsNullOrEmpty(port)) port = "3306";
                    if (string.IsNullOrEmpty(user)) user = "root";
                    if (string.IsNullOrEmpty(password)) password = "";
                    if (string.IsNullOrEmpty(database)) database = "database";
                    string newConnectionString = "server=" + server + ";user=" + user + ";database=" + database + ";port=" + port + ";password=" + password + ";";
                    mysql_connection.ConnectionString = newConnectionString;
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible modificar los parámetros de conexión");
                }
            }
            public int countRows()
            {
                try
                {
                    return dataSet.Tables[tableName].Rows.Count;
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible contar el número de filas");
                }
                return 0;
            }
            public int countCols()
            {
                try
                {
                    return dataSet.Tables[tableName].Columns.Count;
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible contar el número de columnas");
                }
                return 0;
            }
            public string getData(int row, int col)
            {
                string data = "null";
                try
                {
                    if ((row <= countRows()) && (col <= countCols())) data = dataSet.Tables[tableName].Rows[row].ItemArray[col].ToString();
                    return data;
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible extraer información de la base de datos");
                }
                return data;
            }
            public void Insert_String(string insertStr)
            {
                try
                {
                    this.insertStr = insertStr;
                    if(isConnected()) data_adapter.InsertCommand = new MySqlCommand(insertStr, mysql_connection);
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible modificar la cadena de inserción SQL");
                }
            }
            public int Insert_Execute()
            {
                try
                {
                    return data_adapter.InsertCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible ejecutar la sentencia SQL");
                }
                return -1;
            }
            public void Update_String(string updateStr)
            {
                try
                {
                    this.updateStr = updateStr;
                    if (isConnected()) data_adapter.UpdateCommand = new MySqlCommand(updateStr, mysql_connection);
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible modificar la cadena de actualización SQL");
                }
            }
            public int Update_Execute()
            {
                try
                {
                    return data_adapter.UpdateCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible ejecutar la sentencia SQL");
                }
                return -1;
            }
            public void Delete_String(string deleteStr)
            {
                try
                {
                    this.deleteStr = deleteStr;
                    if (isConnected()) data_adapter.DeleteCommand = new MySqlCommand(deleteStr, mysql_connection);
                                    }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible modificar la cadena de borrado SQL");
                }
            }
            public int Delete_Execute()
            {
                try
                {
                    return data_adapter.DeleteCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    if (debug) MessageBox.Show(ex.ToString());
                    else MessageBox.Show("Error, no ha sido posible ejecutar la sentencia SQL");
                }
                return -1;
            }
        #endregion
    }
}

//-------------------------------------------------
//ExecuteNonQuery() ejecuta una sentencia SQL y regresa el número de filas afectadas (int) (Para INSERT, DELETE, UPDATE, etc)
//ExecuteReader() manda la consulta y construye un objeto MySqlDataReader (Solo para SELECT)

//NOTA:
//Mientras que el objeto MySqlDataReader está en uso, la conexión asociada está ocupada atendiendo a este objeto, 
//y ninguna otra operación puede ser ejecutada sobre la conexión, si acaso, cerrarla. Este es el caso de cuando de llama al método MySqlDataReader.Close(). 
//-------------------------------------------------