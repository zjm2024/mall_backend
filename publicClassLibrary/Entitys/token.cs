using SqlSugar;

namespace publicClassLibrary.Entitys
{
    [SugarTable("t_csc_token")]
    public class token
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int TokenId { get; set; }

        [SugarColumn(ColumnName = "Token")]
        public string Token { get; set; }

        [SugarColumn(ColumnName = "CompanyId")]
        public int CompanyId { get; set; }

        [SugarColumn(ColumnName = "DepartmentId")]
        public int DepartmentId { get; set; }

        [SugarColumn(ColumnName = "UserId")]
        public int UserId { get; set; }

        [SugarColumn(ColumnName = "CustomerId")]
        public int CustomerId { get; set; }

        [SugarColumn(ColumnName = "IsUser")]
        public bool IsUser { get; set; }

        [SugarColumn(ColumnName = "Timeout")]
        public DateTime Timeout { get; set; }
    }
}
