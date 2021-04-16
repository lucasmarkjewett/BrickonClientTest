using MoonSharp.Interpreter;
using Network;
using Network.Enums;
using Network.Extensions;
using Shared;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Server
{
    public class Server
    {
        private static ServerConnectionContainer serverConnectionContainer;
        static void consolebar()
        {
            for (int x= 0; x< 55; x++)
                    {
                        Console.Write("\u2588");
            }
            Console.Write("\n");
        }
        static void ragequit(Connection connection, ConnectionType type)
        { }
        public  static int port = 7557;
        public static string actionMessage = "ht";
        static void Main(string[] args)
        { 
            serverConnectionContainer = ConnectionFactory.CreateServerConnectionContainer(port, false);

            //2. Apply optional settings.
            #region Optional settings
            serverConnectionContainer.ConnectionLost += (a, b, c) => Console.WriteLine($"{serverConnectionContainer.Count} {b.ToString()} Connection lost {a.IPRemoteEndPoint.Port}. Reason {c.ToString()}");
            serverConnectionContainer.ConnectionEstablished += connectionEstablished;
            serverConnectionContainer.AllowBluetoothConnections = false;
            serverConnectionContainer.AllowUDPConnections = true;
            serverConnectionContainer.UDPConnectionLimit = Int32.MaxValue;

            #endregion Optional settings
            Console.WriteLine("For setting up a server its recommended to have basic networking knowledge.");
            Console.WriteLine("Hello brickoneer, to start server entar a port number:");
            port = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Server started succesfuly before making your server public please join it twice beacuse first player going to join it likely to crash due some issue, but if you join you fix the issue");
            Console.WriteLine("Server version: 1.0.0 (MAJOR,MINOR,PATCH)");
            Console.WriteLine("Commands: removeplayer (player nickname): removes a player ragequitserver: removers everyone breaks server listplayers: lists everyone (including players not in the server ");
            //Call start here, because we had to enable the bluetooth property at first.
            serverConnectionContainer.Start();

            Script script = new Script();
            UserData.RegisterAssembly();
            script.Globals["Brickon"] = new BrickonApi();

            script.DoFile("brickon.lua");

        cmddt:
            string[] cmd = Console.ReadLine().Split(' ');
            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i] == "removeplayer")
                {
                    int index = players.FindIndex(x => (x.username == cmd[i + 1]));
                    players.RemoveAt(index);
                }
                if (cmd[i] == "ragequitserver")
                {
                    players.Clear();
                    serverConnectionContainer.ConnectionEstablished += ragequit;
                }
                if (cmd[i] == "listplayers")
                {
                    consolebar();
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Nickname:          Index:         Health:              ");
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                       
                    for (int ii = 0; ii < players.Count; ii++)
                    {
                        Console.WriteLine(players[ii].username+"     "+ players[ii].id + "     " + players[ii].hearths);
                    }


                    consolebar();
                }
                if (cmd[i] == "teleport")
                {
                    int index = players.FindIndex(x => (x.username == cmd[i + 1]));
                    players[index].x = Single.Parse(cmd[i + 1+1]);
                    players[index].y = Single.Parse(cmd[i + 1 + 1+1]);
                    players[index].z = Single.Parse(cmd[i + 1 + 1+1]);

                }
                if (cmd[i] == "sethealth")
                {
                    int index = players.FindIndex(x => (x.username == cmd[i + 1]));
                    players[index].hearths = Int32.Parse(cmd[i + 1 + 1]);


                }
            }

            goto cmddt;
            //Thread.Sleep(Timeout.Infinite);
        }

        static List<Player> players = new List<Player>();

        private static void connectionEstablished(Connection connection, ConnectionType type)
        {
            connection.TIMEOUT = Int32.MaxValue;

            connection.KeepAlive = false;
            connection.Fragment = true;
            string user = "freebubax";
            bool plrdth = false;
            int index = 1;

            connection.RegisterStaticPacketHandler<GameUpdate>((position, _) =>
            {

                if (plrdth == false)
                {

                    user = position.Username;
                    Player plr = new Player(position.Username);

                    plr.username = position.Username;
                    plr.x = position.X;
                    plr.z = position.Z;


                    players.Add(plr);
                    index = players.FindIndex(x => (x.username == position.Username));
                    players[index].id = index;
                    Console.WriteLine("Brickon Server Player On The Server:" + plr.username + " Index:" + index + " playerscount:" + players.Count);
                    plrdth = true;
                }
                if(players[index].username == position.Username)
                {
                    players[index].x = position.X;
                players[index].y = position.Y;
                players[index].z = position.Z;
                }
                // Console.WriteLine(user);
            });

            connection.RegisterStaticPacketHandler<GameRequest>(GameRequestFromClient);
        }

        private static void GameRequestFromClient(GameRequest request, Connection connection)
        {
            GameResponse response = new GameResponse(request)
            {
                Username = "DT5",

                Players = Newtonsoft.Json.JsonConvert.SerializeObject(players),
                actionMessage = actionMessage

                
            };

            connection.Send(response);
        }
    }
}
