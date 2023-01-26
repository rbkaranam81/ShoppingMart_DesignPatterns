namespace ShoppingMart.Common.Models;
public class Customer
{
    public Customer(string fullName, string email, string phoneNumber, Address mailingAddress)
    {
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        MailingAddress = mailingAddress;
    }

    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public Address MailingAddress { get; set; }
    
}
