namespace Nop.Core.Domain.Logistics
{
    /// <summary>
    /// 物流参与人
    /// </summary>
    public partial class ConsignmentUser : BaseEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
    }
}
