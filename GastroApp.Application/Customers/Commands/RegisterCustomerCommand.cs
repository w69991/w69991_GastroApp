using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GastroApp.Domain.Entities; 
using GastroApp.Domain.Interfaces; 

namespace GastroApp.Application.Customers.Commands
{

    public record RegisterCustomerCommand(string FirstName, string LastName, string Email, string Password) : IRequest<Guid>;

    public class RegisterCustomerHandler : IRequestHandler<RegisterCustomerCommand, Guid>
    {
        private readonly IAsyncRepository<Customer> _customerRepository;

        public RegisterCustomerHandler(IAsyncRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Guid> Handle(RegisterCustomerCommand request, CancellationToken _)
        {
            //Sprawdzanie duplikatu e-maila
            var exists = (await _customerRepository.ListAllAsync())
                .Any(c => c.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase));

            if (exists)
                throw new EmailAlreadyTakenException(request.Email);

            //Tworzenie klienta po zarejestrowaniu
            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName  = request.LastName,
                Email     = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            await _customerRepository.AddAsync(customer);
            return customer.Id;
        }
        //Klasa wyswietlania bledu duplikatu e-maila
        public class EmailAlreadyTakenException : Exception
        {
            public EmailAlreadyTakenException(string email)
                : base($"E-mail '{email}' is already in use."){ }
        }
    }
}