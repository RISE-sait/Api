// using Api.Database;
// using Api.Mappers;
// using Api.Model.People.Customers;
// using Api.Model.People.Customers.Dto;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace Api.Controllers;

// [ApiController]
// [Route("api/[controller]")]

// public class CustomerController(AppDbContext context) : ControllerBase
// {
//     /// <summary>
//     /// Retrieves a list of customers with optional limit parameter
//     /// </summary>
//     /// <param name="limit">Number of customers to return (valid values: 5, 10, or 20)</param>
//     /// <returns>A list of customers with their family information</returns>
//     /// <response code="200">Returns the list of customers</response>
//     /// <response code="400">If the limit parameter is invalid</response>
//     [HttpGet]
//     [ProducesResponseType(typeof(IEnumerable<CustomerResponseDto>), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public ActionResult<IEnumerable<CustomerResponseDto>> Get([FromQuery] int limit = 5)
//     {
//         // Validate the limit parameter
//         if (limit != 5 && limit != 10 && limit != 20)
//         {
//             return BadRequest(new { Message = "Limit must be 5, 10, or 20" });
//         }

//         // Retrieve customers with their family information
//         var customers = context.Customers
//            .Include(c => c.Family)  // Include family data
//             .Take(limit)            // Limit the number of results
//             .Select(c => c.MapToReturnCustomerDto())
//             .ToList();

//         return Ok(customers);
//     }

//     /// <summary>
//     /// Retrieves a specific customer by their email address
//     /// </summary>
//     /// <param name="email">The email address of the customer to retrieve</param>
//     /// <returns>The customer information</returns>
//     /// <response code="200">Returns the requested customer</response>
//     /// <response code="404">If the customer is not found</response>
//     [HttpGet("by-email/{email}")]
//     [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<CustomerResponseDto>> GetByEmail(string email)
//     {
//         // Find customer by email
//         var customer = await context.Customers
//             .FirstOrDefaultAsync(c => c.Email == email);

//         // Handle not found case
//         if (customer == null)
//         {
//             return NotFound(new { Message = $"Customer with email {email} not found" });
//         }

//         // Map and return the customer data
//         return Ok(customer.MapToReturnCustomerDto());
//     }

//     /// <summary>
//     /// Creates a new customer
//     /// </summary>
//     /// <param name="createCustomerDto">The customer data to create</param>
//     /// <returns>The created customer information</returns>
//     /// <response code="201">Returns the newly created customer</response>
//     /// <response code="400">If the model state is invalid</response>
//     [HttpPost]
//     [ProducesResponseType(typeof(Customer), StatusCodes.Status201Created)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     public ActionResult<Customer> Post([FromBody] CreateCustomerRequest createCustomerRequest)
//     {
//         // Validate the model state
//         if (!ModelState.IsValid)
//         {
//             return BadRequest(ModelState);
//         }

//         // Map DTO to Customer entity
//         var customer = createCustomerRequest.MapToCustomer();

//         // Add customer to database
//         context.Customers.Add(customer);
//         context.SaveChanges();

//         // Return 201 Created response with the created customer
//         return CreatedAtAction(nameof(Post), new { id = customer.Id }, customer);
//     }
//     /// <summary>
//     /// Updates specific properties of a customer identified by their email address
//     /// </summary>
//     /// <param name="email">The email address of the customer to update</param>
//     /// <param name="patchCustomerDto">The properties to update</param>
//     /// <returns>The updated customer information</returns>
//     /// <response code="200">Returns the updated customer</response>
//     /// <response code="400">If the model state is invalid</response>
//     /// <response code="404">If the customer is not found</response>
//     [HttpPut]
//     [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
//     [ProducesResponseType(StatusCodes.Status400BadRequest)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     public async Task<ActionResult<Customer>> Put(
//         [FromRoute] string email,
//         [FromBody] UpdateCustomerRequest updateCustomerRequest)
//     {
//         if (!ModelState.IsValid)
//         {
//             return BadRequest(ModelState);
//         }

//         // Find customer and handle not found case
//         var customer = await context.Customers
//             .FirstOrDefaultAsync(c => c.Email == email);

//         if (customer == null)
//         {
//             return NotFound(new { Message = $"Customer with email {email} not found" });
//         }

//         try
//         {
//             // Update customer properties
//             UpdateCustomerProperties(customer, updateCustomerRequest);
//             context.Customers.Update(customer);
//             await context.SaveChangesAsync();

//             return Ok(customer);
//         }
//         catch (Exception ex)
//         {
//             // Log the exception here if needed
//             return StatusCode(StatusCodes.Status500InternalServerError,
//                 new { Message = $"An error occurred while updating the customer. {ex.Message}" });
//         }
//     }

//     /// <summary>
//     /// Helper method to update customer properties from the patch DTO
//     /// </summary>
//     private static void UpdateCustomerProperties(Customer customer, UpdateCustomerRequest updateCustomerRequest)
//     {
//         var patchProperties = typeof(UpdateCustomerRequest).GetProperties();
//         var customerType = typeof(Customer);

//         foreach (var sourceProp in patchProperties)
//         {
//             var value = sourceProp.GetValue(updateCustomerRequest);
//             if (value == null) continue;

//             var customerProp = customerType.GetProperty(sourceProp.Name);
//             if (customerProp == null || !customerProp.CanWrite) continue;

//             if (Nullable.GetUnderlyingType(sourceProp.PropertyType) != null)
//             {
//                 // Handle nullable value types
//                 customerProp.SetValue(customer, ((dynamic)value).Value);
//             }
//             else
//             {
//                 customerProp.SetValue(customer, value);
//             }
//         }
//     }

//     /// <summary>
//     /// Deletes a customer by their ID
//     /// </summary>
//     /// <param name="id">The unique identifier of the customer to delete</param>
//     /// <returns>No content if successful</returns>
//     /// <response code="204">Returns no content if deletion is successful</response>
//     /// <response code="404">If the customer is not found</response>
//     [ProducesResponseType(StatusCodes.Status204NoContent)]
//     [ProducesResponseType(StatusCodes.Status404NotFound)]
//     [HttpDelete("{id}")]
//     public ActionResult Delete(Guid id)
//     {
//         var customer = context.Customers.Find(id);

//         if (customer == null)
//         {
//             return NotFound(new { Message = "Customer not found" });
//         }

//         context.Customers.Remove(customer);
//         context.SaveChanges();

//         return NoContent();
//     }
// }

