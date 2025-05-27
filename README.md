# MVP Plugin for SCP: Secret Laboratory (Exiled)

这是一个为 SCP: Secret Laboratory 服务器设计的 Exiled 插件，用于实现 MVP (Most Valuable Player) 系统喵~。

## 功能

*   跟踪玩家的击杀数喵~
*   在回合结束时计算并宣布 MVP 玩家喵~
*   可以为 MVP 玩家播放指定的音乐喵~
*   支持在回合结束时自动开启友伤背刺喵~

## 安装

1.  从 <mcurl name="GitHub Releases" url="https://github.com/liangkga/SCPSL-ChunYu-MVP/releases"></mcurl> 下载最新版本的插件 `.dll` 文件，并将其放置到服务器的 `Exiled/Plugins` 文件夹中喵~
2.  安装AudioPlayerApi.dll依赖把这个放在依赖项里面`Exiled/Plugins/dependencies`
3.  启动服务器，插件会自动生成配置文件喵~
4.  根据需要修改配置文件 `EXILED\Configs\Plugins\m_v_p\端口.yml`喵~

## 配置

插件的配置文件位于 `EXILED\Configs\Plugins\m_v_p\端口.yml 

以下是主要的配置选项：

*   `IsEnabled`: 是否启用整个插件 默认为开启喵~
*   `Debug`: 是否启用调试模式 默认为关闭喵~
*   `IsEnableMVP`: 是否启用 MVP 功能 默认为开启喵~
*   `IsEnableRoundEndedFF`: 是否在回合结束时启用友伤 默认为开启喵~

    示例:
    m_v_p_music_path:
      "<SteamID64>@steam": "<绝对音乐文件路径>"

    请确保音乐文件路径是服务器可以访问到的绝对路径，并且音频文件是单声道、48000Hz 频率的 OGG 格式喵~

## 作者

ChunYu-椿雨和保障民生喵~
