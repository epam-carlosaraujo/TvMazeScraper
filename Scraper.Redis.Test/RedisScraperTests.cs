using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Scraper.Redis.Connections;
using Scraper.Types.Abstractions;
using Scraper.Types.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Scraper.Redis.Test
{
    [TestClass]
    public class RedisScraperTests
    {
        private Mock<IRedisConnection> _redisMock;
        private Mock<ITvMazeAPIConnection> _apiMock;

        private RedisScraper GetScraper(int count, int length)
        {
            _apiMock = new Mock<ITvMazeAPIConnection>();
            _apiMock.SetupGet(x => x.ShowsPageLimit).Returns(1);
            if (count <= 0)
            {
                _apiMock.Setup(x => x.GetShowsAsync(It.IsAny<int>())).Throws(new EndOfShowsException());
            }

            _redisMock = new Mock<IRedisConnection>();
            _redisMock.Setup(x => x.SaveToRedisAsync(It.IsAny<List<Types.TvMazeAPI.TvMazeAPITypes.Show>>())).Returns(Task.FromResult(length));
            return new RedisScraper(_redisMock.Object, _apiMock.Object);
        }
        
        [TestMethod]
        public async Task Should_Throw_When_OffsetLessThanZeroAsync()
        {
            const int count = 0;
            var scraper = GetScraper(count, 10);
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => scraper.ScrapAsync(-1, count));
        }

        [TestMethod]
        public async Task Should_ReturnAll_When_CountLessThanZeroAsync()
        {
            const int count = -1;
            const int length = 10;
            var scraper = GetScraper(count, length);
            var result = await scraper.ScrapAsync(0, count);
            Assert.AreEqual(length, result.NumberOfRecordsScraped);
        }

        [TestMethod]
        public async Task Should_Return_When_CountGreaterThanZeroAsync()
        {
            const int count = 5;
            const int length = 10;
            var scraper = GetScraper(count, length);
            var result = await scraper.ScrapAsync(0, count);
            Assert.AreEqual(length, result.NumberOfRecordsScraped);
        }
    }
}
