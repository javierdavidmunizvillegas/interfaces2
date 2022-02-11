using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_004.Models._002.Response
{
    public class CourierResponse
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public int device_type { get; set; }
        public int ranking { get; set; }
        public int status { get; set; }
        public string document_id { get; set; }
        public string monkey_id { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public int city_id { get; set; }
        public string city_name { get; set; }
        public string city_lang { get; set; }
        public string city_country { get; set; }
        public string vehicle_id { get; set; }
        public string vehicle_model { get; set; }
        public string vehicle_type { get; set; }
        public string vehicle_license { get; set; }
        public int is_phone_verified { get; set; }
        public int is_documentation_verified { get; set; }
        public string reference { get; set; }
        public string version_app { get; set; }
        public Int64 accepted_terms { get; set; }
        public int capacity { get; set; }
        public string profile_pic { get; set; }
        public int is_chat_active { get; set; }
    }

}
