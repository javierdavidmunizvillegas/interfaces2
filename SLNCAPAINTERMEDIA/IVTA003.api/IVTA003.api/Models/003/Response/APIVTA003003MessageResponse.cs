using IVTA003.api.Models._002.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA003.api.Models._003.Response
{
    public class APIVTA003003MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        public List<InventSumIVTA003002> InventSumList { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
