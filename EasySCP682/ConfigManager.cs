﻿using System;
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
        public static int EasySCP682_doorDamageCD;
        public static int EasySCP682_damageMin;
        public static int EasySCP682_damageMax;
        public static int EasySCP682_minPlayers;
        public static int EasySCP682_spawnChance;

        public static String EasySCP682_allBroadcast;
        public static String EasySCP682_cassie;
        public static String EasySCP682_spawnBroadcast;
        public static String EasySCP682_doorDamageHint;

        public static String EasySCP682_Command_prefix;
        public static String EasySCP682_Command_playerNotFound;
        public static String EasySCP682_Command_successfully;
        public static String EasySCP682_Command_invalidUsage;


        public static void registerCfg()
        {
            EasySCP682_enable = Plug.Config.GetBool("EasySCP682_enable", true, "on or off EasySCP682?");
            EasySCP682_command = Plug.Config.GetString("EasySCP682_command", "scp682", "command for spawn scp682");
            EasySCP682_info = Plug.Config.GetString("EasySCP682_info", "SCP-682", "custom info for scp 682");
            EasySCP682_hp = Plug.Config.GetInt("EasySCP682_hp", 7000, "hp for scp 682");
            EasySCP682_doorDamageCD = Plug.Config.GetInt("EasySCP682_doorDamageCD", 30, "cd for door break in seconds");
            EasySCP682_damageMin = Plug.Config.GetInt("EasySCP682_damageMin", 89, "minimal damage when scp 682 bite players");
            EasySCP682_damageMax = Plug.Config.GetInt("EasySCP682_damageMax", 1000, "maximum damage when scp 682 bite players");
            EasySCP682_minPlayers = Plug.Config.GetInt("EasySCP682_minPlayers", 12, "min players to spawn scp 682");
            EasySCP682_spawnChance = Plug.Config.GetInt("EasySCP682_spawnChance", 50, "chance to spawn scp682");

            EasySCP682_allBroadcast = Plug.Config.GetString("EasySCP682_allBroadcast", "<color=red>ATTENTION!</color>\nSCP-682 has <color=#FF60A9>contaiment breached</color>\nEveryone evacuate <color=#FF60A9>immediately!</color>", "broadcast to all players when scp682 spawn");
            EasySCP682_cassie = Plug.Config.GetString("EasySCP682_cassie", "ATTENTION TO ALL PERSONNEL . SCP 6 8 2 ESCAPE . ALL HELICOPTERS AND MOBILE TASK FORCES IMMEDIATELY MOVE FORWARD TO ALL GATES . PLEASE EVACUATE IMMEDIATELY", "cassie when scp682 spawn");
            EasySCP682_spawnBroadcast = Plug.Config.GetString("EasySCP682_spawnBroadcast", "You are now <color=#FF60A9>SCP-682</color>\nYou are the most <color=#FF60A9>dangerous</color> object in the <color=#FF60A9>SCP Foundation</color>\nBring <color=#FF60A9>chaos</color> to this game!", "message displayed to scp682 when it spawn");
            EasySCP682_doorDamageHint = Plug.Config.GetString("EasySCP682_doorDamageHint", "Wait <color=#FF60A9>%doorDamageCD%</color> seconds to break the door", "hint message when scp682 break door");

            EasySCP682_Command_prefix = Plug.Config.GetString("EasySCP682_Command_prefix", "SCP682", "prefix for command");
            EasySCP682_Command_playerNotFound = Plug.Config.GetString("EasySCP682_Command_playerNotFound", "Player not found", "player not found message for command");
            EasySCP682_Command_successfully = Plug.Config.GetString("EasySCP682_Command_successfully", "Successfully", "successfully message for command");
            EasySCP682_Command_invalidUsage = Plug.Config.GetString("EasySCP682_Command_invalidUsage", "Invalid Usage", "invalid usage message for command");
        }
    }
}

