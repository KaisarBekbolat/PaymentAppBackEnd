using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentProject.Models;
using PaymentProject.Models.DTOs;
using PaymentProject.Models.DTOs.Outgoing;
using PaymentProject.Services;
using PaymentProject.Services.Redis;

namespace PaymentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataRetrivingController : ControllerBase
    {
        private DataContext dataContext;
        private IRedisCacheService redisCacheService;

        public DataRetrivingController(DataContext context, IRedisCacheService redisCache){
            dataContext = context;
            redisCacheService = redisCache;
        }

        [HttpGet]
        public IActionResult AllData(){
            var result = MappingService.OperationsToDto(dataContext.Payments.Select(x=>x));
            return Ok(result);
        }

        [HttpPost("name/")]
        public IActionResult GetByName([FromBody] DataBasedOnNameOrPhone nameBased){   // DataBasedOnNameOrPhone class was used because i need class for Binding [FromBody] from JSON
            CardInfo? cardInfoName = dataContext.CardInfos.FirstOrDefault(x=>x.Owner==nameBased.Name.ToUpper()); // Using FirstOrDefault - because there UNIQUE owners of card
            var redisResult = redisCacheService.GetData<IEnumerable<UserOperationsDto>>(nameBased.Name.ToUpper());
            if(redisResult!=null){
                return Ok(redisResult);
            }
            if(cardInfoName != null){
                var result = MappingService.OperationsToDto(dataContext.Payments.Where(x=>x.CardInfoId==cardInfoName.CardId));
                redisCacheService.SetData(nameBased.Name.ToUpper(), result);
                return Ok(result);
            }
            return NotFound($"No record with given name: {nameBased.Name}");
        }

        [HttpPost("number/")]
        public IActionResult GetByPhone([FromBody] DataBasedOnNameOrPhone nameBased){  
            DataBasedOnNameOrPhone numberBased = new DataBasedOnNameOrPhone(); 
            numberBased.PhoneNumber = nameBased.PhoneNumber.Trim();  
            Payment? phonePayment = dataContext.Payments.FirstOrDefault(x=>x.PhoneNumber==numberBased.PhoneNumber);

            var redisResult = redisCacheService.GetData<IEnumerable<UserOperationsDto>>(numberBased.PhoneNumber);
            if(redisResult!=null){
                return Ok(redisResult);
            }

            if(phonePayment!=null){
                var result = MappingService.OperationsToDto(dataContext.Payments.Where(x=>x.PhoneNumber==numberBased.PhoneNumber));
                redisCacheService.SetData(numberBased.PhoneNumber, result);
                return Ok(result);
            }
            return NotFound($"No record with given phone number: {numberBased.PhoneNumber}");
        }
    }
}
