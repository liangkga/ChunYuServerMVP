using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;
using UnityEngine;
// 移除RueI的using指令，改用Exiled的ShowHint方法喵~

namespace ChunYuServer.MVPSystem
{
	// 音乐播放器类喵~
	public class MusicPlayer
	{
		// 等待玩家时调用喵~
		public void WaitingForPlayer()
		{
			// 尝试获取名为 "MVP" 的 AudioPlayer 实例喵~
			if (AudioPlayer.TryGet("MVP", out AudioPlayer audioPlayer))
			{
				audioPlayer.RemoveAllClips();
			}
		}

		// 尝试播放音乐并返回播放的音乐文件名喵~
		public static string TryPlayMusic(Player p)
		{
			if (!Plugin.Instance.Config.MVPMusicPath.ContainsKey(p.UserId))
			{
				Log.Info("玩家没有配置音乐路径喵~");
				return null;
			}

			if (p == null)
			{
				Log.Info("玩家为空喵~");
				return null;
			}

			// 获取玩家的音乐路径列表喵~
			List<string> musicPaths = Plugin.Instance.Config.MVPMusicPath[p.UserId];

			if (!musicPaths.Any())
			{
				Log.Info($"玩家 {p.Nickname} 的音乐路径列表为空喵~");
				return null;
			}

			// 限制最多3个路径喵~
			if (musicPaths.Count > 3)
			{
				musicPaths = musicPaths.Take(3).ToList();
				Log.Warn($"玩家 {p.Nickname} 的音乐路径超过3个，已限制为前3个喵~");
			}

			// 随机选择一个音乐路径喵~
			System.Random random = new System.Random();
			string selectedPath = musicPaths[random.Next(musicPaths.Count)];

			// 生成唯一的剪辑名称避免缓存冲突喵~
			string clipName = $"mvp_{DateTime.Now.Ticks}";

			// 加载新的音频剪辑喵~
			AudioClipStorage.LoadClip(selectedPath, clipName);

			// 获取或创建音频播放器喵~
			AudioPlayer audioPlayer = AudioPlayer.CreateOrGet("MVP", null, null, false, true, null, byte.MaxValue, delegate (AudioPlayer pp)
			{
				Speaker speaker = pp.AddSpeaker("Main", 1f, false, 5f, 5000f);
			}, null);

			// 清除播放器中的所有剪辑喵~
			audioPlayer.RemoveAllClips();

			// 添加新的音频剪辑并播放喵~
			audioPlayer.AddClip(clipName, 1f, false, true);
			Log.Info($"MVP音乐已播放，选择的路径: {selectedPath}喵~");

			return System.IO.Path.GetFileNameWithoutExtension(selectedPath);
		}

