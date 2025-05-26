# MVP Plugin for SCP: Secret Laboratory (Exiled)

这是一个为 SCP: Secret Laboratory 服务器设计的 Exiled 插件，用于实现 MVP (Most Valuable Player) 系统喵~。

## 功能

*   跟踪玩家的击杀数喵~。
*   在回合结束时计算并宣布 MVP 玩家喵~。
*   可以为 MVP 玩家播放指定的音乐喵~。
*   支持在回合结束时自动开启友伤背刺喵~。

## 配置

插件的配置文件位于 `EXILED\Configs\Plugins\m_v_p\端口.yml 

以下是主要的配置选项：

*   `IsEnabled`: 是否启用整个插件 默认为开启喵~。
*   `Debug`: 是否启用调试模式 默认为关闭喵~。
*   `IsEnableMVP`: 是否启用 MVP 功能 默认为开启喵~。
*   `IsEnableRoundEndedFF`: 是否在回合结束时启用友伤 默认为开启喵~。

    示例:
    m_v_p_music_path:
      "76561198xxxxxxxxx@steam": "\音乐路径\音乐.ogg"

    请确保音乐文件路径是服务器可以访问到的绝对路径 并且音频文件是单声道 48000Hz 频率的 OGG格式喵~

## 作者

ChunYu-椿雨喵~
