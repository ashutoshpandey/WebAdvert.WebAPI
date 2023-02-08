using AdvertAPI.Models;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAdvert.WebAPI.Models;
using WebAdvert.WebAPI.Services.Contracts;

namespace WebAdvert.WebAPI.Services
{
    public class DynamoDBAdvertStorageService : IAdvertStorageService
    {
        private IMapper _mapper;
        public DynamoDBAdvertStorageService(IMapper mapper) {
            _mapper = mapper;
        }

        [HttpPost]
        [Route("Add")]
        public async Task<string> Add(AdvertModel model)
        {
            var dbModel = _mapper.Map<AdvertDbModel>(model);

            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    dbModel.Id = new Guid().ToString();
                    dbModel.Status = AdvertStatus.Pending;
                    dbModel.CreationDateTime = DateTime.UtcNow;

                    await context.SaveAsync(dbModel);

                    return dbModel.Id;
                }
            }
        }

        [HttpPut]
        [Route("Confirm")]
        public async Task<bool> Confirm(ConfirmAdvertModel model)
        {
            using (var client = new AmazonDynamoDBClient())
            {
                using (var context = new DynamoDBContext(client))
                {
                    var record = await context.LoadAsync<AdvertDbModel>(model.Id);
                    if(record == null)
                    {
                        throw new KeyNotFoundException($"A record with ID = {model.Id} was not found");
                    }

                    if(model.Status == AdvertStatus.Active)
                    {
                        record.Status = AdvertStatus.Active;
                        await context.SaveAsync(record);

                        return true;
                    }
                    else
                    {
                        await context.DeleteAsync(record);
                        return false;
                    }
                }
            }
        }
    }
}
