using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using MEC;

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
			// AudioClipStorage 中的片段管理似乎主要依赖 LoadClip 的覆盖行为喵~
			// 在这里，我们只确保 AudioPlayer 实例是干净的喵~
		}

		// 尝试播放音乐喵~
		public static bool TryPlayMusic(Player p)
		{
			if (p == null)
			{
				Log.Debug($"Player is null, cannot play MVP music喵~");
				return false;
			}

			if (!Plugin.Instance.Config.MVPMusicPath.TryGetValue(p.UserId, out string musicPath) || string.IsNullOrEmpty(musicPath))
			{
				Log.Debug($"No MVP music path configured for player {p.Nickname} ({p.UserId})喵~");
				return false;
			}

			if (!AudioPlayer.TryGet("MVP", out AudioPlayer audioPlayer))
			{
				Log.Debug("AudioPlayer 'MVP' not found, creating a new one喵~");
				audioPlayer = AudioPlayer.CreateOrGet(
					"MVP", 
					null, // owner: Player (can be null)
					null, // parent: GameObject (can be null)
					false, // removeOnStop: bool
					true, // persistent: bool
					null, // spatialBlend: float? (can be null for 2D)
					byte.MaxValue, // defaultVolume: byte
					ap => ap.AddSpeaker("Main", 1f, false, 5f, 5000f) // onCreated: Action<AudioPlayer>
				);
			}
			else
			{
				Log.Debug("Found existing AudioPlayer 'MVP', clearing its clips喵~");
				audioPlayer.RemoveAllClips(); 
			}

			string clipName = "mvp_clip"; // Using a consistent clip name as we clear before adding
			AudioClipStorage.LoadClip(musicPath, clipName); // LoadClip 会自动处理已存在同名片段的情况喵~ 
			audioPlayer.AddClip(clipName, 1f, false, true); // autoPlay is true

			Log.Info($"Playing MVP music '{System.IO.Path.GetFileNameWithoutExtension(musicPath)}' for player {p.Nickname} ({p.UserId})喵~");
			return true;
		}

		// 回合结束时调用喵~
		public void RoundEnded(RoundEndedEventArgs _)
		{
			StringBuilder mvpHintBuilder = new StringBuilder();
			bool isEnableMVP = Plugin.Instance.Config.IsEnableMVP;
			Player mvpPlayer = null;

			if (isEnableMVP)
			{
				if (MvpEvent.PlayerKillCount != null && MvpEvent.PlayerKillCount.Any())
				{
					var mvpEntry = MvpEvent.PlayerKillCount
						.OrderByDescending(kv => kv.Value)
						.FirstOrDefault();

					if (mvpEntry.Key != null)
					{
						mvpPlayer = mvpEntry.Key;
					}
					else
					{
						Log.Warn("MVP selection: PlayerKillCount was not empty but no valid MVP player found.喵~");
					}
				}
				else
				{
					Log.Info("MVP selection: PlayerKillCount is null or empty. No MVP will be announced.喵~");
				}

				if (mvpPlayer != null)
				{
					string musicName = "未知音乐喵~";
					if (Plugin.Instance.Config.MVPMusicPath.TryGetValue(mvpPlayer.UserId, out string musicPath) && !string.IsNullOrEmpty(musicPath))
					{
						try
						{
							musicName = System.IO.Path.GetFileNameWithoutExtension(musicPath);
						}
						catch (ArgumentException ex)
						{
							Log.Error($"Error getting music name from path '{musicPath}': {ex.Message}喵~");
						}
					}
					mvpHintBuilder.AppendLine($"本局<color=#FC0000>MVP </color>是{mvpPlayer.Nickname}本局共击杀 <color=#FF1493>{MvpEvent.PlayerKillCount[mvpPlayer]}人！</color>\n正在播放MVP音乐: {musicName}喵~");
					Log.Info($"MVP是{mvpPlayer.Nickname}本局共击杀{MvpEvent.PlayerKillCount[mvpPlayer]}人喵~");
					Timing.CallDelayed(0.5f, () => TryPlayMusic(mvpPlayer));
				}
			} 

			StringBuilder broadcastBuilder = new StringBuilder();
			// 如果启用了MVP功能且有MVP提示，则添加到广播中喵~
			if (isEnableMVP && mvpHintBuilder.Length > 0)
			{
				broadcastBuilder.Append(mvpHintBuilder.ToString());
			}

			bool isEnableRoundEndedFF = Plugin.Instance.Config.IsEnableRoundEndedFF;
			// 无论是否有MVP提示，如果友伤提示开启，都添加到广播中喵~
			// 确保在添加友伤提示前有换行符，如果 broadcastBuilder 不为空且末尾不是换行符喵~
			if (isEnableRoundEndedFF)
			{
				Server.FriendlyFire = true;
				if (broadcastBuilder.Length > 0 && broadcastBuilder[broadcastBuilder.Length - 1] != '\n')
				{
					broadcastBuilder.AppendLine();
				}
				broadcastBuilder.AppendLine("友伤已开快尽情的背刺喵!~");
			}



			// 清理之前的广播喵~
			Map.ClearBroadcasts();
			foreach (Player player2 in Player.List.Where(x => x != null))
			{
				player2.ClearBroadcasts();
			}

			// 如果 broadcastBuilder 中有内容，就进行广播喵~
			if (broadcastBuilder.Length > 0)
			{
				Map.Broadcast(30, broadcastBuilder.ToString().TrimEnd('\r', '\n'), (ushort)0, false); // 使用 TrimEnd 清理末尾可能多余的换行符喵~
			}
		}
	}
}
