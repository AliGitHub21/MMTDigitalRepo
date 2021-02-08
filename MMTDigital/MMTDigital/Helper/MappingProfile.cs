
using AutoMapper;
using MMTDigital.Model;
using MMTDigital.ViewModel;

namespace MMTDigital.Helper
{
    /// <summary>
    /// This class is used for mapping between model and view-model classes in the application using 'AutoMapper'
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerView>();
            CreateMap<CustomerRecentOrder, CustomerRecentOrderView>();
        }
    }
}
