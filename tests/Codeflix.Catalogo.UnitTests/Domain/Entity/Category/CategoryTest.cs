using System;
using Xunit;
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
}
