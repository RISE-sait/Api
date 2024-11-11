using Api.Model.People.Customers;
using Api.Model.People.Customers.Dto;

namespace Api.Mappers
{
    // [Mapper]

    /// <summary>
    /// Provides extension methods for mapping between Customer and DTO objects.
    /// </summary>
    public static class CustomerMapper
    {

        /// <summary>
        /// Maps a CreateCustomerDto object to a Customer object.
        /// </summary>
        /// <param name="createCustomerDto">The CreateCustomerDto object to map from.</param>
        /// <returns>A Customer object mapped from the CreateCustomerDto object.</returns>
        public static Customer MapToCustomer(this CreateCustomerDto createCustomerDto)
        {
            var customer = new Customer(
                   name: createCustomerDto.Name,
                   email: createCustomerDto.Email,
                   phoneNumber: createCustomerDto.PhoneNumber,
                   credit: createCustomerDto.Credit,
                   balance: createCustomerDto.Balance
               )
            {
                HasConsentMarketingEmails = createCustomerDto.HasConsentMarketingEmails,
                HasConsentMarketingSms = createCustomerDto.HasConsentMarketingSms,
                ShouldReceiveReceiptsForAllPayments = createCustomerDto.ShouldReceiveReceiptsForAllPayments
            };

            if (createCustomerDto.FamilyId.HasValue && createCustomerDto.Role.HasValue)
            {
                customer.FamilyId = createCustomerDto.FamilyId;
                customer.Role = createCustomerDto.Role;
            }

            return customer;
        }

        /// <summary>
        /// Maps a Customer object to a ReturnCustomerDto object.
        /// </summary>
        /// <param name="customer">The Customer object to map from.</param>
        /// <returns>A ReturnCustomerDto object mapped from the Customer object.</returns>
        public static ReturnCustomerDto MapToReturnCustomerDto(this Customer customer)
        {
            return new ReturnCustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                FamilyMembers = customer.Family?.Members != null ? customer.Family.Members.Select(m => m.Name).ToList() : null
            };
        }
    }
}
