using SneakerDrop.Database;
using SneakerDrop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SneakerDrop.Services
{
    public class ConfigurationsService
    {
        #region Singleton
        public static ConfigurationsService Instance
        {
            get
            {
                if (instance == null) instance = new ConfigurationsService();

                return instance;
            }
        }
        private static ConfigurationsService instance { get; set; }
        public ConfigurationsService()
        {

        }
        #endregion

        public Config GetConfig(string Key)
        {
            using (var context = new SDContext())
            {
                return context.Configurations.Find(Key);
            }
        }

        public int PageSize()
        {
            using (var context = new SDContext())
            {
                var pageSizeConfig = context.Configurations.Find("PageSize");

                return pageSizeConfig != null ? int.Parse(pageSizeConfig.Value) : 5;
            }
        }

        public int ShopPageSize()
        {
            using (var context = new SDContext())
            {
                var pageSizeConfig = context.Configurations.Find("ShopPageSize");

                return pageSizeConfig != null ? int.Parse(pageSizeConfig.Value) : 6;
            }
        }
    }
}
