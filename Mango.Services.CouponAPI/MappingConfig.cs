﻿using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(
                config =>
                {
                    config.CreateMap<CouponDto, Coupon>();
                    config.CreateMap<Coupon, CouponDto>();
                    config.CreateMap<List<CouponDto>, List<Coupon>>();
                    config.CreateMap<List<Coupon>, List<CouponDto>>();
                }
            );

            return mappingConfig;
        }
    }
}
