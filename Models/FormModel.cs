using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

using AspCoreFrame.WebUI.Helpers.MVC;

namespace AspCoreFrame.WebUI.Models
{
    public class FormModel
    {
        public long id { get; set; }
    }
}
