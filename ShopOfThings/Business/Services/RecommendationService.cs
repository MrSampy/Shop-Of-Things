using AutoMapper;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class RecommendationService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public RecommendationService(IUnitOfWork unitOfWork, IMapper createMapperProfile)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
        }

        //public async Task<IEnumerable<ProductModel>> GetProductsByRecent

    }
}
