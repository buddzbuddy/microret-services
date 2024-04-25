using api.Contracts.BL.ESP;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;

namespace api.Services.BL.ESP
{
    public class EspDataServiceImpl : IEspDataService
    {
        private readonly IConfiguration _configuration;

        public EspDataServiceImpl(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> GetOriginAppID(Guid appId)
        {
            var connectionString = _configuration.GetConnectionString("operationalDb");
            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = fetchLastJsonDataByAppId;
            cmd.Parameters.Add(new() { ParameterName = "@cissa_app_id", Value = appId });
            var jsonDataObj = await cmd.ExecuteScalarAsync();
            await conn.CloseAsync();

            if (jsonDataObj != null)
            {
                var jsonDataStr = jsonDataObj.ToString();
                var _responseType = new { ID = (int?)123 };
                var deserializedJsonData = JsonConvert.DeserializeAnonymousType(jsonDataStr ?? "{}", _responseType);
                if (deserializedJsonData != null)
                {
                    return deserializedJsonData.ID ?? throw new ArgumentNullException(
                        $"{nameof(deserializedJsonData)}.{nameof(deserializedJsonData.ID)}",
                        ErrorMessageResource.NullDataProvidedError);
                }
                else
                    throw new ArgumentNullException(nameof(deserializedJsonData), ErrorMessageResource.NullDataProvidedError);
            }
            else
                throw new ArgumentNullException(nameof(jsonDataObj), ErrorMessageResource.NullDataProvidedError);
        }

        public async Task<int> SaveJson(string srcJson)
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

        public async Task UpdatePackageInfo(int pkgId, string regNo, Guid appId)
        {
            var connectionString = _configuration.GetConnectionString("operationalDb");
            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = updatePackageInfoSetRegNoAppIdSqlTmpl;
            cmd.Parameters.Add(new() { ParameterName = "@pkg_id", Value = pkgId });
            cmd.Parameters.Add(new() { ParameterName = "@reg_no", Value = regNo });
            cmd.Parameters.Add(new() { ParameterName = "@cissa_app_id", Value = appId });
            var newId = await cmd.ExecuteScalarAsync();
            await conn.CloseAsync();
        }

        public async Task UpdatePackageInfo(Guid appId, string decision, string rejectionReason)
        {
            var connectionString = _configuration.GetConnectionString("operationalDb");
            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = updatePackageInfoSetDecisionRejectionReasonSqlTmpl;
            cmd.Parameters.Add(new() { ParameterName = "@cissa_app_id", Value = appId });
            cmd.Parameters.Add(new() { ParameterName = "@decision", Value = decision });
            cmd.Parameters.Add(new() { ParameterName = "@rejection_reason", Value = rejectionReason });
            var newId = await cmd.ExecuteScalarAsync();
            await conn.CloseAsync();
        }

        const string fetchLastJsonDataByAppId = @"
select json_data from esp_packages p
where p.cissa_app_id = @cissa_app_id
order by id desc
limit 1;
";
        const string insertSrcJsonSqlTmpl = @"
insert into public.esp_packages (json_data)
values(CAST(@json_data AS json)) RETURNING id;
";
        /// <summary>
        /// Params: @reg_no: string, @cissa_app_id: Guid, @pkg_id: int
        /// </summary>
        const string updatePackageInfoSetRegNoAppIdSqlTmpl = @"
update public.esp_packages
set reg_no = @reg_no, cissa_app_id = @cissa_app_id
where id = @pkg_id;
";
        /// <summary>
        /// Params: @reg_no: string, @cissa_app_id: Guid, @pkg_id: int
        /// </summary>
        const string updatePackageInfoSetDecisionRejectionReasonSqlTmpl = @"
update public.esp_packages
set decision = @decision, rejection_reason = @rejection_reason
where cissa_app_id = @cissa_app_id;
";
    }
}
