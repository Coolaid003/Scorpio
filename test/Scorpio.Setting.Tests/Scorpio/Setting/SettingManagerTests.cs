﻿using Microsoft.Extensions.DependencyInjection;

using Shouldly;

using Xunit;
namespace Scorpio.Setting
{
    public class SettingManagerTests : TestBase.IntegratedTest<SettingTestModule>
    {
        private readonly ISettingManager _settingManager;
        public SettingManagerTests() => _settingManager = ServiceProvider.GetService<ISettingManager>();

        [Fact]
        public void GetValue()
        {
            _settingManager.GetAsync<string>("Setting").Result.ShouldBe("SettingValue");
            _settingManager.GetAsync<int>("IntegerSetting").Result.ShouldBe(20);
        }

        [Fact]
        public void SetValue()
        {
            _settingManager.GetAsync<string>("Setting").Result.ShouldBe("SettingValue");
            _settingManager.SetAsync("Setting", "SettingValue2").Wait();
            _settingManager.GetAsync<string>("Setting").Result.ShouldBe("SettingValue2");
            _settingManager.GetAsync<string>("DefaultSetting").Result.ShouldBe(default);
            _settingManager.GetAsync<int>("IntegerSetting").Result.ShouldBe(20);
            _settingManager.SetAsync("IntegerSetting", 30).Wait();
            _settingManager.GetAsync<int>("IntegerSetting").Result.ShouldBe(30);
        }

    }
}
