﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI;

namespace BrickRed.WebParts.Facebook.Wall
{
    public class CommonHelper
    {
        private static JSONObject GetUserDetails(string UserID)
        {
            JSONObject obj = null;
            string url;
            HttpWebRequest request;
            string oAuthToken = string.Empty;

            try
            {
                url = string.Format("https://graph.facebook.com/{0}", UserID);

                //now send the request to facebook
                request = WebRequest.Create(url) as HttpWebRequest;
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string retVal = reader.ReadToEnd();

                    obj = JSONObject.CreateFromString(retVal);
                    if (obj.IsDictionary && obj.Dictionary.ContainsKey("error"))
                    {
                        throw new Exception(obj.Dictionary["error"].Dictionary["type"].String, new Exception(obj.Dictionary["error"].Dictionary["message"].String));
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
                //LblMessage = new Label();
                //LblMessage.Text = Ex.Message;
                //this.Controls.Add(LblMessage);
            }
            return obj;
        }

        private static string GetUserName(string UserID)
        {
            string strUserName = string.Empty;
            JSONObject me = GetUserDetails(UserID);
            if (me.Dictionary["name"] != null)
            {
                strUserName = me.Dictionary["name"].String;
            }
            return strUserName;
        }

        /// <summary>
        /// Create the Header of the Webpart
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static Table CreateHeader(string UserID, bool ShowHeaderImage)
        {
            Table tbHeader;
            TableRow trHeader;
            TableCell tcHeader;
            tbHeader = new Table();
            tbHeader.Width = Unit.Percentage(100);
            tbHeader.CellSpacing = 0;
            tbHeader.CellPadding = 0;
            trHeader = new TableRow();
            Table tbinner = new Table();
            tbinner.Width = Unit.Percentage(100);
            tbinner.CssClass = "fbHeaderMainTable";
            tbinner.CellSpacing = 0;
            tbinner.CellPadding = 0;
            tcHeader = new TableCell();
            TableRow trinner = new TableRow(); 
            TableCell tcinner;

            if (ShowHeaderImage)
            {
                tcinner = new TableCell();
                tcinner.Width = Unit.Percentage(5);
                System.Web.UI.HtmlControls.HtmlImage image = new System.Web.UI.HtmlControls.HtmlImage();
                image.Src = string.Format("https://graph.facebook.com/{0}/picture", UserID);
                image.Attributes.Add("style", "margin-left:10px;vertical-align: middle;");
                image.Height = 50;
                image.Width = 50;
                image.Border = 0;
                HyperLink hplnkImage = new HyperLink();
                hplnkImage.NavigateUrl = "http://www.facebook.com/profile.php?id=" + UserID;
                hplnkImage.Attributes.Add("target", "_blank");
                hplnkImage.Controls.Add(image);
                tcinner.Controls.Add(hplnkImage);
                tcinner.VerticalAlign = VerticalAlign.Middle;
                trinner.Cells.Add(tcinner);
            }

            //Creating the name hyperlink in header
            tcinner = new TableCell();
            HyperLink hplnkName = new HyperLink();
            hplnkName.Text = GetUserName(UserID);
            hplnkName.NavigateUrl = "http://www.facebook.com/profile.php?id=" + UserID;
            hplnkName.Attributes.Add("target", "_blank");
            tcinner.Controls.Add(hplnkName);
            tcinner.CssClass = "fbHeaderLink";
            trinner.Cells.Add(tcinner);

            tcinner = new TableCell();
            tcinner.CssClass = "fbHeaderImage";
            System.Web.UI.HtmlControls.HtmlImage imageFB= new System.Web.UI.HtmlControls.HtmlImage();
            imageFB.Attributes.Add("class", "imageFB");   
            imageFB.Src = "http://www.facebook.com/images/fb_logo_small.png";
            imageFB.Border = 0;
            HyperLink hplnkImageFB = new HyperLink();
            hplnkImageFB.NavigateUrl = "http://www.facebook.com/";
            hplnkImageFB.Attributes.Add("target", "_blank");
            hplnkImageFB.BorderStyle = System.Web.UI.WebControls.BorderStyle.None;
            hplnkImageFB.Controls.Add(imageFB);
            tcinner.Controls.Add(hplnkImageFB);
            trinner.Cells.Add(tcinner);
            tbinner.Rows.Add(trinner);
            tcHeader.Controls.Add(tbinner);
            trHeader.Cells.Add(tcHeader);
            tbHeader.Rows.Add(trHeader);
            return tbHeader;
        }

        public static LiteralControl InlineStyle()
        {
            LiteralControl ltrInlineCSS = new LiteralControl();
            ltrInlineCSS.Text = "<link href='/_layouts/BrickRed.WebParts.Facebook.Wall/style.css' rel='stylesheet' type='text/css' />";
            return ltrInlineCSS;
        }
    }
}
