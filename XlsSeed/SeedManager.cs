using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OfficeOpenXml;

namespace XlsSeed
{
    public class SeedManager
    {
        private static string IdentityInsertTemplate
            = "set identity_insert [{0}] {1};";

        private static string MatchTemplate = "T.Id = S.Id";
        
        private static string MergeTemplate  = 
@"merge into {0} as T
using(values ({1})) as S ({2})
on {4}
    WHEN MATCHED THEN  
UPDATE SET {3}
WHEN NOT MATCHED BY TARGET THEN  
INSERT ({2}) VALUES ({1});";


        private List<string> _excluded = new List<string>();
        
        private Dictionary<string, TabConfig> _config = new Dictionary<string, TabConfig>();

        private Dictionary<(string,string), Type> _enums = new Dictionary<(string,string), Type>();

        public SeedManager MapEnum<T>(string tabName, string propName)
        {
            _enums[(tabName, propName)] = typeof(T);
            return this;
        }

        public SeedManager SetMatch(string tabName, string matchSql)
        {
            InitConfig(tabName);
            _config[tabName].MatchSql = matchSql;
            return this;
        }
        
        public SeedManager SkipIdentityInsert(string tabName, params string[] keys)
        {
            InitConfig(tabName);

            _config[tabName].SkipIdentityInsert = true;
            _config[tabName].Keys = keys?.Length > 0 ? keys : null;
            return this;
        }

        private void InitConfig(string tabName)
        {
            if (!_config.ContainsKey(tabName))
            {
                _config[tabName] = new TabConfig();
            }
        }

        public string GetSql(string fileName)
        {
            var fi = new FileInfo(fileName);
            if (!fi.Exists)
            {
                throw new ArgumentException(nameof(fileName));    
            }
            
            using (var package = new ExcelPackage(fi))
            {
                var sb = new StringBuilder();
                foreach (var workSheet in package.Workbook.Worksheets.Where(x => !_excluded.Contains(x.Name)))
                {
                    sb.AppendLine(TabToSql(workSheet));
                }

                return sb.ToString();
            }
        }

        private TabConfig GetConfg(string tabName)
            => _config.ContainsKey(tabName)
                ? _config[tabName]
                : null;

        private string TabToSql(ExcelWorksheet workSheet)
        {
            var start = workSheet.Dimension.Start;
            var end = workSheet.Dimension.End;

            var propNames = new List<string>();
            
            for (var col = start.Column; col <= end.Column; col++)
            {
                if (!string.IsNullOrEmpty(workSheet.Cells[1, col].Text))
                {
                    propNames.Add(workSheet.Cells[1, col].Text);
                }
            }

            var propNameStr = propNames
                .Where(x => x != "_Comments_")
                .Select(x => $"[{x}]")
                .Aggregate((c, n) => $"{c}, {n}");

            var sb = new StringBuilder();
            var tableName = workSheet.Name;
            if (tableName == "CertifyingBoardsAcceptSpecialti")
            {
                tableName = "CertifyingBoardsAcceptSpecialties";
            }

            for (var row = start.Row + 1; row <= end.Row; row++)
            {
                var updateSql = "";
                var values = "";
                var flag = false;
                
                // check if line empty
                for (var col = start.Column; col <= end.Column; col++)
                {
                    if (workSheet.Cells[1, col].Text == "_Comments_")
                    {
                        continue;                        
                    }
                    
                    if (!string.IsNullOrEmpty(workSheet.Cells[row, col].Text))
                    {
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    continue;;
                }

                for (var col = start.Column; col <= propNames.Count; col++)
                {
                    var val = workSheet.Cells[row, col].Text;
                    var propName = propNames[col - 1];
                    
                    if (!string.IsNullOrEmpty(values))
                    {
                        values += ", ";
                    }
                    
                    if (!string.IsNullOrEmpty(updateSql) && propName != "Id")
                    {
                        updateSql += ", ";
                    }

                    var quotedVal = Prepare(val, tableName, propName);

                    if (propName != "Id")
                    {
                        updateSql += $"[{propName}] = {quotedVal}";
                    }
                    
                    values += quotedVal;
                }

                if (tableName == "TeamMembers")
                {
                    var keys = GetConfg(tableName)?.Keys;
                }

                var match = propNames.Any(x => x == "Id")
                    ? MatchTemplate                    
                    : (GetConfg(tableName)?.Keys ?? propNames.Where(x => x.EndsWith("Id")))
                        .Select(x => $"T.{x} = S.{x}")
                        .Aggregate((c, n) => $"{c} AND {n}");


                
                var sqlrow = string.Format(MergeTemplate, tableName, values, propNameStr, updateSql, match);
                
                sb.AppendLine(sqlrow);
            }

            var iion = string.Format(IdentityInsertTemplate, tableName, "ON");
            var iiof = string.Format(IdentityInsertTemplate, tableName, "OFF");
            var merge = sb.ToString();

            var skipIdentityInsert = _config.ContainsKey(tableName) && _config[tableName].SkipIdentityInsert;
            
            return skipIdentityInsert
                ? merge
                : $"{iion}\n{merge}\n{iiof}";
        }

        private string Prepare(string val, string tabName, string propName)
        {
            var i = 0;
            var b = false;
            var g = Guid.Empty;
            
            if (_enums.ContainsKey((tabName, propName)))
            {
                if (val == "NULL")
                {
                    return "NULL";
                }
                
                var e = Enum.Parse(_enums[(tabName, propName)], val);
                return Convert.ToInt32(e).ToString();
            }
            
            return int.TryParse(val, out i) || bool.TryParse(val, out b) || val == "NULL"
                ? val
                : $"'{val.Replace("\'", "\'\'")}'";
        }

        public SeedManager Exclude(string tableName)
        {
            _excluded.Add(tableName);
            return this;
        }
    }
}