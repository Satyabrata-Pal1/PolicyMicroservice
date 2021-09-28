using PolicyMicroservice.DTO;
using PolicyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PolicyMicroservice.Repository
{
    public interface IPolicyRepository
    {
        
        Task<int> GetQuotes(int businessValue,int propertyValue, ConsumerDTO.PropertyTypes propertyTypes,string token);
        Task<CreatePolicyResponseDTO> CreatePolicy(int policyId,ConsumerDTO consumerDTO,int policyMasterId,string token);
        CreatePolicyResponseDTO IssuePolicy(IssuePolicyDTO issuePolicyDTO);
        ConsumerPolicy ViewPolicy(int policyId, int consumerId, int agentId);


        int GetNewPolicyId();
        Task<ConsumerDTO> GetConsumerDetails(int agentId, int consumerId, int propertyId, string token);

        int CheckPolicy(ConsumerDTO consumerDTO);
        //Task<string> GetToken(HttpClient client);
        //for test purpose
        List<ConsumerPolicy> ViewAllPolicy();


    }
}
