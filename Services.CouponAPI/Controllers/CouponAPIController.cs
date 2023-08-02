﻿using Services.CouponAPI.Data;
using Services.CouponAPI.Models;
using Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using static Azure.Core.HttpHeader;
using Microsoft.EntityFrameworkCore;

namespace Services.CouponAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CouponAPIController : ControllerBase
	{
		private readonly AppDbContext _db;
		private readonly ResponseDto _response;
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
				IEnumerable<Coupon> couponList = _db.Coupons.ToList();
				_response.Result = _mapper.Map<IEnumerable<CouponDto>>(couponList);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}

		[HttpGet]
		[Route("{id:int}")]
		public ResponseDto Get(int id)
		{
			try
			{
				Coupon coupon = _db.Coupons.First(u => u.CouponId == id);
				_response.Result = _mapper.Map<CouponDto>(coupon);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}

		[HttpGet]
		[Route("GetByCode/{code}")]
		public ResponseDto Get(string code)
		{
			try
			{
				Coupon coupon = _db.Coupons.First(u => u.CouponCode.ToLower() == code.ToLower());
				_response.Result = _mapper.Map<CouponDto>(coupon);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}

		[HttpPost]
		public ResponseDto Post([FromBody] CouponDto couponDto)
		{
			try
			{
				Coupon obj = _mapper.Map<Coupon>(couponDto);
				_db.Coupons.Add(obj);
				_db.SaveChanges();

				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}

		[HttpPut]
		public ResponseDto Put([FromBody] CouponDto couponDto)
		{
			try
			{
				Coupon obj = _mapper.Map<Coupon>(couponDto);
				_db.Coupons.Update(obj);
				_db.SaveChanges();

				_response.Result = _mapper.Map<CouponDto>(obj);
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}

		[HttpDelete]
		public ResponseDto Put(int id)
		{
			try
			{
				Coupon obj = _db.Coupons.First(u => u.CouponId == id);
				_db.Coupons.Remove(obj);
				_db.SaveChanges();
			}
			catch (Exception ex)
			{
				_response.IsSuccess = false;
				_response.Message = ex.Message;
			}

			return _response;
		}
	}
}
