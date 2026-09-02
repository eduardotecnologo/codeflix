using FluentAssertions;
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
        // Padrão FluentAssertions
        category.Should().NotBeNull();
        category.Name.Should().Be(validData.Name);
        category.Description.Should().Be(validData.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));

        // Separado para rastreabilidade do test!
        category.CreatedAt.Should().BeAfter(dateTimeBefore);
        category.CreatedAt.Should().BeBefore(dateTimeAfter);

        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(ErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void ErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category(name!, "category description");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(ErrorWhenDescriptionIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    public void ErrorWhenDescriptionIsEmpty()   

    {
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category("category name", null!);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Description should not be null");
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
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    // Nome deve ter no máximo 255 caracteres
    [Fact(DisplayName = nameof(InstantiateErrorWhenNameisGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenNameisGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category(invalidName, "Category Ok Description");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name should be at most 255 characters long");
    }

    // Descrição deve ter no máximo 10000 caracteres
    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionisGreaterThan10000Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenDescriptionisGreaterThan10000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        Action action = () => new Codeflix.Catalogo.Domain.Entity.Category("Category Ok Name", invalidDescription);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Description should be at most 10000 characters long");
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

        category.IsActive.Should().BeTrue();
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

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]    public void Update()
    {
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category name", "category description");
        var newValues = new { Name = "new name", Description = "new description" };
        
        category.Update(newValues.Name, newValues.Description);
        
        category.Name.Should().Be("new name");
        category.Description.Should().Be("new description");
    }


    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category name", "category description");
        var newValues = new { Name = "new name"};
        var currentDescription = category.Description;

        category.UpdateOnlyName(newValues.Name);

        category.Name.Should().Be("new name");
        category.Description.Should().Be(currentDescription);
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
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name should not be empty or null");
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
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name should be at least 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameisGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateErrorWhenNameisGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category name", "category description");
        Action action = () => category.Update(invalidName, "category description");
        action.Should().Throw<ArgumentException>()
            .WithMessage("Name should be at most 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionisGreaterThan10000Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateErrorWhenDescriptionisGreaterThan10000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10001).Select(_ => "a").ToArray());
        var category = new Codeflix.Catalogo.Domain.Entity.Category("category new name", "category description");
        Action action = () => category.Update("category name", invalidDescription);
        action.Should().Throw<ArgumentException>()
            .WithMessage("Description should be at most 10000 characters long");
    }
}
