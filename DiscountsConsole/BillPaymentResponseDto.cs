namespace DiscountsConsole
{
    /// <summary>
    /// Data Transfer Object (DTO) for the response of a bill payment.
    /// </summary>
    public class BillPaymentResponseDto
    {
        /// <summary>
        /// Gets or sets the bill amount.
        /// </summary>
        public double BillAmount { get; set; }

        /// <summary>
        /// Gets or sets the total discount applied to the bill.
        /// </summary>
        public double TotalDiscount { get; set; }

        /// <summary>
        /// Gets or sets the net payable amount after applying discounts.
        /// </summary>
        public double NetPayableAmount { get; set; }
    }
}
