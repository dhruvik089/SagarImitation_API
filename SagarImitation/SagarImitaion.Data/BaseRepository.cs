using Dapper;
using Microsoft.Extensions.Options;
using SagarImitation.Common.Helpers;
using SagarImitation.Model.Config;
using System.Data;
using System.Data.SqlClient;

namespace SagarImitation.Data
{
    public abstract class BaseRepository
    {
        #region Fields
        public readonly IOptions<ConnectionString> _connectionString;
        #endregion

        #region Constructor
        public BaseRepository(IOptions<ConnectionString> connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region SQL Methods

            string encryptedConn = "Server=localhost\\MSSQLSERVER02;Database=SagarImitation;Trusted_Connection=True;";
        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            //string decryptedConn = EncryptionDecryption.GetDecrypt(_connectionString.Value.DefaultConnection);
            
            using (SqlConnection con = new SqlConnection(encryptedConn))
            {
                await con.OpenAsync();
                return await con.QueryFirstOrDefaultAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            //string decryptedConn = EncryptionDecryption.GetDecrypt(_connectionString.Value.DefaultConnection);
            using (SqlConnection con = new SqlConnection(encryptedConn))
            {
                await con.OpenAsync();
                return await con.QueryAsync<T>(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<int> ExecuteAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            //string decryptedConn = EncryptionDecryption.GetDecrypt(_connectionString.Value.DefaultConnection);
            using (SqlConnection con = new SqlConnection(encryptedConn))
            {
                await con.OpenAsync();
                return await con.ExecuteAsync(sql, param, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<SqlMapper.GridReader> QueryMultipleAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            //string decryptedConn = EncryptionDecryption.GetDecrypt(_connectionString.Value.DefaultConnection);
            using (SqlConnection con = new SqlConnection(encryptedConn))
            {
                await con.OpenAsync();
                return await con.QueryMultipleAsync(sql, param, commandType: CommandType.StoredProcedure);
            }
        }
        #endregion
    }
}