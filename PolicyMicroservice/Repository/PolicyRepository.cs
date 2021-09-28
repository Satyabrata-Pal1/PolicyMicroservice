using Newtonsoft.Json;
using PolicyMicroservice.DTO;
using PolicyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PolicyMicroservice.Repository
{
    public class PolicyRepository : IPolicyRepository
    {
        string consumerBaseUri = "https://consumerservice/api/Consumers/";
        string quotesBaseUri = "https://quoteservice/api/Quotes/";
        public  readonly List<PolicyMaster> policyMasters;
        private List<ConsumerPolicy> consumerPolicys;
        public PolicyRepository()
        {
            consumerPolicys = new List<ConsumerPolicy>() { };
            policyMasters = new List<PolicyMaster>()
            {
                new PolicyMaster
                {
                    id =1,
                    propertyType = PolicyMaster.PropertyTypes.Building,
                    consumerType = "rental",
                    assuredSum = 2000000,
                    tenure = 2,
                    businessValue = 8,
                    propertyValue = 5,
                    baseLocation = "Chennai",
                    type = "replacement"

                },
                new PolicyMaster
                {
                    id =2,
                    propertyType = PolicyMaster.PropertyTypes.FactoryEquipment,
                    consumerType = "owner",
                    assuredSum = 400000,
                    tenure = 1,
                    businessValue = 9,
                    propertyValue = 10,
                    baseLocation = "Chennai",
                    type = "replacement"

                },
                new PolicyMaster
                {
                    id =3,
                    propertyType = PolicyMaster.PropertyTypes.PropertyInTransit,
                    consumerType = "owner",
                    assuredSum = 200000,
                    tenure = 1,
                    businessValue = 7,
                    propertyValue = 8,
                    baseLocation = "Pune",
                    type = "pay back"

                },

            };

        }

        public int GetNewPolicyId()
        {
            if(consumerPolicys.Count == 0)
            {
                return 1;
            }
            return consumerPolicys.Max(p => p.PolicyId) + 1;
        }


        public async Task<CreatePolicyResponseDTO> CreatePolicy(int policyId,ConsumerDTO consumerDTO,int policyMasterId,string token)
        {
            int quotesValue = 0;
            CreatePolicyResponseDTO createPolicyResponse = new CreatePolicyResponseDTO();
            ConsumerPolicy consumerPolicy = new ConsumerPolicy()
            {
                PolicyId = policyId,
                ConsumerId = consumerDTO.ConsumerId,
                BusinessId = consumerDTO.BusinessId,
                AgentId = consumerDTO.AgentId,

                PolicyStatus = "Initiated",
                    
            };
            PolicyMaster policymaaster = policyMasters.FirstOrDefault(p => p.id == policyMasterId);
            if (policymaaster == null)
            {
                return null;
            }
           try
            {

                quotesValue = await GetQuotes(policymaaster.businessValue, policymaaster.propertyValue, (ConsumerDTO.PropertyTypes)policymaaster.propertyType,token);

            }
            catch
            {
                return null;
            }
            try
            {

              
                consumerPolicy.CoveredSum = policymaaster.assuredSum;
                consumerPolicy.Duration = policymaaster.tenure;
                consumerPolicy.PolicyType = policymaaster.type;
                consumerPolicy.QuotesValue = quotesValue;
                consumerPolicys.Add(consumerPolicy);
                createPolicyResponse.ConsumerId = consumerDTO.ConsumerId;
                createPolicyResponse.PolicyId = consumerPolicy.PolicyId;
                return createPolicyResponse;
            }
            catch
            {
                return null;
            }


          
          
            
        }

        

       

        public CreatePolicyResponseDTO IssuePolicy(IssuePolicyDTO issuePolicyDTO)
        {
            //To get the policy
            ConsumerPolicy consumerPolicy = consumerPolicys.FirstOrDefault(c => c.PolicyId == issuePolicyDTO.PolicyId);
            CreatePolicyResponseDTO issuePolicyResponse = new CreatePolicyResponseDTO();
          
            if (consumerPolicy == null)
            {
                return null;
            }

            
            try
            {

                consumerPolicy.IssueStatus = true;
                consumerPolicy.PaymentDetails = issuePolicyDTO.PaymentDetails;
                consumerPolicy.PolicyStatus = "Issued";
                consumerPolicy.AcceptanceStatus = issuePolicyDTO.AcceptanceStatus;
                consumerPolicy.EffectiveDate = DateTime.Now;
                issuePolicyResponse.ConsumerId = consumerPolicy.ConsumerId;
                issuePolicyResponse.PolicyId = consumerPolicy.PolicyId;

                return issuePolicyResponse;
             }
            catch
            {
                return null;
            }


        }


        public  ConsumerPolicy ViewPolicy(int policyId, int consumerId, int agentId)
        {
            try
            {
                ConsumerPolicy consumerPolicy = consumerPolicys.FirstOrDefault(c => c.PolicyId == policyId);
                if (consumerPolicy == null)
                    return null;
                else
                    return consumerPolicy;
                /*if(consumerPolicy.ConsumerId == consumerId)
                {
                    return consumerPolicy;
                }
                else
                {
                    return null;
                }*/
            }
            catch
            {
                return null;
            }
            

        }



        //Method to get the consumer details from consumer microservice
        public async Task<ConsumerDTO> GetConsumerDetails(int agentId,int consumerId,int propertyId,string token)
        {
            BusinessDTO businessDTO;
            PropertyDTO propertyDTO;
            ConsumerDTO consumerDTO = new ConsumerDTO();

            //to get the busniess value
            try
            {
                using (var httpClient = new HttpClient())
                {
                   // string token = await GetToken(httpClient);
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.GetAsync($"{consumerBaseUri}ViewConsumerBusiness/{consumerId}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        businessDTO = JsonConvert.DeserializeObject<BusinessDTO>(apiResponse);
                    }
                }
            }
            catch
            {
                return null;
            }

            consumerDTO.BusinessValue = businessDTO.BusinessValue;
            consumerDTO.ConsumerId = businessDTO.ConsumerId;
          

            try
            {
                //to get the property value
                using (var httpClient = new HttpClient())
                {
                  //  string token = await GetToken(httpClient);
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.GetAsync($"{consumerBaseUri}ViewConsumerProperty/{consumerId}/{propertyId}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        propertyDTO = JsonConvert.DeserializeObject<PropertyDTO>(apiResponse);
                    }
                }
            }
            catch
            {
                return null;
            }


            consumerDTO.PropertyValue = propertyDTO.PropertyValue;
            consumerDTO.PropertyType = (ConsumerDTO.PropertyTypes)propertyDTO.PropertyType;
            consumerDTO.BusinessId = propertyDTO.BusinessId;
            consumerDTO.AgentId = agentId;
            consumerDTO.PropertyId = propertyId;
            return consumerDTO;
          
        }

        public  async Task<int> GetQuotes(int busniessValue, int propertyValue,ConsumerDTO.PropertyTypes propertyTypes,string token)
        {
            int value = 0;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.GetAsync($"{quotesBaseUri}GetQuote/{busniessValue}/{propertyValue}/{propertyTypes}"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        value = JsonConvert.DeserializeObject<int>(apiResponse);
                    }
                }
                return value;
            }
            catch
            {
                return 0;
            }
        }

       

       


        //Method to check with the master policy
        public int CheckPolicy(ConsumerDTO consumerDTO)
        {
            foreach(PolicyMaster policyMaster in policyMasters)
            {
                if(policyMaster.propertyType == (PolicyMaster.PropertyTypes)consumerDTO.PropertyType)
                {
                    if(consumerDTO.BusinessValue >= policyMaster.businessValue && consumerDTO.PropertyValue >= policyMaster.propertyValue)
                    {
                        return policyMaster.id;
                    }

                }

            }
            return 0;
           
        }

        public List<ConsumerPolicy> ViewAllPolicy()
        {
            return consumerPolicys;
        }

        /**

        public async  Task<string> GetToken(HttpClient client)
        {
            try
            {
                UserDTO user = new UserDTO { Id = 1, Username = "agent", Password = "agent" };
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, MediaTypeNames.Application.Json);
                var response = await client.PostAsync("https://localhost:44350/api/Authentication", content);
                var apiResponse = await response.Content.ReadAsStringAsync();
                JwtDTO jwt = JsonConvert.DeserializeObject<JwtDTO>(apiResponse);
                return jwt.Token;
            }
            catch
            {
                return null;
            }
            
        } **/
    }
}
