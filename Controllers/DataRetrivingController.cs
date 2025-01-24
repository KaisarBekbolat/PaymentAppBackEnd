using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentProject.Models;
using PaymentProject.Models.DTOs.Outgoing;
using PaymentProject.Services;

namespace PaymentProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataRetrivingController : ControllerBase
    {
        private DataContext dataContext;

        public DataRetrivingController(DataContext context){
            dataContext = context;
        }

        [HttpGet]
        public IActionResult AllData(){
            var result = MappingService.OperationsToDto(dataContext.Payments.Select(x=>x));
            return Ok(result);
        }

        [HttpGet("name/")]
        public IActionResult GetByName([FromBody] DataBasedOnNameOrPhone nameBased){   // DataBasedOnNameOrPhone class was used because i need class for Binding [FromBody] from JSON
            CardInfo? cardInfoName = dataContext.CardInfos.FirstOrDefault(x=>x.Owner==nameBased.Name.ToUpper()); // Using FirstOrDefault - because there UNIQUE owners of card
            if(cardInfoName != null){
                var result = MappingService.OperationsToDto(dataContext.Payments.Where(x=>x.CardInfoId==cardInfoName.CardId));
                return Ok(result);
            }
            return NotFound($"No record with given name: {nameBased.Name}");
        }

        [HttpGet("number/")]
        public IActionResult GetByPhone([FromBody] DataBasedOnNameOrPhone numberBased){         
            Payment? phonePayment = dataContext.Payments.FirstOrDefault(x=>x.PhoneNumber==numberBased.PhoneNumber);
            if(phonePayment!=null){
                var result = MappingService.OperationsToDto(dataContext.Payments.Where(x=>x.PhoneNumber==numberBased.PhoneNumber));
                return Ok(result);
            }
            return NotFound($"No record with given phone number: {numberBased.PhoneNumber}");
        }
    }
}
