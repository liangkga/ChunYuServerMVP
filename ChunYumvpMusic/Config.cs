using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;
using YamlDotNet.Serialization;

namespace ChunYuServer
{
	// 插件配置类喵~
	public class Config : IConfig
	{

		[Description("是否启用MVP插件喵~")] // 是否启用插件的描述喵~
		public bool IsEnabled { get; set; } = true; // 插件是否启用喵~

		public bool Debug { get; set; } = false; // 是否开启调试模式喵~

		public bool IsEnableMVP { get; set; } = true; // 是否启用 MVP 功能喵~

		[Description("对局结束友伤喵~")] // 对局结束友伤的描述喵~
		public bool IsEnableRoundEndedFF { get; set; } = true; // 对局结束是否启用友伤喵~

		[Description("MVP配置 - 每个用户最多3个音乐路径喵~")] // MVP 配置的描述喵~
		[YamlMember(Alias = "mvp_music_path")]
		public Dictionary<string, List<string>> MVPMusicPath { get; set; } = new Dictionary<string, List<string>> // MVP 音乐路径配置喵~
		{
			{
				"ChunYu.wiki@steam", // 玩家 Steam ID 喵~
                new List<string> { "音乐路径1", "音乐路径2", "音乐路径3" } // 对应的音乐文件路径列表(最多3个)喵~
            },
        };
	}
}
