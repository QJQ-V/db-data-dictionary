using PetaPoco;

namespace DBDataDictionary.Models.BO
{
    /// <summary>
    /// 数据字典
    /// </summary>
    public class DataDictionary
    {
        /// <summary>
        /// 表前缀
        /// </summary>
        [Column("table_schema")]
        public string TableSchema { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        [Column("table_name")]
        public string TableName { get; set; }

        /// <summary>
        /// 表描述
        /// </summary>
        [Column("table_description")]
        public string TableDescription { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        [Column("column_name")]
        public string ColumnName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        [Column("data_type")]
        public string DataType { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        [Column("is_nullable")]
        public string IsNullable { get; set; }

        /// <summary>
        /// 以字符为单位的最大长度
        /// </summary>
        [Column("character_maximum_length")]
        public string CharacterMaximumLength { get; set; }

        /// <summary>
        /// 字段描述
        /// </summary>
        [Column("column_description")]
        public string ColumnDescription { get; set; }
    }
}
