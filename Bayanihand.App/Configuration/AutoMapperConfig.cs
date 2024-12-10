using AutoMapper;
using Bayanihand.App.Controllers;
using Bayanihand.App.Models;
using Bayanihand.DataModel;
using AutoMapper;
using System;

namespace Bayanihand.App.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Handyman, HandymanVM>().ReverseMap();
            CreateMap<Customer, CustomerVM>().ReverseMap();
            CreateMap<Service, ServiceVM>().ReverseMap();
            CreateMap<Schedule, ScheduleVM>().ReverseMap();
            CreateMap<CartCommission, HistoryVM>().ReverseMap();
            CreateMap<Rating, RatingVM>().ReverseMap();
        }
    }
}
