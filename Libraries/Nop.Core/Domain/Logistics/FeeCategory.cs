namespace Nop.Core.Domain.Logistics
{
    /// <summary>
    /// 费用分类
    /// </summary>
    public class FeeCategory : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public FeeType Type { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}