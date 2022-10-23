using System;
using Qurre.API;
using SCP682;

namespace EasySCP682
{
    class ConfigManager
    {
        public static bool EasySCP682_enable;
        public static String EasySCP682_command;
        public static String EasySCP682_info;
        public static int EasySCP682_hp;
        public static int EasySCP682_cd;
        public static int EasySCP682_damage;
        public static int EasySCP682_minPlayers;

        public static void registerCfg()
        {
            EasySCP682_enable = Plug.Config.GetBool("EasySCP682_enable", true, "on or off EasySCP682?");
            EasySCP682_command = Plug.Config.GetString("EasySCP682_command", "scp682", "command for spawn scp682");
            EasySCP682_info = Plug.Config.GetString("EasySCP682_info", "SCP-682", "custom info for scp 682");
            EasySCP682_hp = Plug.Config.GetInt("EasySCP682_hp", 10000, "hp for scp 682");
            EasySCP682_cd = Plug.Config.GetInt("EasySCP682_cd", 20, "cd for door break in seconds");
            EasySCP682_damage = Plug.Config.GetInt("EasySCP682_damage", 1000, "damage when scp 682 bite players");
            EasySCP682_minPlayers = Plug.Config.GetInt("EasySCP682_minPlayers", 10, "min players to spawn scp 682");
        }
    }
}

