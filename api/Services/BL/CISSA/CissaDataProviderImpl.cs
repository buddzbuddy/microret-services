using api.Contracts.BL.CISSA;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Models.Enums;
using api.Utils;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace api.Services.BL.CISSA
{
    public class CissaDataProviderImpl : ICissaDataProvider
    {
        private readonly IAddressApiHelper _addressApiHelper;
        private readonly IConfiguration _configuration;
        private readonly IDataHelper _dataHelper;
        public CissaDataProviderImpl(IAddressApiHelper addressApiHelper, IConfiguration configuration,
            IDataHelper dataHelper)
        {
            _addressApiHelper = addressApiHelper;
            _configuration = configuration;
            _dataHelper = dataHelper;
        }

        public async Task<(string regNo, Guid appId)>
            CreateCissaApplication(PersonDetailsInfo applicantPerson, Guid paymentType)
        {
            (var orgId, var userId, var orgPositionId, var orgCode, var regNoLastValue) = await
                DefineUserCredsFromAddressData(applicantPerson.ResidentialAddress);
            
            string regNo = regNoLastValue.ToString();
            while (regNo.Length < 6) regNo = "0" + regNo;
            regNo = $"{orgCode}{regNo}-TUN";

            var model = BuildApplication(applicantPerson, regNo, paymentType);

            var sqlModel = BuildInsertModel(model, orgId, userId, orgPositionId);

            await ExecuteSql(sqlModel);
            return (regNo, model.Id);
        }


        private SocialApplicationDTO BuildApplication(PersonDetailsInfo applicantPerson, string regNo, Guid paymentType)
        {
            var model = new SocialApplicationDTO
            {
                RegNo = regNo,
                RegDate = DateTime.Today,
                ApplyType = StaticCissaReferences.PrimaryApplicationType,
                Applicant = new PersonSheetDTO
                {
                    PIN = applicantPerson.pin!,
                    LastName = applicantPerson.PassportDataInfo!.Surname!,
                    FirstName = applicantPerson.PassportDataInfo!.Name!,
                    MiddleName = applicantPerson.PassportDataInfo!.Patronymic,
                    BirthDate = applicantPerson.PassportDataInfo!.DateOfBirth!.Value,
                    Sex = _dataHelper.GetGender(applicantPerson.pin) == GenderType.MALE
                    ? StaticCissaReferences.MALE : StaticCissaReferences.FEMALE,
                    DocumentType = StaticCissaReferences.PASSPORT_DOCUMENT_TYPE,
                    PassportSeries = GetPassportSeries(applicantPerson.PassportDataInfo!.PassportSeries),
                    PassportNo = applicantPerson.PassportDataInfo!.PassportNumber!,
                    PassportDate = applicantPerson.PassportDataInfo.IssuedDate!.Value,
                    PassportExpiryDate = applicantPerson.PassportDataInfo?.ExpiredDate,
                    PassportOrg = applicantPerson.PassportDataInfo?.PassportAuthority
                },
                PaymentType = paymentType,
                Town = applicantPerson.ResidentialAddress?.Region,
                Street = applicantPerson.ResidentialAddress?.Street,
                House = applicantPerson.ResidentialAddress?.House,
                Apartment = applicantPerson.ResidentialAddress?.Flat,
                MobilePhone = applicantPerson.PassportDataInfo?.PhoneNumber
            };
            return model;
        }
        private Guid GetPassportSeries(string? passportSeries) => passportSeries switch
        {
            "ID" => StaticCissaReferences.PASSPORT_SERIES_ID,
            "AN" => StaticCissaReferences.PASSPORT_SERIES_AN,
            "AC" => StaticCissaReferences.PASSPORT_SERIES_AC,
            _ => throw new ArgumentException(ErrorMessageResource.IllegalDataProvidedError,
                                    nameof(passportSeries)),
        };

        private socialApplicationInsertModel BuildInsertModel(SocialApplicationDTO socialApplication,
            Guid orgId, Guid userId, Guid orgPositionId)
        {
            var model = new socialApplicationInsertModel
            {
                OrgId = orgId, PositionId = orgPositionId, UserId = userId
            };
            var created = DateTime.Now;

            //Applicant Document
            model.insertDocuments.Add(new InsertDocumentItemModel
            {
                Id = socialApplication.Applicant.Id,
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>(),
                Organization_Id = orgId,
                Org_Position_Id = orgPositionId,
                UserId = userId
            });
            model.insertDocuments.Add(new InsertDocumentItemModel
            {
                Id = socialApplication.Id,
                Created = created,
                Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>(),
                Organization_Id = orgId,
                Org_Position_Id = orgPositionId,
                UserId = userId
            });

            //Applicant Data attrs
            model.textAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                (nameof(socialApplication.Applicant.PIN)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.PIN
            });
            model.textAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                (nameof(socialApplication.Applicant.LastName)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.LastName
            });
            model.textAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                (nameof(socialApplication.Applicant.FirstName)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.FirstName
            });
            if (!string.IsNullOrEmpty(socialApplication.Applicant.MiddleName))
                model.textAttributes.Add(new()
                {
                    Created = created,
                    Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                    (nameof(socialApplication.Applicant.MiddleName)),
                    Document_Id = socialApplication.Applicant.Id,
                    UserId = userId,
                    Value = socialApplication.Applicant.MiddleName
                });
            model.textAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                (nameof(socialApplication.Applicant.PassportNo)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.PassportNo
            });
            if (!string.IsNullOrEmpty(socialApplication.Applicant.PassportOrg))
                model.textAttributes.Add(new()
                {
                    Created = created,
                    Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                    (nameof(socialApplication.Applicant.PassportOrg)),
                    Document_Id = socialApplication.Applicant.Id,
                    UserId = userId,
                    Value = socialApplication.Applicant.PassportOrg
                });

            model.dateAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                    (nameof(socialApplication.Applicant.BirthDate)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.BirthDate
            });
            model.dateAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                    (nameof(socialApplication.Applicant.PassportDate)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.PassportDate
            });
            if(socialApplication.Applicant.PassportExpiryDate != null)
                model.dateAttributes.Add(new()
                {
                    Created = created,
                    Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                        (nameof(socialApplication.Applicant.PassportExpiryDate)),
                    Document_Id = socialApplication.Applicant.Id,
                    UserId = userId,
                    Value = socialApplication.Applicant.PassportExpiryDate.Value
                });

            model.enumAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                        (nameof(socialApplication.Applicant.Sex)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.Sex
            });
            model.enumAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                        (nameof(socialApplication.Applicant.DocumentType)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.DocumentType
            });
            model.enumAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<PersonSheetDTO>
                        (nameof(socialApplication.Applicant.PassportSeries)),
                Document_Id = socialApplication.Applicant.Id,
                UserId = userId,
                Value = socialApplication.Applicant.PassportSeries
            });

            //Application data-attrs
            model.textAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                (nameof(socialApplication.RegNo)),
                Document_Id = socialApplication.Id,
                UserId = userId,
                Value = socialApplication.RegNo
            });
            if (!string.IsNullOrEmpty(socialApplication.Town))
                model.textAttributes.Add(new()
                {
                    Created = created,
                    Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                    (nameof(socialApplication.Town)),
                    Document_Id = socialApplication.Id,
                    UserId = userId,
                    Value = socialApplication.Town
                });
            if (!string.IsNullOrEmpty(socialApplication.Street))
                model.textAttributes.Add(new()
                {
                    Created = created,
                    Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                    (nameof(socialApplication.Street)),
                    Document_Id = socialApplication.Id,
                    UserId = userId,
                    Value = socialApplication.Street
                });
            if (!string.IsNullOrEmpty(socialApplication.House))
                model.textAttributes.Add(new()
                {
                    Created = created,
                    Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                    (nameof(socialApplication.House)),
                    Document_Id = socialApplication.Id,
                    UserId = userId,
                    Value = socialApplication.House
                });
            if (!string.IsNullOrEmpty(socialApplication.Apartment))
                model.textAttributes.Add(new()
                {
                    Created = created,
                    Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                    (nameof(socialApplication.Apartment)),
                    Document_Id = socialApplication.Id,
                    UserId = userId,
                    Value = socialApplication.Apartment
                });
            if (!string.IsNullOrEmpty(socialApplication.MobilePhone))
                model.textAttributes.Add(new()
                {
                    Created = created,
                    Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                    (nameof(socialApplication.MobilePhone)),
                    Document_Id = socialApplication.Id,
                    UserId = userId,
                    Value = socialApplication.MobilePhone
                });

            model.dateAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                    (nameof(socialApplication.RegDate)),
                Document_Id = socialApplication.Id,
                UserId = userId,
                Value = socialApplication.RegDate
            });

            model.enumAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                        (nameof(socialApplication.ApplyType)),
                Document_Id = socialApplication.Id,
                UserId = userId,
                Value = socialApplication.ApplyType
            });
            model.enumAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                        (nameof(socialApplication.PaymentType)),
                Document_Id = socialApplication.Id,
                UserId = userId,
                Value = socialApplication.PaymentType
            });

            model.intAttributes.Add(new() { Created = created,
            Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                        (nameof(socialApplication.AmountOfAdults)),
            Document_Id = socialApplication.Id,
            UserId = userId,
            Value = socialApplication.AmountOfAdults
            });

            model.intAttributes.Add(new()
            {
                Created = created,
                Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                        (nameof(socialApplication.FamilyMemberCount)),
                Document_Id = socialApplication.Id,
                UserId = userId,
                Value = socialApplication.FamilyMemberCount
            });

            model.docAttributes.Add(new() { Created = created,
                Def_Id = CissaExtensions.GetDefId<SocialApplicationDTO>
                        (nameof(socialApplication.Applicant)),
                Document_Id = socialApplication.Id,
                UserId = userId,
                Value = socialApplication.Applicant.Id
            });

            var onRegisteringStateTypeId = new Guid("{A9CD37C4-A718-4DE1-9E95-EC8EC280C8D4}");
            model.docState = new DocStateItemModel
            {
                Created = created,
                State_Type_Id = onRegisteringStateTypeId,
                Worker_Id = userId,
                Document_Id = socialApplication.Id
            };

            return model;
        }

        public async Task<(Guid orgId, Guid userId, Guid orgPositionId, string orgCode,
            long regNoLastValue)>
            DefineUserCredsFromAddressData(ResidentialAddressDTO? addressData)
        {
            if (addressData == null) throw new ArgumentNullException(nameof(addressData),
                ErrorMessageResource.NullDataProvidedError);

            if (addressData.RegionId == null)
                throw new ArgumentNullException(nameof(addressData.RegionId),
                ErrorMessageResource.NullDataProvidedError);

            (var orgId, var userId) =
                _addressApiHelper.GetUserAndOrgByDistrict(addressData.RegionId.Value);

            var orgPositionIdTask = definePositionByUser(userId);
            var orgCodeTask = defineCodeByOrg(orgId);
            var regNoLastValueTask = defineApplicationRegNoLastValueByOrg(orgId);

            await Task.WhenAll(orgPositionIdTask, orgCodeTask, regNoLastValueTask);

            return (orgId, userId, orgPositionIdTask.Result,
                orgCodeTask.Result, regNoLastValueTask.Result);
        }

        private async Task<Guid> definePositionByUser(Guid userId)
        {
            var connectionString = _configuration.GetConnectionString("cissaDb");
            using var conn = new SqlConnection(connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = 10;
            var finalSql = string.Format(fetchPositionByUserIdSqlTmpl, userId);
            cmd.CommandText = finalSql;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.Read())
            {
                string un = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
                if (un.Contains('*')) throw new ArgumentException(
                    $"User that found in districts.xml is deprecated." +
                    $" Please update user. userId:{userId}");
                return reader.GetGuid(0);
            }
            throw new ArgumentException($"OrgPosition not found for userId: {userId}", nameof(userId));
        }
        private async Task<string> defineCodeByOrg(Guid orgId)
        {
            var connectionString = _configuration.GetConnectionString("cissaDb");
            using var conn = new SqlConnection(connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = 10;
            var finalSql = string.Format(fetchCodeByOrgIdSqlTmpl, orgId);
            cmd.CommandText = finalSql;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.Read())
            {
                string code = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
                if (string.IsNullOrEmpty(code)) throw new ArgumentException(
                    $"Organization with ID: {orgId} has an empty code");
                return code;
            }
            throw new ArgumentException(
                    $"Organization with ID: {orgId} doesn't have code");
        }
        private async Task<long> defineApplicationRegNoLastValueByOrg(Guid orgId)
        {
            var connectionString = _configuration.GetConnectionString("cissaDb");
            using var conn = new SqlConnection(connectionString);
            using var cmd = conn.CreateCommand();
            cmd.CommandTimeout = 10;
            var finalSql = string.Format(fetchApplicationRegNoLastValueByOrgIdSqlTmpl, orgId);
            cmd.CommandText = finalSql;
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (reader.Read())
            {
                if(reader.IsDBNull(0))
                    throw new ArgumentException($"RegNoLastValue is null. " +
                        $"Organization is invalid. orgId: {orgId}");
                long lastValue =  reader.GetInt64(0);
                return lastValue;
            }
            throw new ArgumentException(
                    $"Organization with ID: {orgId} doesn't have RegNoLastValue");
        }
        async Task ExecuteSql(socialApplicationInsertModel model)
        {
            var connectionString = _configuration.GetConnectionString("cissaDb");
            using SqlConnection connection = new(connectionString);
            await connection.OpenAsync();
            using var transaction = connection.BeginTransaction();
            try
            {
                await callInsertDocuments(CollectionHelper.ConvertFrom(model.insertDocuments), connection, transaction);

                if (model.docState == null)
                {
                    throw new ArgumentNullException(nameof(model.docState));
                }
                /*var docStateAttrTask = */await callSetDocState(model.docState.Document_Id, model.docState.State_Type_Id, model.docState.Worker_Id, connection, transaction);
                /*var docAttrTask = */await callInsertAttributes("dbo.Document_Attributes", CollectionHelper.ConvertFrom(model.docAttributes), connection, transaction);
                /*var intAttrTask = */await callInsertAttributes("dbo.Int_Attributes", CollectionHelper.ConvertFrom(model.intAttributes), connection, transaction);
                /*var dateAttrTask = */await callInsertAttributes("dbo.Date_Time_Attributes", CollectionHelper.ConvertFrom(model.dateAttributes), connection, transaction);
                /*var enumAttrTask = */await callInsertAttributes("dbo.Enum_Attributes", CollectionHelper.ConvertFrom(model.enumAttributes), connection, transaction);
                /*var textAttrTask = */
                await callInsertAttributes("dbo.Text_Attributes", CollectionHelper.ConvertFrom(model.textAttributes), connection, transaction);


                //await Task.WhenAll(docAttrTask, intAttrTask, textAttrTask, dateAttrTask, enumAttrTask);

                await transaction.CommitAsync();
                await connection.CloseAsync();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                    await transaction.RollbackAsync();
                throw;
            }
        }
        async Task callInsertDocuments(DataTable dto, SqlConnection connection, SqlTransaction transaction)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = "[dbo].[insertDocuments]";
            cmd.CommandType = CommandType.StoredProcedure;
            var p1 = cmd.CreateParameter();
            p1.TypeName = "[dbo].[InsertDocumentItemModel]";
            p1.Value = dto;
            p1.ParameterName = "@dto";
            cmd.Parameters.Add(p1);
            await cmd.ExecuteNonQueryAsync();
        }
        async Task callInsertAttributes(string tableName, DataTable dto, SqlConnection connection, SqlTransaction transaction)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandTimeout = 300;
            cmd.CommandText = "[dbo].[insertAttributes]";
            cmd.CommandType = CommandType.StoredProcedure;
            var p1 = cmd.CreateParameter();
            p1.TypeName = "[dbo].[AttributeItemModel]";
            p1.Value = dto;
            p1.ParameterName = "@dto";

            var p2 = cmd.CreateParameter();
            p2.ParameterName = "@tableName";
            p2.Value = tableName;
            cmd.Parameters.AddRange(new[] { p1, p2 });
            await cmd.ExecuteNonQueryAsync();
        }
        async Task callSetDocState(Guid docId, Guid stateTypeId, Guid userId, SqlConnection connection, SqlTransaction transaction)
        {
            using var cmd = connection.CreateCommand();
            cmd.Transaction = transaction;
            cmd.CommandText = "[dbo].[setDocState]";
            cmd.CommandType = CommandType.StoredProcedure;
            var p1 = cmd.CreateParameter();
            p1.ParameterName = "@docId";
            p1.Value = docId;
            var p2 = cmd.CreateParameter();
            p2.ParameterName = "@stateTypeId";
            p2.Value = stateTypeId;
            var p3 = cmd.CreateParameter();
            p3.ParameterName = "@userId";
            p3.Value = userId;
            cmd.Parameters.AddRange(new[] { p1, p2, p3 });
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// 0-userId
        /// </summary>
        const string fetchPositionByUserIdSqlTmpl = @"
select w.OrgPosition_Id, w.[User_Name] from Workers w
where w.Id = '{0}'
";
        /// <summary>
        /// 0-orgId
        /// </summary>
        const string fetchCodeByOrgIdSqlTmpl = @"
select o.Code from Organizations o
where o.Id = '{0}'
";
        const string fetchApplicationRegNoLastValueByOrgIdSqlTmpl = @"
select [Value] from Generators g
where g.Document_Def_Id = '04D25808-6DE9-42F5-8855-6F68A94A224C' and g.Organization_Id = '{0}'
";
    }
}
