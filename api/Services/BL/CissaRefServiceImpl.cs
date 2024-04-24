using api.Contracts.BL;
using api.Domain;
using api.Models.BL;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace api.Services.BL
{
    public class CissaRefServiceImpl : ICissaRefService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CissaRefServiceImpl> _logger;
        public CissaRefServiceImpl(IConfiguration configuration, ILogger<CissaRefServiceImpl> logger) {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<double> GetGMI(gmiRequestDTO requestDTO)
        {
            var connectionString = _configuration.GetConnectionString("cissaDb");
            //_logger.LogInformation("connectionString: {0}", connectionString);
            using var conn = new SqlConnection(connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = 10;
            var finalSql = string.Format(fetchGMIByPeriodSqlTmpl, requestDTO.year, requestDTO.month);
            cmd.CommandText = finalSql;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.Read())
            {
                return reader.IsDBNull(0) ? 0.0 : (double)reader.GetDecimal(0);
            }
            throw new DomainException($"ГМД не найден за выбранный период: {requestDTO.year}-{requestDTO.month}", "GMI_NOT_FOUND");
        }

        /// <summary>
        /// 0-year,1-month
        /// </summary>
        const string fetchGMIByPeriodSqlTmpl = @"
SELECT ca.[Value] as gmi FROM Documents d 
inner join Int_Attributes yearAttr on yearAttr.Document_Id = d.Id and yearAttr.Def_Id  = '3F982405-AAB8-4C8B-A166-F68E710B68F1' and yearAttr.Expired = '99991231' and yearAttr.[Value] = {0}
inner join Int_Attributes monthAttr on monthAttr.Document_Id = d.Id and monthAttr.Def_Id  = 'C9EFBD5A-7527-4113-8009-EA886527BBE5' and monthAttr.Expired = '99991231' and monthAttr.[Value] = {1}
inner join Currency_Attributes ca on ca.Document_Id = d.Id and ca.Def_Id  = '8EF7B5EA-4441-40B0-9829-ED857B586D5D' and ca.Expired = '99991231'
WHERE d.Def_Id = 'F2193444-C4F5-4C8F-9B2F-4953BB1AADE6'
";
    }
}
