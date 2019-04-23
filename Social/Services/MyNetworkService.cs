using Social.Helpers;
using Social.Interfaces;
using Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Services
{
    public class MyNetworkService : INetwork
    {
        private YeauplatoContext _context;
        public MyNetworkService()
        {
            _context = new YeauplatoContext();
        }
        public bool AcceptRequest(User user, string id)
        {
            var userId = 0;
            try { userId = Support.Decode(id); }
            catch (Exception ex) { new EmailService().SendCrushEmail("", ex.Message); return false; }
            
            var requests = _context.ConnectionRequests.FirstOrDefault(
                conn => conn.ReceiverId == user.Id &&
                conn.Id == userId);

            if (requests is null) { return false; }
            var netowrk = new Network
            {
                Id = requests.Id,
                AcceptorId = requests.ReceiverId,
                InitiatorId = requests.SenderId,
                Time = DateTime.UtcNow
            };
            try
            {
                _context.ConnectionRequests.Remove(requests);
                _context.Networks.Add(netowrk);
                _context.SaveChanges();
                return false;
            }
            catch (Exception ex) { new EmailService().SendCrushEmail("", ex.Message); return false; }
        }
        public bool AcceptRequest(User user)
        {
            var requests = _context.ConnectionRequests.Where(conn => conn.ReceiverId == user.Id).ToList();
            if (requests is null) { return false; }
            if (requests.Count == 0) { return false; }

            List<Network> networks = new List<Network>();
            foreach (var _request in requests)
            {
                networks.Add(new Network
                {
                    AcceptorId = _request.ReceiverId,
                    InitiatorId = _request.SenderId,
                    Id = _request.Id,
                    Time = DateTime.UtcNow
                });
            }
            try
            {
                _context.Networks.AddRange(networks);
                _context.ConnectionRequests.RemoveRange(requests);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) { return false; }
        }
        /// <summary>
        /// Deletes a certain request
        /// </summary>
        /// <param name="user">person that delets</param>
        /// <param name="id">the id of the connection initiator being deleted</param>
        /// <returns></returns>
        public bool DeleteRequest(User user, string id)
        {
            var userId = 0;
            try { userId = Support.Decode(id); }
            catch (Exception ex) { new EmailService().SendCrushEmail("", ex.Message); return false; }

            var requests = _context.ConnectionRequests.FirstOrDefault(
                conn => conn.ReceiverId == user.Id &&
                conn.SenderId ==
                (_context.Users.FirstOrDefault
                (u => u.Id == userId).Id));

            if (requests is null) { return false; }            
            try
            {
                _context.ConnectionRequests.Remove(requests);
                _context.SaveChanges();
                return false;
            }
            catch (Exception ex) { new EmailService().SendCrushEmail("", ex.Message); return false; }
        }
        /// <summary>
        /// Deletes all connection requests
        /// </summary>
        /// <param name="user">user who deletes the cconnection request</param>
        /// <returns></returns>
        public bool DeleteRequest(User user)
        {
            var requests = _context.ConnectionRequests.Where(conn => conn.ReceiverId == user.Id).ToList();
            if (requests is null) { return false; }
            if (requests.Count == 0) { return false; }
            try
            {
                _context.ConnectionRequests.RemoveRange(requests);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex) { return false; }
        }
        public int CountMyNetwork(User user)
        {
            var myNetwork = _context.Networks.Where(_net =>( _net.InitiatorId == user.Id || _net.AcceptorId == user.Id));
            return myNetwork?.Count() ?? 0;
        }
        public int CountNewRequest(User user)
        {
            var request = _context.ConnectionRequests.Where(connr => connr.ReceiverId == user.Id).ToList();
            return request?.Count() ?? 0;
        }
        public List<string> GetMyNetwork(User user, int page, int count)
        {
            var myNetwork = _context.Networks
                .Where(_net => _net.InitiatorId == user.Id || _net.AcceptorId == user.Id)
                .OrderByDescending(_net=>_net.Time)
                .Skip(page*count)
                .Take(count);

            var displayData = new List<string>();
            var index = 1;
            foreach (var network in myNetwork)
            {
                var d = "";
                var friend = _context.Users.FirstOrDefault(_user => _user.Id == (network.InitiatorId == user.Id ? network.AcceptorId : network.InitiatorId));
                if (friend is null) { continue; }

                var friendProfile = _context.UserProfiles.FirstOrDefault(_user => _user.UserId == friend.Id);

                var image = Support.DefaultImage;
                var profileImage = _context.UserImages.Where(pi => pi.UserId == friend.Id).OrderByDescending(t => t.Id).FirstOrDefault();
                if (profileImage != null)
                {
                    var imageFile = _context.Images.FirstOrDefault(img => img.Id == profileImage.ImageId);
                    if (imageFile != null)
                        image = imageFile.Url;
                }
                d += $@"
                <div class='col-md-4'>
                    <div class='panel panel-default'>
                        <div class='panel-heading'>
                            <h2 class='panel-title'>{friend.FirstName} {friend.OtherNames}</h2>
                        </div>
                        <div class='zero_margin panel-body '>
                            <div class='picture_box'>
                                <img src='{image}' class='absolute' />
                            </div>
                        </div>
                        <div class='panel-footer text-center'>
                            <a href='#'><i class='ico-transparent ico-hover et-chat ico-xs'></i></a>
                            <a href='../../chat?id={Support.Encode(friend.Id)}'><i class='ico-transparent ico-hover et-envelope ico-xs'></i></a>
                            <a href='../../profile?id={Support.Encode(friend.Id)}'><i class='ico-transparent ico-hover ico-xs fa fa-user-o'></i></a>
                        </div>
                    </div>
                </div>";
                displayData.Add(d);
                index += 1;
            }
            //if (d != "") { d += "</div>"; displayData.Add(d); }
            return displayData;
        }
        public List<string> GetNewRequest(User user)
        {
            var _requests = new List<string>();

            var connectionRequests = _context.ConnectionRequests
                .Where(conn => conn.ReceiverId == user.Id)
                .OrderByDescending(conn => conn.Time)
                .ToList();

            var data = "<div><!-- SLIDE 1 --><ul class='list-unstyled nomargin nopadding text-left'>";
            var counter = 0;
            var slideCounter = 1;
            foreach (var connectionRequest in connectionRequests)
            {
                if (counter == 3) { counter = 0; slideCounter++; data += $"</ul></div><div><!-- SLIDE {slideCounter} --><ul class='list-unstyled nomargin nopadding text-left'>"; }
                var sender = _context.Users.FirstOrDefault(u => u.Id == connectionRequest.SenderId);
                var userImage = _context.Images
                    .FirstOrDefault(img => img.Id == _context.UserImages
                        .FirstOrDefault(uimg => uimg.UserId == sender.Id)
                        .ImageId);
                var image = $@"<li class='clearfix'>
                <div class='thumbnail featured clearfix pull-left picture_main'>
                    <a href='#'>
                        <img class='absolute' src='{(userImage is null? "assets/images/demo/shop/products/300x450/p10.jpg" : "assets/images/demo/shop/products/100x100/p10.jpg")}' width='80' height='80' alt='featured item'>
                    </a>
                </div>";

                var body = $@"
                <a class='block size-12' href='#'>{user.FirstName} {user.OtherNames}</a>
                <div>{Support.GetRelativetime(connectionRequest.Time)}</div>
                <div class='size-18 text-left'>
                    <button type='button' class='btn btn-primary btn-sm accept' id='{Support.Encode(connectionRequest.Id)}'>Accept</button>
                    <button type='button' class='btn btn-danger btn-sm refuse' id='{Support.Encode(connectionRequest.Id)}'>Refuse</button>
                </div></li>";

                data += image + body;
                _requests.Add(data);
                counter++;
            }
            if(_requests.Count % 3 != 0)
            {
                var last = _requests.LastOrDefault();
                last += "</ul></div>";
                _requests.RemoveAt(_requests.Count - 1);
                _requests.Add(last);
            }

            return _requests;
        }
        public (bool check, string message) SendRequest(User user, string reciverEmail)
        {
            //|| Support.CheckEmail(reciverEmail).check
            if (user.Email == reciverEmail.Trim()) { return (false, Support.ShowMessage("Cannot send request to your ownself!", Support.MessageType.ERROR)); }
            if (string.IsNullOrWhiteSpace(reciverEmail)) { return (false, Support.ShowMessage("We could not determine this email or email is incorrect!", Support.MessageType.ERROR)); }
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == reciverEmail.Trim());
            if (existingUser == null)
            {
                var email = new EmailService();
                var mess = "";
                try
                {
                    var domain = "http://localhost:55782/";
                    var @params = new Register
                    {
                        Email = reciverEmail,
                        Id = Support.GetID(),
                        ReceiverId = 0,
                        SenderId = user.Id,
                        Time = DateTime.UtcNow
                    };
                    var url = $"{domain}account/register?d={@params.ToEncryptJson(Support.Key)}";


                    var _emailService = new EmailService();
                    var hmltBody = _emailService.InviteTemplate($"{user.FirstName} {user.OtherNames}", url);

                    var response = _emailService.SendEmail(reciverEmail, hmltBody);
                    if (response)
                    {
                        mess = "We determined that this person is not yet on Yeauplato and we have sent a succesfull invitation!";
                    }
                    else { mess = "We determined that this person is not yet on Yeauplato and we have sent an invitation,\n However this invitation has not been sent succesfully. Kindly try again later."; }
                }
                catch (Exception ex) { mess = "We determined that this person is not yet on Yeauplato and we have sent an invitation,\n However this invitation has not been sent succesfully. Kindly try again later."; }
                return (false, Support.ShowMessage($"{mess}", Support.MessageType.INFO));
            }

            var connection = _context.ConnectionRequests.FirstOrDefault(conn => conn.SenderId == user.Id && conn.ReceiverId == existingUser.Id);
            if (connection != null)
            {
                return (false, Support.ShowMessage($"You have already sent a request to this email.", Support.MessageType.INFO));
            }
            var network = _context.Networks
                .FirstOrDefault(conn => conn.InitiatorId == user.Id && conn.AcceptorId == existingUser.Id || conn.AcceptorId == user.Id && conn.InitiatorId == existingUser.Id);
            if (network != null)
            {
                return (false, Support.ShowMessage($"This person is already part of your network.", Support.MessageType.INFO));
            }
            var request = new ConnectionRequest
            {
                Id = Support.GetID(),
                ReceiverId = existingUser.Id,
                SenderId = user.Id,
                Time = DateTime.UtcNow
            };
            _context.ConnectionRequests.Add(request);
            _context.SaveChanges();
            return (true, Support.ShowMessage("Connection request has been sent successfully!", Support.MessageType.SUCCESS));
        }

        public bool IsInMyNetwork(int id)
        {
            var check = true;
            var myNetwork = _context.Networks.FirstOrDefault(_net => (_net.InitiatorId == id || _net.AcceptorId == id));
            if (myNetwork == null)
            {
                check = false;
            }
            return check;
        }
    }
}