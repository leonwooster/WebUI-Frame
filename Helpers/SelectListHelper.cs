using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AspCoreFrame.Entities;

namespace AspCoreFrame.WebUI.Helpers
{
    public static class SelectListHelper
    {
        public static SelectList ToSelectList(this IEnumerable<CommonDataBO> v, string which)
        {
            var s = v.Where(e => e.key_words == which).FirstOrDefault();

            if (s == null)
                throw new AppException($"No such value {which}");

            string[] split = s.key_values.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            List<SelectListItem> items = new List<SelectListItem>();
            foreach(var a in split)
            {
                items.Add(new SelectListItem()
                {
                    Text = a,
                    Value = a
                });
            }

            return new SelectList(items, "Value", "Text");
        }
    }
}
