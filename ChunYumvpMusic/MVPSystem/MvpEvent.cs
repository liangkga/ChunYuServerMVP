using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;

namespace ChunYuServer.MVPSystem
{
	// MVP 事件处理类喵~
	public class MvpEvent
	{
		// 等待玩家时调用喵~
		public void WaitingForPlayer()
		{
			Server.FriendlyFire = false;
			MvpEvent.PlayerKillCount.Clear();
		}

		// 玩家验证时调用喵~
		public void Verified(VerifiedEventArgs ev)
		{
			if (ev.Player != null)
			{
				// 初始化玩家击杀计数为 0 喵~
				MvpEvent.PlayerKillCount[ev.Player] = 0;
			}
		}

		// 玩家死亡时调用喵~
		public void Dying(DyingEventArgs ev)
		{
			if (ev.Player != null && ev.Attacker != null && MvpEvent.PlayerKillCount.ContainsKey(ev.Attacker))
			{
				if (ev.Player.IsHuman)
				{
					MvpEvent.PlayerKillCount[ev.Attacker]++;
				}
				if (ev.Player.IsScp)
				{
					MvpEvent.PlayerKillCount[ev.Attacker] += 5;
				}
			}
		}

		// 玩家击杀计数字典喵~
		public static Dictionary<Player, int> PlayerKillCount = new Dictionary<Player, int>();
	}
}
