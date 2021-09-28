using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PolicyMicroservice.DTO;
using PolicyMicroservice.Models;
using PolicyMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
namespace PolicyMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyRepository policyRepository;

        public PolicyController(IPolicyRepository policy)
        {
            policyRepository = policy;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("CreatePolicy")]
        public async  Task<IActionResult> CreatePolicy([FromBody] ConsumerPolicyDTO consumerPolicyDTO)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
           // var token = await HttpContext.GetTokenAsync("access_token");
            if(consumerPolicyDTO == null)
            {
                return BadRequest();
            }

            //Getting the consumer's business value,property value,property type
            ConsumerDTO consumerDTO = await policyRepository.GetConsumerDetails(1, consumerPolicyDTO.ConsumerId,consumerPolicyDTO.PropertyId, token);
            if(consumerDTO == null)
            {
                return BadRequest("Cannot get the consumer Details");
            }

            //To check with the policy master
            int value = policyRepository.CheckPolicy(consumerDTO);
            if (value == 0)
            {
                return BadRequest("Cannot create this policy as it does not matches with our criteria");
            }

            //Creating policy
            int max = consumerDTO.ConsumerId;
            CreatePolicyResponseDTO res =  await policyRepository.CreatePolicy(max,consumerDTO,value,token);
            if(res == null)
            {
                return BadRequest();
            }

            return Ok(res);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("IssuePolicy")]
        public  IActionResult IssuePolicy([FromBody] IssuePolicyDTO issuePolicyDTO)
        {
            if(issuePolicyDTO == null)
            {
                return BadRequest();
            }

            CreatePolicyResponseDTO res =  policyRepository.IssuePolicy(issuePolicyDTO);
            return Ok(res);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("ViewPolicy/{policyId}/{consumerId}/{agentId}")]
        public IActionResult ViewPolicy(int policyId,int consumerId,int agentId)
        {
            var res = policyRepository.ViewPolicy(policyId, consumerId, agentId);
            
            return Ok(res);
        }


       /** [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("GetQuotes/{businessValue}/{propertyValue}/{propertyTypes}")]
        public async Task<IActionResult> GetQuotes(int businessValue,int propertyValue,ConsumerDTO.PropertyTypes propertyTypes)
        {
            int res =  await policyRepository.GetQuotes( businessValue,  propertyValue, propertyTypes);
            if(res == 0)
            {
                return BadRequest();
            }
            return Ok(res);
        }
       **/

        //test purpose
        [HttpGet("ViewAllPolicy")]
        public IActionResult ViewAllPolicy()
        {
            var obj  = policyRepository.ViewAllPolicy();
            return Ok(obj);
        }

        /**[HttpGet("GetToken")]
        public async Task<IActionResult> GetToken()
        {
            HttpClient client = new HttpClient();
            string token = await policyRepository.GetToken(client);
            return Ok(token);
        }**/
    }
}
