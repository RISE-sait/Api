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
        /// <param name="createCustomerRequest">The CreateCustomerDto object to map from.</param>
        /// <returns>A Customer object mapped from the CreateCustomerDto object.</returns>
        public static Customer MapToCustomer(this CreateCustomerRequest request)
        {
            var customer = new Customer(
                id: request.Id,
                   name: request.Name,
                   email: request.Email,
                   phoneNumber: request.PhoneNumber,
                   credit: request.Credit,
                   balance: request.Balance
               )
            {
                HasConsentMarketingEmails = request.HasConsentMarketingEmails,
                HasConsentMarketingSms = request.HasConsentMarketingSms,
                ShouldReceiveReceiptsForAllPayments = request.ShouldReceiveReceiptsForAllPayments
            };

            if (request.FamilyId.HasValue && request.Role.HasValue)
            {
                customer.FamilyId = request.FamilyId;
                customer.Role = request.Role;
            }

            return customer;
        }

        /// <summary>
        /// Maps a Customer object to a ReturnCustomerDto object.
        /// </summary>
        /// <param name="customer">The Customer object to map from.</param>
        /// <returns>A ReturnCustomerDto object mapped from the Customer object.</returns>
        public static CustomerResponseDto MapToReturnCustomerDto(this Customer customer)
        {
            return new CustomerResponseDto
            {
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                FamilyMembers = [.. customer.Family.HasValue ? customer.Family.Value.Members : []]
            };
        }
    }
}
