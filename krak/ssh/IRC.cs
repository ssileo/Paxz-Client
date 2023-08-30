using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;
using PaxzClient;
using UnityEngine;

namespace PaxzClient.ssh
{

  //  public class ShMessage
   // {
      //  public string Channel { get; set; }
       // public string Sender { get; set; }
    //    public string Msg { get; set; }
    }
  //  class IRC
   // {
      //  public static WatsonTcpClient client = new WatsonTcpClient("212.227.10.252", 2087);
      //  public static void StartClient()
    //    {
         //   client.Events.ServerConnected += ServerConnected;
         //   client.Events.ServerDisconnected += ServerDisconnected;
         //   client.Events.MessageReceived += MessageReceived;
       //     client.Callbacks.SyncRequestReceived = SyncRequestReceived;
       //     client.Connect();
     //   }
      //  static void MessageReceived(object sender, MessageReceivedEventArgs args)
     //   {
         //   try
      //      {
            //    ShMessage msg = JsonConvert.DeserializeObject<ShMessage>(Encoding.UTF8.GetString(args.Data));
         //       var sendern = "";
          //      if (msg.Sender.ToLower() != "integration")
        //        {
           //         sendern = msg.Sender + ": ";
         //       }

            //    var msgg = ($"[{msg.Channel}] {sendern}{msg.Msg.Replace(msg.Sender + ":", "")}");
         //       Paxz.Logger.LogMessage(msgg);
          //      Paxz.latest_irc_msg = msgg;
       //     }
         //   catch (Exception ex)
       //     {
          //      Paxz.Logger.LogError(Encoding.UTF8.GetString(args.Data));
       //     }

       // }

      //  public static void SendMessage(ShMessage message)
       // {
      //      client.Send(JsonConvert.SerializeObject(message));
       // }

      //  static void ServerConnected(object sender, EventArgs args)
    //    {
   //         Paxz.Logger.LogMessage("IRC: Server connected");
   //     }

   //     static void ServerDisconnected(object sender, EventArgs args)
      //  {
   //         Paxz.Logger.LogMessage("IRC: Server disconnected");
      //  }

   //     static SyncResponse SyncRequestReceived(SyncRequest req)
        //{
   //         return new SyncResponse(req, "Hello back at you!");
       // }
 //   }
//}
