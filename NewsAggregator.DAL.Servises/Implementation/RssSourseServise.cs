using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Servises.Implementation
{
    public class RssSourseServise : IRssSourseServise
    {
        private readonly IUnitOfWork _unitOfWork;

        public RssSourseServise(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void dosms()
        {
            _unitOfWork.RssSourse.Dispose();
        }
    }
}
