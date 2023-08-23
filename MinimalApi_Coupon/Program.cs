
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalApi_Coupon.Data;
using MinimalApi_Coupon.Models;
using MinimalApi_Coupon.Models.DTOs;
using MinimalApi_Coupon.Validations;
using System;
using System.Reflection.Metadata.Ecma335;

namespace MinimalApi_Coupon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddValidatorsFromAssemblyContaining<CouponCreateDTO>();
            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/api/coupons", () =>
            {
                APIResponse response = new APIResponse();

                response.Result = CouponStore.couponList;
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Results.Ok(response);
            }).WithName("GetCoupons")
            .Produces(200);

            app.MapGet("/api/coupon/{id:int}", (int id) =>
            {
                APIResponse response = new APIResponse();

                response.Result = CouponStore.couponList.FirstOrDefault(c => c.Id == id);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Results.Ok(response);
            }).WithName("GetCoupon");

            //app.MapPost("/api/coupon", async ([FromBody]Coupon coupon) =>
            //{
            //    if (coupon.Id != 0 || string.IsNullOrEmpty(coupon.Name))
            //    {
            //        return Results.BadRequest("Invalid ID or coupon name");
            //    }
            //    if (CouponStore.couponList.FirstOrDefault(c => c.Name.ToLower() == coupon.Name.ToLower()) != null)
            //    {
            //        return Results.BadRequest("Coupon name already exists");
            //    }

            //    coupon.Id = CouponStore.couponList.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;
            //    CouponStore.couponList.Add(coupon);
            //    return Results.Created($"/api/coupon/{coupon.Id}", coupon);
            //});

            app.MapPost("/api/coupon/", async ([FromServices]IValidator<CouponCreateDTO> _validator, [FromServices]IMapper _mapper, [FromBody]CouponCreateDTO couponCreate) =>
            {
                APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                var validationResult = await _validator.ValidateAsync(couponCreate);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(response);
                }
                if (CouponStore.couponList.FirstOrDefault(c => c.Name.ToLower() == couponCreate.Name.ToLower()) != null)
                {
                    response.ErrorMessages.Add("Coupon name is already in use");
                    return Results.BadRequest(response);
                }

                // Without automapper.
                //Coupon c = new Coupon()
                //{
                //    IsActive = coupon.IsActive,
                //    Name = coupon.Name,
                //    Percent = coupon.Percent
                //};

                //With automapper.
                Coupon coupon = _mapper.Map<Coupon>(couponCreate);


                coupon.Id = CouponStore.couponList.OrderByDescending(c => c.Id).FirstOrDefault().Id + 1;
                CouponStore.couponList.Add(coupon);

                CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);

                response.Result = couponDTO;
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.Created;
                return Results.Ok(response);
            }).WithName("CreateCoupon")
            .Accepts<CouponCreateDTO>("application/json")
            .Produces<APIResponse>(201)
            .Produces(400);

            app.MapPut("/api/coupon/", async (IMapper _mapper, IValidator<CouponUpdateDTO> _validator, [FromBody]CouponUpdateDTO couponUpdate) =>
            {
                // Add response.
                APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                // Add validation.
                var validateResult = await _validator.ValidateAsync(couponUpdate);

                if (!validateResult.IsValid)
                {
                    response.ErrorMessages.Add(validateResult.Errors.FirstOrDefault().ToString());
                }

                Coupon couponToUpdate = CouponStore.couponList.FirstOrDefault(c => c.Id == couponUpdate.Id);
                couponToUpdate.IsActive = couponUpdate.IsActive;
                couponToUpdate.Name = couponUpdate.Name;
                couponToUpdate.Percent = couponUpdate.Percent;
                couponToUpdate.LastUpdate = DateTime.Now;

                Coupon coupon = _mapper.Map<Coupon>(couponUpdate);

                response.Result = _mapper.Map<CouponDTO>(couponToUpdate);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Results.Ok(response);
            }).WithName("CouponUpdate")
            .Accepts<CouponUpdateDTO>("application/json")
            .Produces<APIResponse>(200);

            app.MapDelete("/api/coupon/{id:int}", (int id) =>
            {
                APIResponse response = new APIResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                Coupon couponToDelete = CouponStore.couponList.FirstOrDefault(c => c.Id == id);

                if (couponToDelete != null)
                {
                    CouponStore.couponList.Remove(couponToDelete);
                    response.IsSuccess = true;
                    response.StatusCode = System.Net.HttpStatusCode.NoContent;
                    return Results.Ok(response);
                }

                response.ErrorMessages.Add("Invalid ID");
                return Results.BadRequest(response);

            }).WithName("CouponDelete");

            app.Run();
        }
    }
}