using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolFoodpandaFoodRecommend.Structure
{
    public class STRUdeliveryheroV1Feed
    {
        public class Rootobject
        {
            public Feed feed { get; set; }
            public Meta meta { get; set; }
            public string request_id { get; set; }
        }

        public class Feed
        {
            public int count { get; set; }
            public Item[] items { get; set; }
        }

        public class Item
        {
            public string component { get; set; }
            public string headline { get; set; }
            public Item1[] items { get; set; }
            public string type { get; set; }
            public Ui ui { get; set; }
        }

        public class Ui
        {
            public string design_version { get; set; }
            public string layout { get; set; }
        }

        public class Item1
        {
            public bool accepts_instructions { get; set; }
            public string address { get; set; }
            public string address_line2 { get; set; }
            public int budget { get; set; }
            public Chain chain { get; set; }
            public Characteristics characteristics { get; set; }
            public City city { get; set; }
            public string code { get; set; }
            public Cuisine1[] cuisines { get; set; }
            public string custom_location_url { get; set; }
            public string customer_phone { get; set; }
            public string customer_type { get; set; }
            public string delivery_box { get; set; }
            public string delivery_fee_type { get; set; }
            public string delivery_provider { get; set; }
            public string description { get; set; }
            public object[] disclaimers { get; set; }
            public object[] discounts { get; set; }
            public Discounts_Info[] discounts_info { get; set; }
            public float distance { get; set; }
            public Food_Characteristics1[] food_characteristics { get; set; }
            public bool has_delivery_provider { get; set; }
            public bool has_online_payment { get; set; }
            public string hero_image { get; set; }
            public string hero_listing_image { get; set; }
            public int id { get; set; }
            public bool is_active { get; set; }
            public bool is_best_in_city { get; set; }
            public bool is_checkout_comment_enabled { get; set; }
            public bool is_delivery_enabled { get; set; }
            public bool is_new { get; set; }
            public object is_new_until { get; set; }
            public bool is_pickup_enabled { get; set; }
            public bool is_premium { get; set; }
            public bool is_preorder_enabled { get; set; }
            public bool is_promoted { get; set; }
            public bool is_replacement_dish_enabled { get; set; }
            public bool is_service_fee_enabled { get; set; }
            public bool is_service_tax_enabled { get; set; }
            public bool is_service_tax_visible { get; set; }
            public bool is_test { get; set; }
            public bool is_vat_disabled { get; set; }
            public bool is_vat_included_in_product_price { get; set; }
            public bool is_vat_visible { get; set; }
            public bool is_voucher_enabled { get; set; }
            public float latitude { get; set; }
            public string logo { get; set; }
            public float longitude { get; set; }
            public float loyalty_percentage_amount { get; set; }
            public bool loyalty_program_enabled { get; set; }
            public int maximum_express_order_amount { get; set; }
            public Metadata metadata { get; set; }
            public float minimum_delivery_fee { get; set; }
            public float minimum_delivery_time { get; set; }
            public float minimum_order_amount { get; set; }
            public float minimum_pickup_time { get; set; }
            public string name { get; set; }
            public object[] payment_types { get; set; }
            public string post_code { get; set; }
            public int premium_position { get; set; }
            public int primary_cuisine_id { get; set; }
            public float rating { get; set; }
            public string redirection_url { get; set; }
            public int review_number { get; set; }
            public int review_with_comment_number { get; set; }
            public float score { get; set; }
            public int service_fee_percentage_amount { get; set; }
            public int service_tax_percentage_amount { get; set; }
            public string tag { get; set; }
            public object[] tag_ids { get; set; }
            public Tag[] tags { get; set; }
            public string url_key { get; set; }
            public int vat_percentage_amount { get; set; }
            public Vendor_Legal_Information vendor_legal_information { get; set; }
            public int vendor_points { get; set; }
            public string vertical { get; set; }
            public string vertical_parent { get; set; }
            public string vertical_segment { get; set; }
            public string[] vertical_type_ids { get; set; }
            public string web_path { get; set; }
            public string website { get; set; }
        }

        public class Chain
        {
            public string code { get; set; }
            public string main_vendor_code { get; set; }
            public string name { get; set; }
            public string url_key { get; set; }
        }

        public class Characteristics
        {
            public Cuisine[] cuisines { get; set; }
            public Food_Characteristics[] food_characteristics { get; set; }
            public Primary_Cuisine primary_cuisine { get; set; }
        }

        public class Primary_Cuisine
        {
            public int id { get; set; }
            public bool main { get; set; }
            public string name { get; set; }
            public string url_key { get; set; }
        }

        public class Cuisine
        {
            public int id { get; set; }
            public bool main { get; set; }
            public string name { get; set; }
            public string url_key { get; set; }
        }

        public class Food_Characteristics
        {
            public int id { get; set; }
            public bool is_halal { get; set; }
            public bool is_vegetarian { get; set; }
            public string name { get; set; }
        }

        public class City
        {
            public string name { get; set; }
        }

        public class Metadata
        {
            public object available_in { get; set; }
            public object[] close_reasons { get; set; }
            public object[] events { get; set; }
            public bool has_discount { get; set; }
            public bool is_delivery_available { get; set; }
            public bool is_dine_in_available { get; set; }
            public bool is_express_delivery_available { get; set; }
            public bool is_flood_feature_closed { get; set; }
            public bool is_pickup_available { get; set; }
            public bool is_temporary_closed { get; set; }
            public string timezone { get; set; }
        }

        public class Vendor_Legal_Information
        {
            public string legal_name { get; set; }
            public string trade_register_number { get; set; }
        }

        public class Cuisine1
        {
            public int id { get; set; }
            public bool main { get; set; }
            public string name { get; set; }
            public string url_key { get; set; }
        }

        public class Discounts_Info
        {
            public string id { get; set; }
            public int value { get; set; }
        }

        public class Food_Characteristics1
        {
            public int id { get; set; }
            public bool is_halal { get; set; }
            public bool is_vegetarian { get; set; }
            public string name { get; set; }
        }

        public class Tag
        {
            public string code { get; set; }
            public string text { get; set; }
        }

        public class Meta
        {
            public string config { get; set; }
            public Dependency[] dependencies { get; set; }
            public int response_time { get; set; }
            public object[] testing_groups { get; set; }
        }

        public class Dependency
        {
            public string dependency_name { get; set; }
            public int response_time { get; set; }
            public int parsing_time { get; set; }
            public string origin { get; set; }
        }
    }
}
