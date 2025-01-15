using System;
using System.Collections.Generic;

namespace WPFAgenda.Models;

public partial class Contact
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public Contact()
    {
        
    }

    public Contact(string name, string email, string phone)
    {
        Name= name;
        Email= email;
        Phone= phone;
    }
}
