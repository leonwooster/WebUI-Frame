using System.Collections.Generic;
using System.Linq;

using AspCoreFrame.Data;
using AspCoreFrame.Entities;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;

using Org.BouncyCastle.Bcpg;

namespace AspCoreFrame.Services
{    
    public interface ICommonDataService
    {
        public IEnumerable<CommonDataBO> GetData(string country);
    }

    public class CommonDataService : ServiceBase, ICommonDataService
    {
        DataContext _ctx;
        public CommonDataService(DataContext ctx, IHttpContextAccessor httpContextAccessor) :
            base(ctx, httpContextAccessor)
        {
            _ctx = ctx;
        }

        public IEnumerable<CommonDataBO> GetData(string country)
        {
            if (string.IsNullOrEmpty(country))
                throw new AppException("Passed in parameter cannot be null.");

            var r = _ctx.tbl_DKSH_TPI_KEYWORDS.Where(e => e.country_code == country).ToList();

            return r;
        }
    }
}
