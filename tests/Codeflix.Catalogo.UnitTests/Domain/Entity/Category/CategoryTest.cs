using System;
using System.ComponentModel;
using Xunit;
using DomainEntity = Codeflix.Catalogo.Domain.Entity;
namespace Codeflix.Catalogo.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]

    public void InstantiateWithIsActive(bool isActive)
    {
        // Arrange
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };

        var dateTimeBefore = DateTime.Now;

        // Act
        var category = new Codeflix.Catalogo.Domain.Entity.Category(validData.Name, validData.Description, isActive);
        
        var dateTimeAfter = DateTime.Now.AddSeconds(1);
        //Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);

        // Separado para rastreabilidade do test!
        Assert.True(category.CreatedAt > dateTimeBefore);
        Assert.True(category.CreatedAt < dateTimeAfter);

        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(ErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void ErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category(name!, "category description");
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(ErrorWhenDescriptionIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    public void ErrorWhenDescriptionIsEmpty()   

    {
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category("category name", null!);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    // Nome deve ter no mínimo 3 caracteres
    [Theory(DisplayName = nameof(InstantiateErrorWhenNameisLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("a")]
    [InlineData("ca")]
    public void InstantiateErrorWhenNameisLessThan3Characters(string invalidName)
    {
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category(invalidName, "Category Ok Description");
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    // Nome deve ter no máximo 255 caracteres
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameisGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenNameisGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category(invalidName, "Category Ok Description");
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Name should be at most 255 characters long", exception.Message);
    }

    // Descrição deve ter no máximo 10000 caracteres
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionisGreaterThan10000Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenDescriptionisGreaterThan10000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category("Category Ok Name", invalidDescription);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Description should be at most 10000 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Arrange
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };
        // Act
        var category = new Codeflix.Catalogo.Domain.Entity.Category(validData.Name, validData.Description, false);
        category.Activate();

        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(DeActivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void DeActivate()
    {
        // Arrange
        var validData = new
        {
            Name = "category name",
            Description = "category description"
        };
        // Act
        var category = new Codeflix.Catalogo.Domain.Entity.Category(validData.Name, validData.Description, true);
        category.DeActivate();

        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]    public void Update()
    {
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category name", "category description");
        var newValues = new { Name = "new name", Description = "new description" };
        
        category.Update(newValues.Name, newValues.Description);
        
        Assert.Equal("new name", category.Name);
        Assert.Equal("new description", category.Description);
    }


    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category name", "category description");
        var newValues = new { Name = "new name"};
        var currentDescription = category.Description;

        category.UpdateOnlyName(newValues.Name);

        Assert.Equal("new name", category.Name);
        Assert.Equal(currentDescription, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category name", "category description");
        Action action = () => category.UpdateOnlyName(name!);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameisLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("2")]
    [InlineData("a")]
    [InlineData("ca")]
    public void UpdateErrorWhenNameisLessThan3Characters(string invalidName)
    {
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category name", "category description");
        Action action = () => category.Update(invalidName, "category description");
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Name should be at least 3 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameisGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateErrorWhenNameisGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category name", "category description");
        Action action = () => category.Update(invalidName, "category description");
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Name should be at most 255 characters long", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionisGreaterThan10000Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateErrorWhenDescriptionisGreaterThan10000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category new name", "category description");
        Action action = () => category.Update("category name", invalidDescription);
        var exception = Assert.Throws<ArgumentException>(action);
        Assert.Equal("Description should be at most 10000 characters long", exception.Message);
    }
}
