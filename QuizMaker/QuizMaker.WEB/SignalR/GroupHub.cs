using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Microsoft.AspNet.SignalR;
using QuizMaker.Interfaces.BL;
using QuizMaker.Models.QuestionModels;
using QuizMaker.WEB.Models;

namespace QuizMaker.WEB.SignalR
{
    public class GroupHub : Hub
    {
        public override Task OnConnected()
       {
            //Clients.All.user(Context.User.Identity.Name + "; "+ Context.ConnectionId);
            return base.OnConnected();
        }
        public void SendQuestion(string model, long quiz_id)
        {
            //JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            //QuestionViewModel q_model = json_serializer.Deserialize<QuestionViewModel>(model);
            //var s  = q_model.Question_text;

            Clients.Group(quiz_id.ToString()).message(model, quiz_id);
        }
        public void RemoveQuestion(long id, string groupName)
        {
            Clients.OthersInGroup(groupName).remove(id);
        }
        public void Broadcast(string username,string quizname,int quizid)
        {

            Clients.Others.user(username, quizname, quizid);
        }
        public void Disable(string groupName)
        {

            Clients.Group(groupName).disable();
        }
        public void Enable(string groupName)
        {

            Clients.Group(groupName).enable();
        }
        public async Task AddToGroup(string groupName)
        {
                await Groups.Add(Context.ConnectionId, groupName);
            //Clients.OthersInGroup(groupName).message($"{Context.User.Identity.Name} is online.");
        }

        public void RemoveFromGroup(string groupName, string username)
        {
            //await Groups.Remove(Context.ConnectionId, groupName);
            Clients.OthersInGroup(groupName).refresh(username);
        }

        public void ReadOnly(long questionId, bool disable, long groupName)
        {
            Clients.OthersInGroup(groupName.ToString()).readOnly(questionId, disable);
        }
        public void SendChanges(string model, long groupName)
        {

            Clients.OthersInGroup(groupName.ToString()).displayChanges(model, groupName);
        }
    }
}
