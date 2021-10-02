using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace Colleague_Of_The_Month.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(Message message, string[] openiningclosingdatedetails)
        {
            var emailMessage = CreateEmailMessage(message, openiningclosingdatedetails);
            Send(emailMessage);
        }
        private MimeMessage CreateEmailMessage(Message message, string[] openiningclosingdatedetails)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            string openingdate = openiningclosingdatedetails[0];
            string closingdate = openiningclosingdatedetails[1];
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<h2 style='color:red;'>The Voting session will open on the {0} and close {1}</h2>", openingdate,closingdate) };
            

            return emailMessage;
        }

        public void SendEmailManagerVotingSession(Message message, string[] openiningclosingdatedetails)
        {
            var votingSessionManagerEmailContent = CreateEmailVotingSessionManagerEmailContent(message, openiningclosingdatedetails);
            Send(votingSessionManagerEmailContent);
        }

        private MimeMessage CreateEmailVotingSessionManagerEmailContent(Message message, string[] openiningclosingdatedetails)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();

            //import image
            var image = builder.LinkedResources.Add(@"wwwroot/images/sd.jpg");
            var image2 = builder.LinkedResources.Add(@"wwwroot/images/fam.jpg");
            var image3 = builder.LinkedResources.Add(@"wwwroot/images/vn.jpg");
            //dates
            string openingdate = openiningclosingdatedetails[0];
            string closingdate = openiningclosingdatedetails[1];

            image.ContentId = MimeUtils.GenerateMessageId();
            image2.ContentId = MimeUtils.GenerateMessageId();
            image3.ContentId = MimeUtils.GenerateMessageId();


            // Set the html version of the message text
            builder.HtmlBody = string.Format(@"<div align=‘center’ style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><table border=‘0’ cellpadding=‘0’ cellspacing=‘0’ style=‘background: rgb(217, 217, 217); border-collapse: collapse; margin-left: calc(0%); width: 100%;’><tbody><tr><td style=‘width: 566.95pt; background: rgb(255, 255, 255); padding: 0cm; height: 4cm; vertical-align: top;’ valign=‘top’ width=‘100%’><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'>
<span style=‘width:942px;height:188px;’>
<img width=‘754’ src=""cid:{0}"" style=‘height: 1.566in; width: 7.85in;’ class=‘fr-fic fr-dii’></span><span style=‘color:#303641;’>&nbsp;</span></p></td></tr><tr><td style=‘width: 566.95pt; background: rgb(255, 255, 255); padding: 0cm; height: 4cm; vertical-align: top;’ valign=‘top’ width=‘100%’><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><br></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:32px;color:#303641;’>Voting Time</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'>
<strong><span style=‘font-size:21px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'>
<strong><span style=‘font-size:21px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:24px;color:#303641;’>You never know when a moment and a few sincere words</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'>
<strong><span style=‘font-size:24px;color:#303641;’>can have an impact on a life.</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'>
<strong><span style=‘font-size:24px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:21px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'>
<strong><span style=‘font-size:21px;color:#303641;’>So take a moment to express your gratitude.</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:19px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:19px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><span style=‘font-size:19px;color:#303641;’>
<strong><span style=‘font-size:21px;color:#303641;’>Opening Date for Voting Session: {3}.</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:19px;color:#303641;’>&nbsp;</span></strong></p>
<strong><span style=‘font-size:21px;color:#303641;’>Closing Date for Voting Session: {4}.</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:19px;color:#303641;’>&nbsp;</span></strong></p>

<strong>Click </strong></span><a href=‘https://google.com’ rel=‘noopener noreferrer’ target=‘_blank’><span style=‘font-size:19px;color:#303641;’>
<strong><u>here</u></strong></span></a><span style=‘font-size:19px;color:#303641;’><strong> to fill in the Survey</strong></span></p><p><br></p>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
<img width=‘729’ src=""cid:{1}"" class=‘fr-fic fr-dii’ style=‘width: 328px;’><p><br></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><br></p></td></tr></tbody></table></div><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>Kind Regard</span><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>&nbsp;</span></p><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>HR Team</span></p><div align=‘center’ style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><table border=‘0’ cellpadding=‘0’ cellspacing=‘0’ style=‘border-collapse:collapse;’><tbody><tr></tr></tbody></table></div><p>
<img width=‘754’ src=""cid:{2}"" style=‘width: 665px;’ class=‘fr-fic fr-dib’></p>





                   ", image.ContentId, image2.ContentId, image3.ContentId,openingdate,closingdate);


            emailMessage.Body = builder.ToMessageBody();


            return emailMessage;
        }
        public void SendEmailForWinners(Message message)
        {
            var WinnerEmailContent = CreateEmailForWinners(message);
            Send(WinnerEmailContent);
        }

        private MimeMessage CreateEmailForWinners(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();

            //import image
            var image = builder.LinkedResources.Add(@"wwwroot/images/wn.jpg");
            var image2 = builder.LinkedResources.Add(@"wwwroot/images/sdlogo.png");

            image.ContentId = MimeUtils.GenerateMessageId();
            image2.ContentId = MimeUtils.GenerateMessageId();

            // Set the html version of the message text
            builder.HtmlBody = string.Format(@"<p style=‘text-align: center;’>
<img src=""cid:{0}"" style=‘width: 300px;’ class=‘fr-fic fr-dib’></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>Hi Winner,</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;'>&nbsp;</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;margin-top:0cm;margin-bottom:11.25pt;line-height:16.5pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>Congratulations for being awarded Colleague Of the Month! Duly Deserved..<strong><span style='font-family:’Arial’,sans-serif;'>.<span class=‘fr-emoticon fr-deletable fr-emoticon-img’ style=‘background: url(https://cdnjs.cloudflare.com/ajax/libs/emojione/2.0.1/assets/svg/1f601.svg);’>&nbsp;</span></span></strong></span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;margin-top:0cm;margin-bottom:11.25pt;line-height:16.5pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>All your hard work and efforts have paid off. Keep it up!</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;margin-top:0cm;margin-bottom:11.25pt;line-height:16.5pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>Kindly click&nbsp;</span>
<a href=‘localhost:4200’ rel=‘noopener noreferrer’ target=‘_blank’><span style=‘font-size: 16px; font-family: Arial, sans-serif; color: rgb(84, 172, 210);’><u>here</u></span></a><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>&nbsp;to indicate your preferences from the 6 Vouchers.</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;margin-top:0cm;margin-bottom:11.25pt;line-height:16.5pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>Your voucher value is Rs 1000/-</span><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>&nbsp; &nbsp;&nbsp;</span></p><table border=‘0’ cellpadding=‘0’ cellspacing=‘3’><tbody><tr><td style=‘padding:.75pt .75pt .75pt .75pt;’><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>Kind Regards,</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>HR Team</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
 <img border=‘0’ width=‘150’ src=""cid:{1}"" fr-dib=‘‘ fr-fir’=‘‘ class=‘fr-fic fr-dii’></p></td></tr></tbody></table>


                                                 ", image.ContentId, image2.ContentId);


            emailMessage.Body = builder.ToMessageBody();


            return emailMessage;
        }

        public void SendEmailForVouchers(Message message)
        {
            var VoucherEmailContent = CreateEmailForVouchers(message);
            Send(VoucherEmailContent);
        }

        private MimeMessage CreateEmailForVouchers(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();

            //import image
            var image = builder.LinkedResources.Add(@"wwwroot/images/COTMBanner.png");
            image.ContentId = MimeUtils.GenerateMessageId();

            // Set the html version of the message text
            builder.HtmlBody = string.Format(@"<center><img src=""cid:{0}""></center>
                                                    <br>
                                                    <br>
                                                <div style = 'text - align:justify; '>
                                                   < p>Dear Colleagues,<br>
                                                <p>Please nominate your team member(s) whom you believe deserve to win the Colleague of the Month .<br>
                                                <p>To nominate, simply click here to fill in the form by latest Friday 5th February 2021.<br>
                                                <p>A Committee will vote for the Colleague of the Month and the announcement will be done in the next Monthly Briefing.<br>
                                                <p>Kind Regards,HR Team<br></div>
                                                ", image.ContentId);




            emailMessage.Body = builder.ToMessageBody();


            return emailMessage;
        }


        public void SendEmailForCOTMEventStart(Message message)
        {
            var COTMEventContent = CreateEmailForCOTMEvent(message);
            Send(COTMEventContent);

        }

        private MimeMessage CreateEmailForCOTMEvent(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();


            //import image
            var image = builder.LinkedResources.Add(@"wwwroot/images/sd.jpg");
            var image2 = builder.LinkedResources.Add(@"wwwroot/images/er.png");
            var image3 = builder.LinkedResources.Add(@"wwwroot/images/vn.jpg");

            image.ContentId = MimeUtils.GenerateMessageId();
            image2.ContentId = MimeUtils.GenerateMessageId();
            image3.ContentId = MimeUtils.GenerateMessageId();

            // Set the html version of the message text
            builder.HtmlBody = string.Format(@"<div align=‘center’ style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><table border=‘0’ cellpadding=‘0’ cellspacing=‘0’ style=‘background:#D9D9D9;border-collapse:collapse;’><tbody><tr><td style=‘width: 566.95pt; background: rgb(255, 255, 255); padding: 0cm; height: 4cm; vertical-align: top;’ valign=‘top’ width=‘100%’><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><span style=‘width:941px;height:189px;’>
<img width=‘753’ src=""cid:{0}"" style=‘height: 1.575in; width: 7.841in;’ class=‘fr-fic fr-dii’></span></p></td></tr><tr><td style=‘width: 566.95pt; background: rgb(255, 255, 255); padding: 0cm; height: 4cm; vertical-align: top;’ valign=‘top’ width=‘100%’><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><strong><span style=‘font-size:27px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:32px;color:#303641;’>Colleague of the Month</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><strong><span style=‘font-size:21px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:21px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:24px;color:#303641;’>You never know when a moment and a few sincere words</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:24px;color:#303641;’>can have an impact on a life.</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:24px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:21px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:21px;color:#303641;’>So take a moment to express your gratitude.</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:19px;color:#303641;’>&nbsp;</span></strong><strong><span style=‘font-size:19px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:19px;color:#303641;’>Click&nbsp;</span><span style=‘font-size: 19px; color: rgb(84, 172, 210);’><u>here</u></span><span style=‘font-size:19px;color:#303641;’>&nbsp;to fill in the Survey</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><br></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'>&nbsp;
<img src=""cid:{1}"" style=‘width: 563px;’ class=‘fr-fic fr-dib’></p><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>Kind Regard</span></p><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>HR Team</span></p><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>
<img src=""cid:{2}"" style=‘width: 664px;’ class=‘fr-fic fr-dib’></span></p><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><br></p></td></tr></tbody></table></div>


                                                 ", image.ContentId, image2.ContentId, image3.ContentId);




            emailMessage.Body = builder.ToMessageBody();


            return emailMessage;
        }
        public void SendEmailForShortlistPeriodOpen(Message message)
        {
            var COTMEventContent = CreateEmailForShortlistPeriodOpen(message);
            Send(COTMEventContent);

        }

        private MimeMessage CreateEmailForShortlistPeriodOpen(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var closingDateShortlistPeriod = message.Content;

            var builder = new BodyBuilder();


            //import image
            var image = builder.LinkedResources.Add(@"wwwroot/images/sl.png");
            var image2 = builder.LinkedResources.Add(@"wwwroot/images/sdlogo.png");

            image.ContentId = MimeUtils.GenerateMessageId();
            image2.ContentId = MimeUtils.GenerateMessageId();

            // Set the html version of the message text
            builder.HtmlBody = string.Format(@"<p style=‘text-align: center;’>
<img src=""cid:{0}"" style=‘width: 300px !important;’ class=‘fr-fic fr-dib’></p>
<p style=‘font-size: 15px; font-family: Calibri, sans-serif; margin: 0cm; text-align: center;’><span style='font-size:32px;font-family:’Arial’,sans-serif;color:#7C2855;'>COLLEAGUE OF THE MONTH</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>Dear Colleagues,</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;'>&nbsp;</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;margin-top:0cm;margin-bottom:11.25pt;line-height:16.5pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>You can now shortlist your team member(s) whom you believe deserve to win the Colleague of the Month for <strong><span style='font-family:’Arial’,sans-serif;'>January 2021.</span></strong></span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;margin-top:0cm;margin-bottom:11.25pt;line-height:16.5pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>To shortlist, simply Log In to COTM</span><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>&nbsp;by latest <strong><span style='font-family:’Arial’,sans-serif;'>Friday 5th February 2021.</span></strong></span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:16.5pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>A Committee will soon be scheduled to vote for the Colleague of the Month.</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'><br>&nbsp; &nbsp; &nbsp;</span></p><table border=‘0’ cellpadding=‘0’ cellspacing=‘3’><tbody><tr><td style=‘padding:.75pt .75pt .75pt .75pt;’><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>Kind Regards,</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;line-height:18.0pt;'><span style='font-size:16px;font-family:’Arial’,sans-serif;color:#74797C;'>HR Team</span></p><p style='margin-right:0cm;margin-left:0cm;font-size:15px;font-family:’Calibri’,sans-serif;margin:0cm;'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; 
<img border=‘0’ width=‘150’ src=""cid:{1}"" fr-dib=‘‘ fr-fir’=‘‘ class=‘fr-fic fr-dii’></p></td></tr></tbody></table>


                                                ", image.ContentId, image2.ContentId);




            emailMessage.Body = builder.ToMessageBody();


            return emailMessage;
        }
        public void SendEmailForChoiceOfVouchers(Message message, string[] details)
        {
            var COTMEventContent = CreateEmailForChoiceOfVouchers(message, details);
            Send(COTMEventContent);

        }

        private MimeMessage CreateEmailForChoiceOfVouchers(Message message, string[] details)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();
            string firstname = details[0];
            string lastname = details[1];
            string firstnamemanager = details[2];
            string lastnamemanager = details[3];
            string department = details[4];
            string division = details[5];
            string period = details[6];
            string vouchername = details[7];




            //import image
            var image = builder.LinkedResources.Add(@"wwwroot/images/sd.jpg");
            var image2 = builder.LinkedResources.Add(@"wwwroot/images/vn.jpg");

            image.ContentId = MimeUtils.GenerateMessageId();
            image2.ContentId = MimeUtils.GenerateMessageId();

            // Set the html version of the message text
            builder.HtmlBody = string.Format(@"<div align=‘center’ style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><table border=‘0’ cellpadding=‘0’ cellspacing=‘0’ style=‘background:#D9D9D9;border-collapse:collapse;’><tbody><tr><td style=‘width: 566.95pt; background: rgb(255, 255, 255); padding: 0cm; height: 4cm; vertical-align: top;’ valign=‘top’ width=‘100%’><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><span style=‘width:941px;height:189px;’>
<img width=‘753’ src=""cid:{0}"" style=‘height: 1.575in; width: 7.841in;’ class=‘fr-fic fr-dii’></span></p></td></tr><tr><td style=‘width: 566.95pt; background: rgb(255, 255, 255); padding: 0cm; height: 4cm; vertical-align: top;’ valign=‘top’ width=‘100%’><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;'><strong><span style=‘font-size:27px;color:#303641;’>&nbsp;</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><strong><span style=‘font-size:32px;color:#303641;’>Colleague of the Month</span>
</strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><br></p><p style=‘margin: 0cm; font-size: 15px; font-family: Calibri, sans-serif; text-align: left;’><strong>
<span style=‘font-size:19px;color:#303641;’>Dear Colleagues,</span></strong></p><p style=‘margin: 0cm; font-size: 15px; font-family: Calibri, sans-serif; text-align: left;’><strong><span style=‘font-size:19px;color:#303641;’>
{1}  {2} has chosen {3} as his/her voucher for {4}. Please do the needful asap.</span></strong></p><p style='margin:0cm;font-size:15px;font-family:’Calibri’,sans-serif;text-align:center;'><br></p>
<p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>Kind Regard</span>
</p><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>HR Team</span>
</p><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><span style='font-size:16px;line-height:105%;font-family:’Arial’,sans-serif;color:#303641;'>
<img src=""cid:{5}"" style=‘width: 664px;’ class=‘fr-fic fr-dib’></span></p><p style='margin:0cm;font-size:37px;font-family:’Calibri Light’,sans-serif;line-height:105%;'><br></p></td></tr></tbody></table></div>



                                                ", image.ContentId, firstname, lastname, vouchername, period, image2.ContentId);




            emailMessage.Body = builder.ToMessageBody();


            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, _emailConfig.EnableSsl);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.Username, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
