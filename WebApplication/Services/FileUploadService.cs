using System;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApplication.Services
{
    public class FileUploadService<T>: TwoPhaseCommitServiceBase<T, DbException>
        where T: IHasFile
    {
        private readonly DbContext _dbContext;

        public FileUploadService(DbContext dbContext, ILogger logger) 
            : base(logger)
        {
            _dbContext = dbContext;
        }
        
        protected override async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }

        protected override async Task TryAsync(T obj)
        {
            using (var fs = new FileStream("", FileMode.Create))
            {
                await obj.FormFile.CopyToAsync(fs);
            }
        }

        protected override Task CatchAsync(T obj, DbException e)
        {
            // удалить файл
            return Task.CompletedTask;
        }
    }
}