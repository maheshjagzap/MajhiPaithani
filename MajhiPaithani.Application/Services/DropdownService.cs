using MajhiPaithani.Application.DataAccess;
using MajhiPaithani.Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Services
{
    public class DropdownService
    {
        private readonly DropdownDataAccess _dataAccess;

        public DropdownService(DropdownDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<List<DropdownDto>> GetDropdownListAsync(int taskId)
        {
            return await _dataAccess.GetDropdownListAsync(taskId);
        }
    }
}
