namespace DiscountsConsole
{
    /// <summary>
    /// Data Transfer Object (DTO) for bill payment discounts
    /// </summary>
    public class BillPaymentRequestDto
    {
        /// <summary>
        /// Gets or sets the bill amount.
        /// </summary>
        public double BillAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has a gold card.
        /// </summary>
        public bool HasGoldCard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has a silver card.
        /// </summary>
        public bool HasSilverCard { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user is an affiliate.
        /// </summary>
        public bool IsAffiliate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has been a customer for over 2 years.
        /// </summary>
        public bool IsLongTermCustomer { get; set; }
    }
}
