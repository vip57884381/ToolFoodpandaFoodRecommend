using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToolFoodpandaFoodRecommend.Models;
using static ToolFoodpandaFoodRecommend.Structure.STRUdeliveryheroV1Feed;

namespace ToolFoodpandaFoodRecommend
{
    public class Program 
    {
        static private string logFolder_ = @"Log\ToolFoodpandaFoodRecommend\";
        static private string url_ = @"https://disco.deliveryhero.io/search/api/v1/feed";
        static private string searchParameter_ = @"{
    ""location"": {
        ""point"": {
            ""longitude"": 121.402694,
            ""latitude"": 25.078557
        }
    },
    ""brand"": ""foodpanda"",
    ""config"": ""Original"",
    ""language_code"": ""zh"",
    ""language_id"": ""6"",
    ""country_code"": ""tw"",
    ""dynamic_pricing"": 0,
    ""use_free_delivery_label"": false,
    ""customer_type"": ""regular"",
    ""vertical_types"": [""restaurants""],
    ""opening_type"": ""delivery"",
    ""include_component_types"": [""vendors""],
    ""include_fields"": [""feed""],
    ""platform"": ""web"",
    ""limit"": 48, 
    ""offset"": 0
}
";
        static private int chooseNum_ = 5;
        static private string noSelectID_ = @"176,181";

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            List<int> noSelectIDList = noSelectID_
                .Split(',')?
                .Select(Int32.Parse)?
                .ToList();

            Dictionary<string, string> inParameter 
                = new Dictionary<string, string>();
            
            inParameter
                .Add(
                    ""
                    , searchParameter_
                );
            NetCustom netCustom 
                = new NetCustom();

            string result = netCustom
                .httpPost(
                    url_
                    , inParameter
                    , "application/json"
                );
            Directory
                .CreateDirectory(
                    logFolder_
                );
            File
                .WriteAllText(
                    string
                    .Format(
                        @"{0}{1}.txt"
                        , logFolder_
                        , DateTime.Now.ToString("yyyyMMddHHmmssffff")
                    )
                    ,result
                );
            Rootobject rootobject = JsonConvert
                .DeserializeObject<Rootobject>(
                    result
                );
            Item[] itemArr = rootobject
                .feed
                .items;
            Item1[] item1Arr = itemArr[0]
                .items;
            
            List<int> chooseList = new List<int>();
            int i_e = item1Arr
                .Length;
            List<int> noChooseList = new List<int>();
            for(int i = 0; i < i_e; i++)
            {
                Item1 item1Temp = item1Arr[i];
                Cuisine[] cuisineArrTemp = item1Temp
                    .characteristics
                    .cuisines;

                int j_e = cuisineArrTemp
                    .Count();
                for(int j = 0; j < j_e; j++)
                {
                    int id_j = cuisineArrTemp[j]
                        .id;
                    if (
                        noSelectIDList
                            .Contains(
                                id_j
                            ) == false
                    )
                        continue;
                    noChooseList
                        .Add(
                            i
                        );
                    break;
                }
            }
            i_e -= noChooseList
                .Count;
            
            Random random = new Random();
            while (
                 Math
                     .Min(
                         chooseList
                            .Count
                            ,i_e
                     ) < chooseNum_
            )
            {
                int num = random.Next() % i_e;
                if (
                    noChooseList
                        .Contains(
                            num
                        ) == true
                    || chooseList
                        .Contains(
                            num
                        ) == true
                )
                    continue;

                chooseList
                    .Add(
                        num
                    );
            }
            chooseList
                .Sort();

            int count = 1;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (int indexTemp in chooseList)
            {
                Item1 item1Temp = item1Arr[indexTemp];
                stringBuilder
                    .Append(
                        string
                            .Format(
                                @"No:{0}
店名:{1} ,星:{2}
網址:{3}
"
                                , count
                                , item1Temp
                                    .name
                                , item1Temp
                                    .rating
                                , item1Temp
                                    .redirection_url
                            )
                    );
                ++count;
            }

            LineNotify
                .sendMSG(
                    stringBuilder
                        .ToString()
                );
        }
    }
}
