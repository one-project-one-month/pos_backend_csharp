using DotNet7.PosBackendApi.Features.Setup.Staff;
using DotNet7.PosBackendApi.Models;
using DotNet7.PosBackendApi.Models.Setup.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace DotNet7.PosBackendApi.Features.Setup.Shop
{
    [Route("api/v1/shops")]
    [ApiController]
    public class ShopController : BaseController
    {
        private readonly ShopService _shopService;
        private readonly BL_Shop _bL_Shop;
        private readonly ResponseModel _response;

        public ShopController(ShopService shopService, BL_Shop bL_Shop, ResponseModel response)
        {
            _shopService = shopService;
            _bL_Shop = bL_Shop;
            _response = response;
        }

        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            try
            {
                var shopLst =  await _bL_Shop.GetShops();
                var responseModel = _response.ReturnGet(shopLst.Count,shopLst);
                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShop(int id)
        {
            try
            {
                var shop = await _bL_Shop.GetShop(id);
                var responseModel = _response.ReturnById(shop);
                return Ok(responseModel);
                //return Ok(await _shopService.GetShop(id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateShop(ShopModel shop)
        {
            try
            {
                JObject jObject = new JObject();
                jObject.Add("shop", "valueqwerqwer");
                var model = await _bL_Shop.CreateShop(shop);
                //var responseModel = _response.ReturnCommand(model.IsSuccess,model.Message,shop);
                var responseModel = _response.ReturnCommandV1(model.IsSuccess, model.Message,"shop" ,shop);
                return Content(JsonConvert.SerializeObject(responseModel),"application/json");
                //return Ok(await _shopService.CreateShop(shop));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(int id, ShopModel shop)
        {
            try
            {
                var model = await _bL_Shop.UpdateShop(id, shop);
                var responseModel = _response.ReturnCommand(model.IsSuccess, model.Message, shop);
                return Ok(responseModel);
                //return Ok(await _shopService.UpdateShop(id, shop));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            try
            {
                JObject jObject =  new JObject();
                jObject.Add("shop","valueqwerqwer");
                var model = await _bL_Shop.DeleteShop(id);
                var responseModel = _response.ReturnCommand(model.IsSuccess, model.Message, jObject:jObject);
                return Ok(responseModel);
               // return Ok(await _shopService.DeleteShop(id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            };
        }
    }
}
