using System;
using System.Collections.Generic;
using System.Linq;
using Capella.LandelijkeTabellen.Landentabel;
using Xunit;

namespace Capella.LandelijkeTabellen.Tests;

public class LandentabelTests
{
    private readonly LandenService _underTest;

    public LandentabelTests()
    {
        _underTest = new LandenService();
    }

    [Fact]
    public void When_requesting_countries_Then_a_list_is_returned()
    {
        // Act
        List<Land> landen = _underTest.GetAllAsList();
        // Arrange
        Assert.NotEmpty(landen);
        Assert.Null(landen.SingleOrDefault(x => x.LandCode == "5100")); // Macedonië
    }

    [Fact]
    public void When_requesting_countries_with_historic_date_Then_a_list_is_returned()
    {
        // Act
        List<Land> landen = _underTest.GetAllAsList(new DateTime(2000, 1, 1));
        // Arrange
        Assert.NotNull(landen.SingleOrDefault(x => x.LandCode == "5100")); // Macedonië
    }

    [Fact]
    public void When_requesting_countries_with_history_Then_a_list_is_returned()
    {
        // Act
        List<Land> landen = _underTest.GetAllWithHistory(false);
        // Arrange
        Assert.Null(landen.SingleOrDefault(x => x.LandCode == "0000")); // Onbekend
        Assert.NotNull(landen.SingleOrDefault(x => x.LandCode == "5100")); // Macedonië
        Assert.NotNull(landen.SingleOrDefault(x => x.LandCode == "6030")); // Nederland
    }

    [Fact]
    public void Given_an_existing_country_When_requested_by_code_Then_the_country_is_returned()
    {
        // Act
        Land land = _underTest.GetByCode("6030");
        // Assert
        Assert.Equal("Nederland", land.Omschrijving);
    }

    [Fact]
    public void When_requested_the_unknown_country_by_code_Then_the_country_is_returned()
    {
        // Act
        Land land = _underTest.GetByCode(0);
        // Assert
        Assert.Equal("Onbekend", land.Omschrijving);
    }

    [Fact]
    public void Given_an_existing_country_When_requested_by_numeric_code_Then_the_country_is_returned()
    {
        // Act
        Land land = _underTest.GetByCode(6030);
        // Assert
        Assert.Equal("Nederland", land.Omschrijving);
    }

    [Fact]
    public void When_requesting_a_key_value_list_Then_the_countries_by_code_are_returned()
    {
        // Act
        IDictionary<string, string> landDic = _underTest.GetAllAsDictionary(new DateTime(2022, 1, 1));
        // Assert
        Assert.Contains("6030", landDic); // Nederland
        Assert.Contains("0000", landDic); // Onbekend
        Assert.DoesNotContain("5100", landDic); // Macedonië
    }

    [Fact]
    public void When_requesting_a_key_value_list_without_unknown_value_Then_the_countries_by_code_are_returned()
    {
        // Act
        IDictionary<string, string> landDic = _underTest.GetAllAsDictionary(new DateTime(2022, 1, 1), false);
        // Assert
        Assert.Contains("6030", landDic); // Nederland
        Assert.DoesNotContain("0000", landDic); // Onbekend
        Assert.DoesNotContain("5100", landDic); // Macedonië
    }
}