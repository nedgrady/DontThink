using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    namespace Database
    {
        /*class StoredProcedure
            :IDisposable
        {

            private string _spName;
            private Action _dispose;
            private SqlParameter[] _parameters;
            private SqlCommand _sqlCommand;
            public SqlParameterCollection Parameters => _sqlCommand.Parameters;

            public StoredProcedure(string spName, SqlParameter[] parameters = null)
            {
                _spName = spName;
                _parameters = parameters;
            }

            public async Task<SqlDataReader> ExecuteAsync()
            {
                (SqlDataReader reader, SqlCommand command, Action action) resultTuple = DatabaseHelper.E;


                SqlDataReader results = resultTuple.reader;
                _sqlCommand = resultTuple.command;
                _dispose = resultTuple.action;
                return results;
            }

            #region IDisposable Members
            public void Dispose()
            {
                _dispose?.Invoke();
            }
            #endregion
        }


    }*/
    }
}