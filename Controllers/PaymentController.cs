using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProject.Models;
using PaymentProject.Models.DTOs;
using PaymentProject.Services;

namespace PaymentProject.Controllers
{
    [Route("api/[controller]")]       /// No need for explicitely checking ModelState binding results and etc
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private DataContext dataContext;
        public PaymentController(DataContext context){
            dataContext = context;
        }

        [HttpPost]
        public IActionResult ProcessPayment([FromBody] PaymentRequestDto paymentRequestDto){
            // TODO DataAnotations checks if Prefix and does encryption. But attribute returns bool
            try{
                //TODO continue encrypting and saving
                Payment payment = MappingService.DtoToPayment(paymentRequestDto, HttpContext, dataContext);
                dataContext.Payments.Add(payment);
                dataContext.SaveChanges();
                return Ok("Payment completed succesfully");
            }

            catch(KeyNotFoundException ex){
                return NotFound(new { message = ex.Message });
            }
            catch(DataException ex){
                return NotFound(new {message=ex.Message});
            }
        }
    }
}
