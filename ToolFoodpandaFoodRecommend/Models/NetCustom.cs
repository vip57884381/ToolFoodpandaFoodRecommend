using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ToolFoodpandaFoodRecommend.Models
{
    public class NetCustom
    {
        public string ErrMsg
        {
            get
            {
                string temp = errMsg_;
                errMsg_ = "";
                return temp;
            }

            set
            {
                errMsg_ += (value + "\r\n");
            }
        }

        private WebRequest webRequest_ = null;
        private WebResponse webResponse_ = null;
        private string errMsg_ = "";

        private string[] regexMatches(
            string inRegexString,
            string inHtmlBody,
            int inGroupId)
        {
            string[] result = null;

            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            Regex regex = new Regex(inRegexString, options);
            MatchCollection matchCollection = regex.Matches(inHtmlBody);
            result = (from Match match in matchCollection
                      select match.Groups[inGroupId].Value).ToArray();

            return result;
        }

        public bool downloadFileByUrl(
            string inUrl,
            string inFile)
        {
            bool result = false;

            try
            {
                WebClient webclient = new WebClient();
                webclient.DownloadFile(inUrl, inFile);

                result = true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }

            return result;
        }

        public string fileNameSymbolReplace(
            string inFileName)
        {
            string ret = "";

            inFileName = inFileName.Replace("\\", "%5C%");
            inFileName = inFileName.Replace("/", "%2F%");
            inFileName = inFileName.Replace(":", "%3A%");
            inFileName = inFileName.Replace("*", "%2A%");
            inFileName = inFileName.Replace("?", "%3F%");
            inFileName = inFileName.Replace("\"", "%2F%");
            inFileName = inFileName.Replace("<", "%3C%");
            inFileName = inFileName.Replace(">", "%3E%");
            inFileName = inFileName.Replace("|", "%7C%");
            ret = inFileName;

            return ret;
        }

        public string[] getHrefUrl(
            string inHtml)
        {
            string[] result = null;

            result = regexMatches("\\shref\\s*=\\s*\"(.*?)\"", inHtml, 1);

            return result;
        }

        public string getHtml(
            string inUrl)
        {
            string result = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(inUrl);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(
                        receiveStream,
                        Encoding.GetEncoding(response.CharacterSet));
                }

                string html = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                result = html;
            }

            return result;
        }

        public string getHtmlBody(
            string inHtml)
        {
            RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            Regex reg = new Regex(@"<body(?:[^>]*)>(?<theBody>.*)</body>", options);
            Match body = reg.Match(inHtml);

            return body.Groups["theBody"].ToString();
        }

        public string[] getImgUrl(
            string inHtmlBody)
        {
            List<string> result = new List<string>();
            string[] resultTemp = null;

            RegexOptions regexOptions = RegexOptions.IgnoreCase;
            Regex regex = new Regex(
                 @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>",
                 regexOptions);
            MatchCollection matchCollection = regex.Matches(inHtmlBody);
            resultTemp = (from Match match in matchCollection
                          select match.Value).ToArray();

            regex = new Regex(
                 @"<img.*?src=""(?<src>[^""]*)""[^>]*>",
                 regexOptions);
            for (int i = 0; i < resultTemp.Count(); i++)
            {
                foreach (Match match in regex.Matches(resultTemp[i]))
                {
                    result.Add(match.Groups["src"].Value);
                }
            }

            return result.ToArray();
        }

        public string getImgUrlName(
            string inImgUrl)
        {
            string result = "";
            bool pointFg = false;

            for (int i = (inImgUrl.Length - 1); i >= 0; i--)
            {
                if (inImgUrl[i] == '.')
                {
                    pointFg = true;
                }
                else if (inImgUrl[i] == '/')
                {
                    if (pointFg == true)
                    {
                        for (int j = (i + 1); j < inImgUrl.Length; j++)
                        {
                            result += inImgUrl[j];
                        }
                    }
                    break;
                }
            }

            return result;
        }

        public string httpSymbolReplace(
            string inString)
        {
            string result = inString;

            result = result.Replace("+", "%2B");
            result = result.Replace("&", "%26");
            result = result.Replace("'%26'", "&");

            return result;
        }

        public string httpPost(
            string inUrl,
            Dictionary<string, string> inParameter,
            string inContentType = "")
        {
            string result = "",
              response = "";

            try
            {
                var url = inUrl;
                webRequest_ = WebRequest
                    .Create(url);

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < inParameter.Count(); i++)
                {
                    string keyTemp = inParameter.ElementAt(i).Key;
                    string valueTemp = inParameter.ElementAt(i).Value;

                    valueTemp = httpSymbolReplace(valueTemp);

                    if (i > 0)
                        stringBuilder.Append("'&'");

                    if (keyTemp.Length > 0)
                    {
                        stringBuilder
                            .Append(
                                string
                                    .Format(
                                        @"{0}={1}"
                                        , keyTemp
                                        , valueTemp
                                    )
                            );
                    }
                    else
                    {
                        stringBuilder
                            .Append(
                                string
                                    .Format(
                                        @"{0}"
                                        , valueTemp
                                    )
                            );
                    }
                }

                long postDataLen = stringBuilder.Length;
                var data = Encoding.UTF8.GetBytes(stringBuilder.ToString());

                webRequest_.Method = "POST";
                webRequest_.ContentType =
                    (inContentType.Length == 0) ?
                    "application/x-www-form-urlencoded" : inContentType;
                webRequest_.ContentLength = data.Length;
                webRequest_.Timeout = Timeout.Infinite;

                using (
                    var stream = webRequest_.GetRequestStream()
                )
                {
                    stream
                        .Write(
                            data
                            , 0
                            , data.Length
                        );
                }

                webResponse_ = webRequest_.GetResponse();

                if (((HttpWebResponse)webResponse_).StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = webResponse_.GetResponseStream();
                    response = new StreamReader(responseStream).ReadToEnd();

                    result = response;
                }
                else
                {
                    ErrMsg += (((HttpWebResponse)webResponse_).StatusCode).ToString();
                }
            }
            catch (Exception exception)
            {
                ErrMsg += exception.Message;
            }

            webRequest_ = null;
            webResponse_ = null;

            return result;
        }

        public bool httpPostInterrupt()
        {
            bool result = false;

            if (webRequest_ == null)
                goto fatal;

            webRequest_.Abort();
            result = true;

            fatal:
            return result;
        }

        public string httpPostWebServiceFilter(
            string inString)
        {
            string result = "";

            try
            {
                result = inString.Split('>')[2].Split('<')[0];
            }
            catch (Exception exception)
            {
                ErrMsg += exception.Message;
            }

            return result;
        }

        /// <summary> 
        /// 提取頁面連結 
        /// </summary> 
        /// 
        /// <param name="inHtml"></param> /// <returns></returns>
        public string[] getHtmlImageUrlList(string inHtml)
        {
            // 定義正則表達式用來匹配 img 標籤 
            Regex regImg = new Regex(
                @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>",
                RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(inHtml);
            List<string> sUrlList = new List<string>();

            // 取得匹配項列表 
            foreach (Match match in matches)
                sUrlList.Add(match.Groups["imgUrl"].Value);
            return sUrlList.ToArray();
        }
    }
}
