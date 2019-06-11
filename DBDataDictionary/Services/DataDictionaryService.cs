using DBDataDictionary.Models.BO;
using DBDataDictionary.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBDataDictionary.Services
{
    public class DataDictionaryService
    {
        private List<DataDictionary> GetDataDictionaries()
        {
            PetaPoco.Database db = new PetaPoco.Database("conn");

            string sql = @"SELECT
	                        a.TABLE_SCHEMA AS table_schema,
	                        a.TABLE_NAME AS table_name,
	                        c.value AS table_description,
	                        a.COLUMN_NAME AS column_name,
	                        a.DATA_TYPE AS data_type,
	                        a.IS_NULLABLE AS is_nullable,
	                        a.CHARACTER_MAXIMUM_LENGTH AS character_maximum_length,
	                        b.value AS column_description 
                        FROM
	                        information_schema.COLUMNS AS a
	                        LEFT JOIN sys.extended_properties AS b ON a.TABLE_NAME= OBJECT_NAME( b.major_id ) 
	                        AND a.ORDINAL_POSITION= b.minor_id
	                        LEFT JOIN sys.extended_properties AS c ON a.TABLE_NAME= OBJECT_NAME( c.major_id ) 
	                        AND c.minor_id = 0";

            List<DataDictionary> dataDictionaries = db.Fetch<DataDictionary>(sql);

            return dataDictionaries;
        }

        /// <summary>
        /// 创建Bootstrap版
        /// </summary>
        public void CreateHtml()
        {
            List<DataDictionary> dataDictionaries = GetDataDictionaries().OrderBy(r => r.TableSchema).ThenBy(r => r.TableName).ToList();

            StringBuilder tabHtml = new StringBuilder();
            StringBuilder tabConentHtml = new StringBuilder();

            var tabs = dataDictionaries.GroupBy(r => new { r.TableSchema }).ToList();

            foreach (var tabItem in tabs)
            {
                int tabIndex = tabs.IndexOf(tabItem);

                tabHtml.AppendFormat("<li class='{0}'><a href=#tab{1} data-toggle='tab'>{2}（{3}）</a></li>", tabIndex ==0 ? "active":"" ,tabIndex, tabItem.Key.TableSchema, TableSchema.Dic.Where(r=>r.Key == tabItem.Key.TableSchema).FirstOrDefault().Value);
                tabConentHtml.AppendFormat("<div class='tab-pane fade {0}' id='tab{1}'>",tabIndex == 0 ? " in active" : "", tabIndex);

                var tables = dataDictionaries.FindAll(r => r.TableSchema == tabItem.Key.TableSchema).GroupBy(r => new {r.TableName, r.TableDescription }).ToList();

                foreach (var tableItem in tables)
                {
                    int tableIndex = tables.IndexOf(tableItem);

                    tabConentHtml.Append("<table class='table table-bordered table-striped'>");
                    tabConentHtml.AppendFormat("<caption class='table-caption'>{0}.{1}：{2}</caption>", tabItem.Key.TableSchema, tableItem.Key.TableName, tableItem.Key.TableDescription);

                    tabConentHtml.Append("<tr>");
                    tabConentHtml.Append("<th width='300'>列名</th>");
                    tabConentHtml.Append("<th width='150'>类型</th>");
                    tabConentHtml.Append("<th width='150'>是否可空</th>");
                    tabConentHtml.Append("<th width='180'>以字符为单位的最大长度</th>");
                    tabConentHtml.Append("<th>字段描述</th>");
                    tabConentHtml.Append("</tr>");

                    foreach (DataDictionary columnItem in dataDictionaries.FindAll(r => r.TableName == tableItem.Key.TableName))
                    {
                        tabConentHtml.Append("<tr>");
                        tabConentHtml.AppendFormat("<td>{0}</td>", columnItem.ColumnName);
                        tabConentHtml.AppendFormat("<td>{0}</td>", columnItem.DataType);
                        tabConentHtml.AppendFormat("<td>{0}</td>", columnItem.IsNullable);
                        tabConentHtml.AppendFormat("<td>{0}</td>", columnItem.CharacterMaximumLength);
                        tabConentHtml.AppendFormat("<td>{0}</td>", columnItem.ColumnDescription);
                        tabConentHtml.Append("</tr>");
                    }

                    tabConentHtml.Append("</table>");
                }

                tabConentHtml.Append("</div>");
            }

            string htmltext = HtmlUtils.ReadHtml();
            htmltext = htmltext.Replace("$tabHtml", tabHtml.ToString());
            htmltext = htmltext.Replace("$tabConentHtml", tabConentHtml.ToString());

            HtmlUtils.WriteHtml(htmltext);
        }

        /// <summary>
        /// 创建ElementUi版（页面数据量多导致卡顿）
        /// </summary>
        public void CreateElementHtml()
        {
            List<DataDictionary> dataDictionaries = GetDataDictionaries().OrderBy(r => r.TableSchema).ThenBy(r => r.TableName).ToList();

            StringBuilder htmlContent = new StringBuilder();
            StringBuilder data = new StringBuilder();

            var tabs = dataDictionaries.GroupBy(r => new { r.TableSchema }).ToList();

            htmlContent.Append("<el-tabs v-model='activeName' style='width: 1140px;'>");
            data.Append("{");

            foreach (var tabItem in tabs)
            {
                int tabIndex = tabs.IndexOf(tabItem);
                if (tabIndex == 0)
                {
                    data.AppendFormat("activeName: 'tab{0}',", tabIndex);
                }
                var tables = dataDictionaries.FindAll(r => r.TableSchema == tabItem.Key.TableSchema).GroupBy(r => new { r.TableName, r.TableDescription }).ToList();

                htmlContent.AppendFormat("<el-tab-pane label='{0}（{1}）' name='tab{2}'>", tabItem.Key.TableSchema, "定价", tabIndex);

                foreach (var tableItem in tables)
                {
                    int tableIndex = tables.IndexOf(tableItem);
                    htmlContent.Append("<div class='tableDiv'>");
                    htmlContent.AppendFormat("<el-alert title='{0}.{1}：{2}' type='info' :closable='false'></el-alert>", tabItem.Key.TableSchema, tableItem.Key.TableName, tableItem.Key.TableDescription);
                    htmlContent.AppendFormat("<el-table :data='tableData{0}{1}' border stripe  style='width:100%' >", tabIndex, tableIndex);
                    htmlContent.Append("<el-table-column prop='column_name' label='列名' width='300'></el-table-column>");
                    htmlContent.Append("<el-table-column prop='data_type' label='类型' width='180'></el-table-column>");
                    htmlContent.Append("<el-table-column prop='is_nullable' label='是否可空' width='180'></el-table-column>");
                    htmlContent.Append("<el-table-column prop='character_maximum_length' label='以字符为单位的最大长度' width='180'></el-table-column>");
                    htmlContent.Append("<el-table-column prop='column_description' label='字段描述' width='298'></el-table-column>");
                    htmlContent.Append("</el-table>");
                    htmlContent.Append("</div>");

                    data.AppendFormat("tableData{0}{1}:[", tabIndex, tableIndex);

                    foreach (DataDictionary columnItem in dataDictionaries.FindAll(r => r.TableName == tableItem.Key.TableName))
                    {
                        data.Append("{");
                        data.AppendFormat("column_name:'{0}',", columnItem.ColumnName);
                        data.AppendFormat("data_type:'{0}',", columnItem.DataType);
                        data.AppendFormat("is_nullable:'{0}',", columnItem.IsNullable);
                        data.AppendFormat("character_maximum_length:'{0}',", columnItem.CharacterMaximumLength);
                        data.AppendFormat("column_description:'{0}',", columnItem.ColumnDescription);
                        data.Append("},");
                    }

                    data.Append("],");
                }

                htmlContent.Append("</el-tab-pane>");
            }

            htmlContent.Append("</el-tabs>");
            data.Append("}");

            string htmltext = HtmlUtils.ReadHtml();
            htmltext = htmltext.Replace("$htmlFormat", htmlContent.ToString());
            htmltext = htmltext.Replace("$dataFormat", data.ToString());

            HtmlUtils.WriteHtml(htmltext);
        }
    }
}
