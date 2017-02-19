using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using VkParser.Contexts;
using VkParser.Entities;
using VkParser.Models;
using Logic.Extensions.Models;

namespace VkParser.Work.Post
{
    public class VkPostLookUper : IDisposable
    {
        #region Constructors
        public VkPostLookUper(VkParseContext context)
        {
            _context = context;
        }
        #endregion

        #region Fields
        VkParseContext _context;
        #endregion

        #region Properties
        VkParseContext Context
        {
            get
            {
                return _context;
            }
        }


        #endregion

        #region Public Methods
        public async Task<WorkerResult> LookUpVkPostAsync(VkPerformerLookedUpPostModel model)
        {
            //если пост действительно существует
            if(await Context.VkFoundPosts.AnyAsync(x => x.Id == model.PostId))
            {
                //проверяем не существует ли уже просмотр этого пользователя по этому посту
                VkPostLookUp existingLookUp = await Context.VkPostLookUps
                    .FirstOrDefaultAsync(x => x.VkFoundPostId == model.PostId 
                                            && x.VkPerformerGuid == model.PerformerGuid);

                if(existingLookUp == null)
                {
                    VkPostLookUp lookUp = new VkPostLookUp
                    {
                        VkFoundPostId = model.PostId,
                        VkPerformerGuid = model.PerformerGuid,
                        Count = 1
                    };
                    Context.VkPostLookUps.Add(lookUp);
                    await Context.SaveChangesAsync();
                }
                else
                {
                    existingLookUp.Count++;
                    await Context.SaveChangesAsync();
                }
                //вне зависимости от того создаем мы новую запись или инкрементим старую
                //результат положителен
                return new WorkerResult
                {
                    Succeeded = true
                };
            }
            else
            {
                return new WorkerResult("Пост не найден в базе");
            }
            
        }
        
        public async Task<IEnumerable<VkPostLookUp>> GetPostLookUpsAsync(string performerGuid)
        {
            return await Context.VkPostLookUps.Where(p => p.VkPerformerGuid == performerGuid)
                .Include(x => x.VkPost).ToListAsync();
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // Для определения избыточных вызовов

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: освободить управляемое состояние (управляемые объекты).
                    if(_context != null)
                    {
                        _context.Dispose();
                        _context = null;
                    }
                }

                // TODO: освободить неуправляемые ресурсы (неуправляемые объекты) и переопределить ниже метод завершения.
                // TODO: задать большим полям значение NULL.
                
                disposedValue = true;
            }
        }

        // TODO: переопределить метод завершения, только если Dispose(bool disposing) выше включает код для освобождения неуправляемых ресурсов.
        // ~VkPostLookUper() {
        //   // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
        //   Dispose(false);
        // }

        // Этот код добавлен для правильной реализации шаблона высвобождаемого класса.
        void IDisposable.Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки выше, в методе Dispose(bool disposing).
            Dispose(true);
            // TODO: раскомментировать следующую строку, если метод завершения переопределен выше.
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
