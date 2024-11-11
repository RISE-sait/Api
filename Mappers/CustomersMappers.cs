using Riok.Mapperly.Abstractions;
using Api.Model.People.Customers;
using Api.Model.People.Customers.Dto;
using Api.Model.People.Employees;
using Api.Model.People.Employees.Dto;

namespace Api.Mappers
{
    // [Mapper]
    public static class CustomerMapper
    {
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
