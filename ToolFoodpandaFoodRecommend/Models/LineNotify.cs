using System;
using System.Data;
using System.Net;
using System.Text;
using ToolFoodpandaFoodRecommend.Settings;

namespace ToolFoodpandaFoodRecommend.Models
{
    public class LineNotify
    {
        static private bool sendFg_ = false;

        static public DataTable getSTRU()
        {
            DataTable dataTable = new DataTable("main");

            try
            {
                dataTable.Columns.Add(
                    "token"
                    , typeof(string)

                );
                dataTable.Columns.Add(
                    "msg"
                    , typeof(string)
                );
                dataTable.Columns.Add(
                    "isUseHtmlEncodeFg"
                    , typeof(bool)
                );
            }
            catch (Exception exception) { Console.WriteLine(exception.Message.ToString()); }

            return dataTable;
        }

        static public bool sendMSG(
            string inMSG
            )
        {
            return sendMSG(
                inMSG
                , true
            );
        }

        /// <summary>
        /// 寄 line notify
        /// </summary>
        /// <param name="inMSG"></param>
        /// <param name="isUseHtmlEncodeFg"></param>
        static public bool sendMSG(
            string inMSG
            , bool isUseHtmlEncodeFg
            )
        {
            bool result = false;

            if (
                SettingsLineNotify
                    .Default
                    .token
                    .Trim()
                    .Length == 0
            )
                goto fatal;

            result
                = sendMSG(
                    SettingsLineNotify
                        .Default
                        .token
                    , inMSG
                    , isUseHtmlEncodeFg
                );

            fatal:
            return result;
        }

        static public bool sendMSG(
            string inToken
            , string inMSG
            )
        {
            return sendMSG(
                inToken
                , inMSG
                , true
            );
        }

        /// <summary>
        /// 寄 line notify
        /// </summary>
        /// <param name="inToken"></param>
        /// <param name="inMSG"></param>
        /// <param name="isUseHtmlEncodeFg"></param>
        static public bool sendMSG(
            string inToken
            , string inMSG
            , bool isUseHtmlEncodeFg
        )
        {
            bool result = false;

            try
            {
                if (
                    sendFg_ == true
                )
                    goto fatal;


                DateTime dateTime = DateTime.Now;

                sendFg_ = true;

                string strUrl = "https://notify-api.line.me/api/notify";
                string strPostData = string.Format(
                    "message={0}"
                    ,
                        (
                            (isUseHtmlEncodeFg == true) ?
                                WebUtility.HtmlEncode("\r\n" + inMSG)
                                : "\r\n" + inMSG
                        )
                    );

                string strHeadersValue = string.Format(
                    "Bearer {0}"
                    , inToken
                );

                byte[] btyeArr = Encoding.UTF8.GetBytes(strPostData);
                Uri target = new Uri(strUrl);
                WebRequest request = WebRequest.Create(target);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = btyeArr.Length;
                request.Headers.Add("Authorization", strHeadersValue);

                using (
                    var dataStream = request.GetRequestStream()
                )
                {
                    dataStream.Write(
                        btyeArr
                        , 0
                        , btyeArr.Length
                    );
                }

                sendFg_ = false;
                
                result = true;
            }
            catch (Exception exception) { Console.WriteLine(exception.Message.ToString()); }

            fatal:
            return result;
        }
    }
}