		// 回合结束时调用喵~
		public void RoundEnded(RoundEndedEventArgs _)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Player player = null; // 将player声明提到这里喵~
			bool isEnableMVP = Plugin.Instance.Config.IsEnableMVP;
			if (isEnableMVP)
			{
				// 找到击杀数最多且在线的玩家作为 MVP 喵~
			if (MvpEvent.PlayerKillCount.Any()) // 确保有击杀记录再获取MVP喵~
			{
				// 首先尝试根据击杀数选择在线的MVP喵~
				var onlinePlayersWithKills = MvpEvent.PlayerKillCount
					.Where(kv => kv.Key != null && kv.Key.IsConnected && Player.List.Contains(kv.Key))
					.OrderByDescending(kv => kv.Value);
				
				if (onlinePlayersWithKills.Any())
				{
					player = onlinePlayersWithKills.First().Key;
				}
				else
				{
					// 如果没有在线的击杀玩家，根据伤害统计选择MVP喵~
					var onlinePlayersWithDamage = MvpEvent.PlayerDamageDealt
						.Where(kv => kv.Key != null && kv.Key.IsConnected && Player.List.Contains(kv.Key) && kv.Value > 0)
						.OrderByDescending(kv => kv.Value);
					
					if (onlinePlayersWithDamage.Any())
					{
						player = onlinePlayersWithDamage.First().Key;
						Log.Info($"根据伤害统计选择MVP: {player.Nickname}，造成伤害: {MvpEvent.PlayerDamageDealt[player]}喵~");
					}
				}
			}
				// string playedMusicName = "未知歌曲"; // 这行被删掉了喵~ 因为没用到

				if (player != null && (MvpEvent.PlayerKillCount.ContainsKey(player) || MvpEvent.PlayerDamageDealt.ContainsKey(player))) // 确保player不是null并且在击杀记录或伤害记录中喵~
				{
					// 构建 MVP 信息字符串（不含音乐名，音乐播放成功后再添加）喵~
				if (MvpEvent.PlayerKillCount.ContainsKey(player))
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"本局<color=#FC0000>MVP </color>是 ",
						player.Nickname,
						" 本局共击杀 <color=#FF1493>",
						MvpEvent.PlayerKillCount[player].ToString(),
						"人！</color>"
					}));
				}
				else if (MvpEvent.PlayerDamageDealt.ContainsKey(player))
				{
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"本局<color=#FC0000>MVP </color>是 ",
						player.Nickname,
						" 本局共造成伤害 <color=#FF1493>",
						MvpEvent.PlayerDamageDealt[player].ToString("F1"),
						"点！</color>"
					}));
				}
					if (MvpEvent.PlayerKillCount.ContainsKey(player))
				{
					Log.Info(string.Concat(new string[]
					{
						"MVP是",
						player.Nickname,
						"本局共击杀",
						MvpEvent.PlayerKillCount[player].ToString(),
						"人"
					}));
				}
				else if (MvpEvent.PlayerDamageDealt.ContainsKey(player))
				{
					Log.Info(string.Concat(new string[]
					{
						"MVP是",
						player.Nickname,
						"本局共造成伤害",
						MvpEvent.PlayerDamageDealt[player].ToString("F1"),
						"点"
					}));
				}

					// 延迟调用播放音乐喵~
					Timing.CallDelayed(0.5f, delegate()
					{
						string musicName = MusicPlayer.TryPlayMusic(player);
						if (!string.IsNullOrEmpty(musicName))
							{
								Log.Info($"MVP音乐播放成功: {musicName}喵~");
								// 使用RueI显示MVP信息喵~
								stringBuilder.AppendLine($"正在播放MVP音乐: <color=#00FFFF>{musicName}</color>");
								if (Plugin.Instance.Config.IsEnableRoundEndedFF)
								{
									Server.FriendlyFire = true;
									stringBuilder.AppendLine("友伤已开快尽情的背刺喵!~");
								}
								Map.Broadcast(30, $"<size=80%>{stringBuilder.ToString()}</size>", 0, false);
							}
						else
							{
								Log.Info("MVP音乐播放失败或未配置喵~");
								// 如果音乐播放失败，也需要显示基础的MVP信息和友伤信息喵~
								if (Plugin.Instance.Config.IsEnableRoundEndedFF)
								{
									Server.FriendlyFire = true;
									stringBuilder.AppendLine("友伤已开快尽情的背刺喵!~");
								}
								Map.Broadcast(30, $"<size=80%>{stringBuilder.ToString()}</size>", 0, false);
							}
					});
				}
				else // 如果没有有效的MVP玩家喵~
			{
				Log.Info("本局没有符合条件的MVP或MVP数据无效喵~");
				stringBuilder.AppendLine("本局<color=#FC0000>MVP</color>：虚位以待喵~");
				if (Plugin.Instance.Config.IsEnableRoundEndedFF)
				{
					Server.FriendlyFire = true;
					stringBuilder.AppendLine("友伤已开快尽情的背刺喵!~");
				}
				Map.Broadcast(30, $"<size=80%>{stringBuilder.ToString()}</size>", 0, false);
			}
			}
			else // 如果MVP功能未开启喵~
			{
				if (Plugin.Instance.Config.IsEnableRoundEndedFF)
				{
					Server.FriendlyFire = true;
					stringBuilder.AppendLine("友伤已开快尽情的背刺喵!~");
					Map.Broadcast(30, $"<size=80%>{stringBuilder.ToString()}</size>", 0, false);
				}
				// 如果MVP和友伤都没开，就不显示任何信息喵~
			}
		}
	}
}
