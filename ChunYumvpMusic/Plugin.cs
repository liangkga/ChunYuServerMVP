using System;
using System.Net.Http;
using ChunYuServer.MVPSystem;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Features;
using Exiled.Events.Handlers;

namespace ChunYuServer
{
	// 插件主类喵~
	public class Plugin : Plugin<Config>
	{
		public override string Author { get; } = "ChunYu椿雨";

		public override string Name { get; } = "MVP";

		public static HttpClient HttpClient { get; private set; }

		// 插件启用时调用喵~
		public override void OnEnabled()
		{
			base.OnEnabled();
			Log.Info("MVP插件开启ChunYu椿雨");
			Plugin.Instance = this;
			Plugin.Singleton = this;
			this.musicPlayer = new MusicPlayer();
			this.mvpEvent = new MvpEvent();
			// 订阅事件喵~
			Exiled.Events.Handlers.Player.Verified += new CustomEventHandler<VerifiedEventArgs>(this.mvpEvent.Verified);
			Exiled.Events.Handlers.Player.Dying += new CustomEventHandler<DyingEventArgs>(this.mvpEvent.Dying);
			Exiled.Events.Handlers.Server.WaitingForPlayers += new CustomEventHandler(this.mvpEvent.WaitingForPlayer);
			Exiled.Events.Handlers.Server.RoundEnded += new CustomEventHandler<RoundEndedEventArgs>(this.musicPlayer.RoundEnded);
			Exiled.Events.Handlers.Server.WaitingForPlayers += new CustomEventHandler(this.musicPlayer.WaitingForPlayer);
		}

		// 插件禁用时调用喵~
		public override void OnDisabled()
		{
			base.OnDisabled();
			// 取消订阅事件喵~
			Exiled.Events.Handlers.Player.Verified -= new CustomEventHandler<VerifiedEventArgs>(this.mvpEvent.Verified);
			Exiled.Events.Handlers.Player.Dying -= new CustomEventHandler<DyingEventArgs>(this.mvpEvent.Dying);
			Exiled.Events.Handlers.Server.WaitingForPlayers -= new CustomEventHandler(this.mvpEvent.WaitingForPlayer);
			Exiled.Events.Handlers.Server.RoundEnded -= new CustomEventHandler<RoundEndedEventArgs>(this.musicPlayer.RoundEnded);
			Exiled.Events.Handlers.Server.WaitingForPlayers -= new CustomEventHandler(this.musicPlayer.WaitingForPlayer);
			Plugin.Instance = null;
			Plugin.Singleton = null;
			this.mvpEvent = null;
			this.musicPlayer = null;
		}

		public static Plugin Instance;

		public static Plugin Singleton;

		private MusicPlayer musicPlayer;

		private MvpEvent mvpEvent;
	}
}
