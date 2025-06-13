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
			MvpEvent.PlayerDamageDealt.Clear();
		}

		// 玩家验证时调用喵~
		public void Verified(VerifiedEventArgs ev)
		{
			bool flag = ev.Player != null;
			if (flag)
			{
				// 延迟初始化玩家击杀计数为 1 和伤害为 0 喵~
				Timing.CallDelayed(0.5f, delegate()
				{
					MvpEvent.PlayerKillCount[ev.Player] = 1;
					MvpEvent.PlayerDamageDealt[ev.Player] = 0f;
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
					// 击杀人类单位（包括D级人员、科学家、设施警卫、九尾狐等）获得1分喵~
				bool isHumanTarget = ev.Player.IsHuman;
				if (isHumanTarget)
				{
					Dictionary<Player, int> playerKillCount = MvpEvent.PlayerKillCount;
					Player attacker = ev.Attacker;
					int num = playerKillCount[attacker];
					playerKillCount[attacker] = num + 1;
				}
				// 击杀SCP异常实体获得5分喵~
				bool isScpTarget = ev.Player.IsScp;
				if (isScpTarget)
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
		
		// 玩家伤害统计字典喵~
		public static Dictionary<Player, float> PlayerDamageDealt = new Dictionary<Player, float>();
		
		// 玩家受伤时调用喵~
		public void Hurting(Exiled.Events.EventArgs.Player.HurtingEventArgs ev)
		{
			bool flag = ev.Player != null && ev.Attacker != null && ev.Amount > 0;
			if (flag)
			{
				bool flag2 = MvpEvent.PlayerDamageDealt.ContainsKey(ev.Attacker);
				if (flag2)
				{
					MvpEvent.PlayerDamageDealt[ev.Attacker] += ev.Amount;
				}
			}
		}
	}
}
