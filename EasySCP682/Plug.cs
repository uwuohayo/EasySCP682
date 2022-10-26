using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasySCP682;
using Qurre;
using Qurre.API;
using Qurre.API.Controllers;
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
        private DateTime LastBreak = DateTime.Now;
        public static List<Player> isSCP682 = new List<Player>();
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

            Round.Start += new Main.AllEvents(this.RoundStart);
            Qurre.Events.Player.InteractDoor += new Main.AllEvents<InteractDoorEvent>(this.Destroy);
            Qurre.Events.Player.Damage += new Main.AllEvents<DamageEvent>(this.Damage);
            Qurre.Events.Player.RoleChange += new Main.AllEvents<RoleChangeEvent>(this.Spawn);
            Qurre.Events.Player.Spawn += new Main.AllEvents<SpawnEvent>(this.Spawn);
            Qurre.Events.Player.Dead += new Main.AllEvents<DeadEvent>(this.Dead);
            Server.SendingRA += new Main.AllEvents<SendingRAEvent>(this.Ra);
            isSCP682.Clear();

            Log.Info(" "+Name+" enabled :)");
            Log.Info(" version: " + Version);
            Log.Info(" dev: " + Developer);
            Log.Info(" site: www.rootkovskiy.ovh");
        }
        public override void Disable()
        {
            instance = null;

            Round.Start -= new Main.AllEvents(this.RoundStart);
            Qurre.Events.Player.InteractDoor -= new Main.AllEvents<InteractDoorEvent>(this.Destroy);
            Qurre.Events.Player.Damage -= new Main.AllEvents<DamageEvent>(this.Damage);
            Qurre.Events.Player.RoleChange -= new Main.AllEvents<RoleChangeEvent>(this.Spawn);
            Qurre.Events.Player.Spawn -= new Main.AllEvents<SpawnEvent>(this.Spawn);
            Qurre.Events.Player.Dead -= new Main.AllEvents<DeadEvent>(this.Dead);
            Server.SendingRA -= new Main.AllEvents<SendingRAEvent>(this.Ra);
            isSCP682.Clear();

            Log.Info(" " + Name + " disabled :(");
            Log.Info(" version: " + Version);
            Log.Info(" dev: "+Developer);
            Log.Info(" site: www.rootkovskiy.ovh");
        }
        private void RoundStart()
        {
            isSCP682.Clear();
            if (isSCP682.Count == 0)
            {
                List<Player> list = (from x in Player.List where x.UserId != null && x.UserId != string.Empty && !x.Overwatch select x).ToList<Player>();

                if (list.Count >= ConfigManager.EasySCP682_minPlayers)
                {
                    Random rnd = new Random();
                    int random = rnd.Next(0, 1);
                    if (random == 1)
                    {
                        Player pl = list[Extensions.Random.Next(list.Count)];
                        this.Spawn(pl);
                    }
                }
            }
        }
        private void Spawn(Player pl)
        {
            List<Player> sendAttention = (from p in Player.List where p.UserId != null && p.UserId != string.Empty && p != pl select p).ToList<Player>();
            foreach(Player plr in sendAttention)
            {
                plr.Broadcast(ConfigManager.EasySCP682_allBroadcast, 15);
            }
            Cassie.Send(ConfigManager.EasySCP682_cassie, false, false, true);
            
            pl.Role = RoleType.Scp93989;
            pl.DisableEffect(Qurre.API.Objects.EffectType.Visuals939);
            pl.Hp = ConfigManager.EasySCP682_hp;
            isSCP682.Add(pl);
            pl.CustomInfo = ConfigManager.EasySCP682_info;
            pl.Scale = new UnityEngine.Vector3(1.2f, 1.2f, 1.2f);
            pl.Broadcast(ConfigManager.EasySCP682_spawnBroadcast, 15);
        }

        private void Destroy(InteractDoorEvent ev)
        {
            if (isSCP682.Contains(ev.Player))
            {
                if ((DateTime.Now - this.LastBreak).TotalSeconds > ConfigManager.EasySCP682_doorDamageCD)
                {
                    ev.Door.Destroyed = true;
                    this.LastBreak = DateTime.Now;
                    return;
                } else
                {
                    ev.Player.ShowHint(ConfigManager.EasySCP682_doorDamageHint.Replace("%doorDamageCD%", ConfigManager.EasySCP682_doorDamageCD.ToString()), 2);
                    return;
                }
            }
        }
        private void Damage(DamageEvent ev)
        {
            if (isSCP682.Contains(ev.Attacker))
            {
                Random rnd = new Random();
                int random = rnd.Next(0, 2);
                if (random == 1)
                {
                    ev.Amount = ConfigManager.EasySCP682_damage;
                } else
                {
                    ev.Amount = 99;
                }
            }
        }
        private void Dead(DeadEvent ev)
        {
            if (isSCP682.Contains(ev.Target))
            {
                ev.Target.CustomInfo = null;
                isSCP682.Remove(ev.Target);
            }
        }
        private void Spawn(RoleChangeEvent ev)
        {
            if (isSCP682.Contains(ev.Player))
            {
                if (ev.NewRole != RoleType.Scp93989)
                {
                    ev.Player.CustomInfo = null;
                    isSCP682.Remove(ev.Player);
                }
            }
        }
        private void Spawn(SpawnEvent ev)
        {
            if (isSCP682.Contains(ev.Player))
            {
                if (ev.RoleType != RoleType.Scp93989)
                {
                    ev.Player.CustomInfo = null;
                    isSCP682.Remove(ev.Player);
                }
            }
        }
        public void Ra(SendingRAEvent ev)
        {
            if (ev.Name == ConfigManager.EasySCP682_command)
            {
                ev.Prefix = "SCP682";
                ev.Allowed = false;
                if (ev.Args.Length == 1)
                {
                    try
                    {
                        Player player = Player.Get(ev.Args[0]);
                        if (player == null)
                        {
                            ev.Success = false;
                            ev.ReplyMessage = "Player not found";
                        }
                        else
                        {
                            ev.ReplyMessage = "Successfully";
                            Spawn(player);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex.StackTrace);
                    }
                } else
                {
                    ev.Success = false;
                    ev.ReplyMessage = "Invalid Usage";
                }
                }
            }
        }
    }

