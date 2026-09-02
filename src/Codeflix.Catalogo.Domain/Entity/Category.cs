using Codeflix.Catalogo.Domain.Exceptions;
using System;

namespace Codeflix.Catalogo.Domain.Entity;

public class Category : SeedWork.Entity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.Now;
        Validate();

    }
    
    public void Validate()
    {
        if(String.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException($"{nameof(Name)} should not be empty or null");
        }
        if(Description == null)
        {
            throw new ArgumentException($"{nameof(Description)} should not be null");
        }
        if(Name.Length < 3)
        {
            throw new ArgumentException($"{nameof(Name)} should be at least 3 characters long");
        }
        if(Name.Length > 255)
        {
            throw new ArgumentException($"{nameof(Name)} should be at most 255 characters long");
        }
        if(Description.Length > 10000)
        {
            throw new ArgumentException($"{nameof(Description)} should be at most 10000 characters long");
        }
        
    }
}
