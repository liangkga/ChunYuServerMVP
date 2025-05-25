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
			bool flag = ev.Player != null;
			if (flag)
			{
				// 延迟初始化玩家击杀计数为 1 喵~
				Timing.CallDelayed(0.5f, delegate()
				{
					MvpEvent.PlayerKillCount[ev.Player] = 1;
				});
			}
		}

		// 玩家死亡时调用喵~
		public void Dying(DyingEventArgs ev)
		{
			bool flag = ev.Player != null && ev.Attacker != null;
			if (flag)
			{
				bool flag2 = MvpEvent.PlayerKillCount.ContainsKey(ev.Attacker);
				if (flag2)
				{
					bool isHuman = ev.Player.IsHuman;
					if (isHuman)
					{
						Dictionary<Player, int> playerKillCount = MvpEvent.PlayerKillCount;
						Player attacker = ev.Attacker;
						int num = playerKillCount[attacker];
						playerKillCount[attacker] = num + 1;
					}
					bool isScp = ev.Player.IsScp;
					if (isScp)
					{
						Dictionary<Player, int> playerKillCount2 = MvpEvent.PlayerKillCount;
						Player attacker = ev.Attacker;
						playerKillCount2[attacker] += 5;
					}
				}
			}
		}

		// 玩家击杀计数字典喵~
		public static Dictionary<Player, int> PlayerKillCount = new Dictionary<Player, int>();
	}
}
