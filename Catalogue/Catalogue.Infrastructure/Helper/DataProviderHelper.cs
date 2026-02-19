using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Helper
{
    internal class DataProviderHelper
    {
        internal async Task<BaseResponse<T>> ExecuteReaderAsync<T>(string connectionString, string storedProc, Func<DbDataReader, Task<T>> readerParserAction, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandTimeout = 180;
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        }

                        T response;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            response = await readerParserAction(reader);
                        }

                        Dictionary<string, object> dict = new Dictionary<string, object>
                            {
                                {"message", "NO_DATA_FOUND"}
                            };
                        return new BaseResponse<T>()
                        {
                            code = response != null ? 1 : -1,
                            message = "NO_DATA_FOUND",
                            data = response
                            //extension = response != null ? null : dict
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.CloseAsync();
                        command.Parameters.Clear();
                    }
                }
            }
        }

        internal async Task<BaseResponse<T>> ExecuteReaderAsync<T>(string connectionString, string storedProc, Func<DbDataReader, Task<T>> readerParserAction, MySqlParameter output = null, MySqlParameter newid = null, MySqlParameter message = null, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandTimeout = 180;
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        }

                        if (output != null) command.Parameters.Add(output);
                        if (newid != null) command.Parameters.Add(newid);
                        if (message != null) command.Parameters.Add(message);

                        T response;
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            response = await readerParserAction(reader);
                        }

                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        if (message != null)
                            dict = new Dictionary<string, object>
                            {
                                {"message", Convert.ToString(command.Parameters["@message"].Value)}
                            };

                        return new BaseResponse<T>()
                        {
                            code = output != null ? Convert.ToInt32(command.Parameters["@output"].Value) : 1,
                            message = message != null ? Convert.ToString(command.Parameters["@message"].Value) : "",
                            data = response
                            //extension = dict
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.CloseAsync();
                        command.Parameters.Clear();
                    }
                }
            }
        }

        internal T ExecuteReader<T>(string connectionString, string storedProc, Func<DbDataReader, T> readerParserAction, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandTimeout = 180;
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        }
                        using (var reader = command.ExecuteReader())
                        {
                            return readerParserAction(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.Close();
                        command.Parameters.Clear();
                    }
                }
            }
        }

        internal async Task ExecuteNonQueryAsync(string connectionString, string storedProc, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(connectionString))
            {

                await connection.OpenAsync();

                using (var command = new MySqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 180;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        }

                        await command.ExecuteNonQueryAsync();
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.CloseAsync();
                        command.Parameters.Clear();
                    }
                }
            }
        }

        internal async Task<BaseResponse<long>> ExecuteNonQueryAsync(string connectionString, string storedProc, MySqlParameter output = null, MySqlParameter newid = null, MySqlParameter message = null, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new MySqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 180;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        }

                        if (output != null) command.Parameters.Add(output);
                        if (newid != null) command.Parameters.Add(newid);
                        if (message != null) command.Parameters.Add(message);

                        await command.ExecuteNonQueryAsync();

                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        if (message != null)
                            dict = new Dictionary<string, object>
                            {
                                {"message", Convert.ToString(command.Parameters["@message"].Value)}
                            };

                        return new BaseResponse<long>()
                        {
                            code = output != null ? Convert.ToInt32(command.Parameters["@output"].Value) : 1,
                            message = message != null ? Convert.ToString(command.Parameters["@message"].Value) : "",
                            data = newid != null ? Convert.ToInt64(command.Parameters["@newid"].Value) : 0
                            //extension = dict
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.CloseAsync();
                        command.Parameters.Clear();
                    }
                }
            }
        }

        internal async Task<BaseResponse<object>> ExecuteScalerAsync(string connectionString, string storedProc, MySqlParameter output = null, MySqlParameter newid = null, MySqlParameter message = null, params MySqlParameter[] parameters)
        {
            using (var connection = new MySqlConnection(connectionString))
            {

                await connection.OpenAsync();

                using (var command = new MySqlCommand(storedProc, connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.ParameterName, parameter.Value);
                        }
                        var value = await command.ExecuteScalarAsync();

                        Dictionary<string, object> dict = new Dictionary<string, object>();
                        if (message != null)
                            dict = new Dictionary<string, object>
                            {
                                {"message", Convert.ToString(command.Parameters["@message"].Value)}
                            };

                        return new BaseResponse<object>()
                        {
                            code = output != null ? Convert.ToInt32(command.Parameters["@output"].Value) : 1,
                            message = message != null ? Convert.ToString(command.Parameters["@message"].Value) : "",
                            data = value
                            //extension = dict
                        };
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State != ConnectionState.Closed)
                            connection.CloseAsync();
                        command.Parameters.Clear();
                    }
                }
            }
        }

    }
}
