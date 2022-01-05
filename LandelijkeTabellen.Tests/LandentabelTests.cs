using System.Collections.Generic;
using Capella.LandelijkeTabellen.Landentabel;
using Xunit;

namespace Capella.LandelijkeTabellen.Tests
{
    public class LandentabelTests
    {
        [Fact]
        public void When_requesting_countries_Then_a_list_is_returned()
        {
            // Arrange
            var landenService = new LandenService();
            // Act
            List<Land>? landen = landenService.GetLanden();
            // Arrange
            Assert.NotEmpty(landen);
        }
    }
}