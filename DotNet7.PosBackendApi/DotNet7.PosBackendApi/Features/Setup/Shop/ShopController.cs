using DotNet7.PosBackendApi.Features.Setup.Staff;
using DotNet7.PosBackendApi.Models.Setup.Shop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet7.PosBackendApi.Features.Setup.Shop
{
    [Route("api/v1/Shops")]
    [ApiController]
    public class ShopController : BaseController
    {
        private readonly ShopService _shopService;

        public ShopController(ShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            try
            {
                return Ok(await _shopService.GetShops());
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
                return Ok(await _shopService.GetShop(id));
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

                return Ok(await _shopService.CreateShop(shop));
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
                return Ok(await _shopService.UpdateShop(id, shop));
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
                return Ok(await _shopService.DeleteShop(id));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            };
        }
    }
}
