﻿using System;
using System.Collections.Generic;
using System.Linq;
using EasySCP682;
using Qurre;
using Qurre.API;
using Qurre.API.Events;
using Qurre.Events.Modules;
using Player = Qurre.API.Player;
using Round = Qurre.Events.Round;
using Server = Qurre.Events.Server;

namespace SCP682
{
    public class Plug : Plugin
    {
        public override string Developer => "ohayo!#5601";
        public override string Name => "EasySCP682";
        public override Version Version => new Version(1, 0, 0);
        public override int Priority => int.MinValue;
        public String pluginTag => "SCP682";
        private DateTime LastBreak = DateTime.Now;
        private Plug instance;
        public Plug getInstanse()
        {
            return instance;
        }

        public override void Enable()
        {
            instance = this;

            ConfigManager.registerCfg();

            if (ConfigManager.EasySCP682_enable == false)
            {
                Log.Error(" > the "+Name+" is disabled because you disabled it in the config");
                return;
            }

            Round.Start += new Main.AllEvents(this.Started);
            Qurre.Events.Player.InteractDoor += new Main.AllEvents<InteractDoorEvent>(this.Destroy);
            Qurre.Events.Player.Damage += new Main.AllEvents<DamageEvent>(this.Damage);
            Qurre.Events.Player.RoleChange += new Main.AllEvents<RoleChangeEvent>(this.Spawn);
            Qurre.Events.Player.Spawn += new Main.AllEvents<SpawnEvent>(this.Spawn);
            Qurre.Events.Player.Dead += new Main.AllEvents<DeadEvent>(this.Dead);
            Server.SendingRA += new Main.AllEvents<SendingRAEvent>(this.Ra);

            Log.Info(" "+Name+" enabled :)");
            Log.Info(" version: " + Version);
            Log.Info(" dev: " + Developer);
            Log.Info(" site: www.rootkovskiy.ovh");
        }
        public override void Disable()
        {
            instance = null;

            Round.Start -= new Main.AllEvents(this.Started);
            Qurre.Events.Player.InteractDoor -= new Main.AllEvents<InteractDoorEvent>(this.Destroy);
            Qurre.Events.Player.Damage -= new Main.AllEvents<DamageEvent>(this.Damage);
            Qurre.Events.Player.RoleChange -= new Main.AllEvents<RoleChangeEvent>(this.Spawn);
            Qurre.Events.Player.Spawn -= new Main.AllEvents<SpawnEvent>(this.Spawn);
            Qurre.Events.Player.Dead -= new Main.AllEvents<DeadEvent>(this.Dead);
            Server.SendingRA -= new Main.AllEvents<SendingRAEvent>(this.Ra);

            Log.Info(" " + Name + " disabled :(");
            Log.Info(" version: " + Version);
            Log.Info(" dev: "+Developer);
            Log.Info(" site: www.rootkovskiy.ovh");
        }
        private void Started()
        {
            if ((from x in Player.List where x.Tag.Contains(pluginTag) select x).Count<Player>() == 0)
            {
                List<Player> list = (from x in Player.List where x.UserId != null && x.UserId != string.Empty && !x.Overwatch select x).ToList<Player>();

                if (list.Count >= 10)
                {
                    Player pl = list[Extensions.Random.Next(list.Count)];
                    this.Spawn(pl);
                }
            }
        }
        private void Spawn(Player pl)
        {
            pl.Role = RoleType.Scp93989;
            pl.Hp = ConfigManager.EasySCP682_hp;
            pl.Tag += pluginTag;
            pl.CustomInfo = ConfigManager.EasySCP682_info;
            pl.Ahp = 0;
            pl.ShowHint("You are now SCP-682\nYou are the most dangerous object in the SCP Foundation\nBring chaos to this game!", 15);
        }

        private void Destroy(InteractDoorEvent ev)
        {
            if (ev.Player.Tag.Contains(pluginTag))
            {
                if ((DateTime.Now - this.LastBreak).TotalSeconds > ConfigManager.EasySCP682_cd)
                {
                    ev.Door.Destroyed = true;
                    this.LastBreak = DateTime.Now;
                } else
                {
                    ev.Player.ShowHint("Wait "+ (DateTime.Now - this.LastBreak).TotalSeconds + " seconds to break the door", 2);
                }
            }
        }
        private void Damage(DamageEvent ev)
        {
            if (ev.Attacker.Tag.Contains(pluginTag))
            {
                ev.Amount = ConfigManager.EasySCP682_damage;
            }
        }
        private void Dead(DeadEvent ev)
        {
            if (ev.Target.Tag.Contains(pluginTag))
            {
                ev.Target.CustomInfo = "";
                ev.Target.Tag.Replace(pluginTag, "");
            }
        }
        private void Spawn(RoleChangeEvent ev)
        {
            if (ev.Player.Tag.Contains(pluginTag))
            {
                if (ev.NewRole != RoleType.Scp93989)
                {
                    ev.Player.Tag = ev.Player.Tag.Replace(pluginTag, "");
                }
            }
        }
        private void Spawn(SpawnEvent ev)
        {
            if (ev.Player.Tag.Contains(pluginTag))
            {
                if (ev.RoleType != RoleType.Scp93989)
                {
                    ev.Player.Tag = ev.Player.Tag.Replace(pluginTag, "");
                }
            }
        }
        public void Ra(SendingRAEvent ev)
        {
            if (ev.Name == ConfigManager.EasySCP682_command)
            {
                ev.Prefix = "SCP682";
                ev.Allowed = false;
                try
                {
                    string text = string.Join(" ", ev.Args);
                    Player player = Player.Get(text);
                    bool flag2 = player == null;
                    if (flag2)
                    {
                        ev.Success = false;
                        ev.ReplyMessage = "Player not found";
                    }
                    else
                    {
                        ev.ReplyMessage = "Successfully";
                        this.Spawn(player);
                    }
                }
                catch
                {
                    ev.Success = false;
                    ev.ReplyMessage = "An error has occurred";
                }
            }
        }
    }
}