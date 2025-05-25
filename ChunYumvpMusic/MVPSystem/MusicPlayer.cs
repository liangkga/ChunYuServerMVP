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
			AudioPlayer audioPlayer;

			// 尝试获取名为 "MVP" 的 AudioPlayer 实例喵~
			if (AudioPlayer.TryGet("MVP", out audioPlayer))
			{
				audioPlayer.RemoveAllClips();
			}
		}

		// 尝试播放音乐喵~
		public static bool TryPlayMusic(Player p)
		{
			bool flag = !Plugin.Instance.Config.MVPMusicPath.ContainsKey(p.UserId);
			bool result;
			if (flag)
			{
				Log.Info("1");
				result = false;
			}
			else
			{
				bool flag2 = p == null;
				if (flag2)
				{
					Log.Info("2");
					result = false;
				}
				else
				{
					AudioClipStorage.LoadClip(Plugin.Instance.Config.MVPMusicPath[p.UserId], "mvp");
					AudioPlayer audioPlayer = AudioPlayer.CreateOrGet("MVP", null, null, false, true, null, byte.MaxValue, delegate(AudioPlayer pp)
					{
						Speaker speaker = pp.AddSpeaker("Main", 1f, false, 5f, 5000f);
					}, null);
					audioPlayer.AddClip("mvp", 1f, false, true);
					Log.Info("MVP音乐已播放");
					result = true;
				}
			}
			return result;
		}

		// 回合结束时调用喵~
		public void RoundEnded(RoundEndedEventArgs ev)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool isEnableMVP = Plugin.Instance.Config.IsEnableMVP;
			if (isEnableMVP)
			{
				// 找到击杀数最多的玩家作为 MVP 喵~
				Player player = (from kv in MvpEvent.PlayerKillCount
				orderby kv.Value descending
				select kv).First<KeyValuePair<Player, int>>().Key;
				// 构建 MVP 信息字符串喵~
				stringBuilder.AppendLine(string.Concat(new string[]
				{
                    "本局<color=#FC0000>MVP </color>是",
					player.Nickname,
                    "本局共击杀 <color=#FF1493>",
					MvpEvent.PlayerKillCount[player].ToString(),
					"人！</color>"
				}));
				Log.Info(string.Concat(new string[]
				{
					"MVP是",
					player.Nickname,
					"本局共击杀",
					MvpEvent.PlayerKillCount[player].ToString(),
					"人"
				}));
				bool flag = player != null;
				if (flag)
				{
					// 延迟调用播放音乐喵~
					Timing.CallDelayed(0.5f, delegate()
					{
						bool flag2 = MusicPlayer.TryPlayMusic(player);
						Log.Info(string.Format("是否找到对应字典: {0}", flag2));
					});
				}
			}
			bool isEnableRoundEndedFF = Plugin.Instance.Config.IsEnableRoundEndedFF;
			if (isEnableRoundEndedFF)
			{
				Server.FriendlyFire = true;
				stringBuilder.AppendLine("友伤已开快尽情的背刺喵!~");
			}
			Map.ClearBroadcasts();
			// 清除所有玩家的广播喵~
			foreach (Player player2 in from x in Player.List
			where x != null
			select x)
			{
				player2.ClearBroadcasts();
			}
			// 广播 MVP 和友伤信息喵~
			Map.Broadcast(30, stringBuilder.ToString(), 0, false);
		}
	}
}
