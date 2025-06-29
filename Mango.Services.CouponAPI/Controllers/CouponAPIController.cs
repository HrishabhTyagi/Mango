using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        

        public CouponAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper; 
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _db.Coupons.ToList();
                var couponDtoList = _mapper.Map<List<CouponDto>>(objList);
                _response.Result = couponDtoList;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
                _response.Errors = [ex.ToString()];
            }
            return _response;
        }

        [HttpGet("{id:int}")]
        public ResponseDto GetById(int id)
        {
            var response = new ResponseDto();
            try
            {
                Coupon obj = _db.Coupons.First(a => a.CouponId == id);
                response.Result = _mapper.Map<Coupon, CouponDto>(obj); ;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Errors = [ex.ToString()];
            }
            return response;
        }

        [HttpGet("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            var response = new ResponseDto();
            try
            {
                var coupon = _db.Coupons
                    .FirstOrDefault(c => c.CouponCode.Equals(code, StringComparison.OrdinalIgnoreCase));
                if (coupon != null)
                {
                    var couponDto = _mapper.Map<CouponDto>(coupon);
                    response.Result = couponDto;
                }
                else
                {
                    response.Result = null;
                    response.Message = "Coupon not found.";
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Errors = [ex.ToString()];
            }
            return response;
        }


        [HttpPost]
        public ResponseDto Post([FromBody] CouponDto couponDto)
        {
            var response = new ResponseDto();
            try
            {
                if (couponDto == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid coupon data.";
                    return response;
                }

                var coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(coupon);
                _db.SaveChanges();

                var createdCouponDto = _mapper.Map<CouponDto>(coupon);
                response.Result = createdCouponDto;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Errors = [ex.ToString()];
            }
            return response;
        }


        [HttpPut("{id:int}")]
        public ResponseDto Put(int id, [FromBody] CouponDto couponDto)
        {
            var response = new ResponseDto();
            try
            {
                if (couponDto == null || id != couponDto.CouponId)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid coupon data.";
                    return response;
                }

                var coupon = _db.Coupons.FirstOrDefault(c => c.CouponId == id);
                if (coupon == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Coupon not found.";
                    return response;
                }

                // Map updated fields
                coupon.CouponCode = couponDto.CouponCode;
                if (double.TryParse(couponDto.DiscountAmount, out double discountAmount))
                {
                    coupon.DiscountAmount = discountAmount;
                }
                coupon.MinAmount = couponDto.MinAmount;

                _db.Coupons.Update(coupon);
                _db.SaveChanges();

                var updatedCouponDto = _mapper.Map<CouponDto>(coupon);
                response.Result = updatedCouponDto;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Errors = [ex.ToString()];
            }
            return response;
        }

        [HttpDelete("{id:int}")]
        public ResponseDto Delete(int id)
        {
            var response = new ResponseDto();
            try
            {
                var coupon = _db.Coupons.FirstOrDefault(c => c.CouponId == id);
                if (coupon == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Coupon not found.";
                    return response;
                }

                _db.Coupons.Remove(coupon);
                _db.SaveChanges();

                response.IsSuccess = true;
                response.Message = "Coupon deleted successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Errors = [ex.ToString()];
            }
            return response;
        }
    }
}
