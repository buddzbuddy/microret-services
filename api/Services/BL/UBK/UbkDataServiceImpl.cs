using api.Contracts.BL.UBK;
using api.Domain;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace api.Services.BL.UBK
{
    public class UbkDataServiceImpl : IUbkDataService
    {
        private readonly IConfiguration _configuration;
        public UbkDataServiceImpl(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<int> InsertSrcJsonToDb(string srcJson)
        {
            var connectionString = _configuration.GetConnectionString("operationalDb");
            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = insertSrcJsonSqlTmpl;
            cmd.Parameters.Add(new() { ParameterName = "@json_data", Value = srcJson });
            var newId = await cmd.ExecuteScalarAsync();
            await conn.CloseAsync();
            return (int)newId;
        }


        const string insertSrcJsonSqlTmpl = @"
insert into public.ubk_packages (json_data)
values(CAST(@json_data AS json)) RETURNING id;
";
    }
}
