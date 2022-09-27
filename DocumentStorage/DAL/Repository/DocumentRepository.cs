using System;
using DocumentStorage.DAL.Context;
using DocumentStorage.DAL.Model;
using Microsoft.EntityFrameworkCore;

namespace DocumentStorage.DAL.Repository
{
    /// <summary>
    /// Репозиторий для взаимодействия с таблицей документы
    /// </summary>
    public class DocumentRepository : IDisposable
    {
        /// <summary>
        /// логер
        /// </summary>
        protected readonly ILogger _logger;

        /// <summary>
        /// Контекст БД
        /// </summary>
        protected AppPostgreContext _context;

        /// <summary>
        /// Таблица БД
        /// </summary>
        protected DbSet<DocumentInfoModel> _dbSetDocumentInfo;

        /// <summary>
        /// Таблица БД
        /// </summary>
        protected DbSet<FileModel> _dbSetFileModel;

        /// <summary>
        /// Констуктор
        /// </summary>
        /// <param name="context">Контекст БД</param>
        /// <param name="logger">Логгер</param>
        public DocumentRepository(AppPostgreContext context, ILogger<DocumentRepository> logger)
        {
            _context = context;
            _logger = logger;
            _dbSetDocumentInfo = context.Set<DocumentInfoModel>();
            _dbSetFileModel = context.Set<FileModel>();
        }


        #region Dispose

        private bool disposed = false;

        /// <summary>
        /// Не вызывать
        ///
        /// Сводка:
        /// Releases the allocated resources for this context.
        ///
        /// Примечания:
        /// See DbContext lifetime, configuration, and initialization for more information.
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        /// <summary>
        /// Не вызывать
        ///
        /// Сводка:
        /// Releases the allocated resources for this context.
        ///
        /// Примечания:
        /// See DbContext lifetime, configuration, and initialization for more information.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <param name="id">Ключ</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        public DocumentView? Get(Guid id)
        {
            var documentInfo = GetDocumentInfo(m => m.Id == id);
            if (documentInfo != null)
            {
                var file = GetFile(x => x.Id == documentInfo.DigitalBytesId);
                if (file != null)
                {
                    return new DocumentView(file, documentInfo);
                }
            }
            return null;
        }

        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        protected DocumentInfoModel? GetDocumentInfo(Func<DocumentInfoModel, bool> predicate) => _dbSetDocumentInfo.SingleOrDefault(predicate);

        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        protected IEnumerable<DocumentInfoModel> GetDocumentInfoCollection(Func<DocumentInfoModel, bool> predicate) => _dbSetDocumentInfo.Where(predicate);

        /// <summary>
        /// Получение одного объекта по ключу
        /// </summary>
        /// <param name="predicate">Предикат</param>
        /// <returns>Один объект или пусто если нет объекта с таким ключом</returns>
        protected FileModel? GetFile(Func<FileModel, bool> predicate)
        {
            return _dbSetFileModel.SingleOrDefault(predicate);
        }

        /// <summary>
        /// Создание объекта
        /// </summary>
        /// <param name="item">Объект</param>
        public Guid? Create(DocumentView item)
        {
            try
            {
                var file = new FileModel(item);
                var getFile = GetFile(m => m.Hash == file.Hash && m.Size == file.Size);
                Guid digitalBytesId;
                if (getFile != null)
                {
                    digitalBytesId = getFile.Id;
                }
                else
                {
                    _dbSetFileModel.Add(file);
                    digitalBytesId = file.Id;
                }
                var getDocumentInfoCollection = GetDocumentInfoCollection(m => m.DigitalBytesId == digitalBytesId);
                foreach (var getDocumentInfo in getDocumentInfoCollection)
                {
                    if (getDocumentInfo.FileName == item.FileName)
                    {
                        return getDocumentInfo.Id;
                    }
                }
                var documentInfo = new DocumentInfoModel(item, digitalBytesId);
                _dbSetDocumentInfo.Add(documentInfo);
                var res = _context.SaveChanges();
                return res > 0
                    ? (Guid?)documentInfo.Id
                    : throw new Exception($"Не удалось сохранить документ");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Не удалось сохранить документ" + Environment.NewLine + exception.Message);
                return null;
            }
        }
    }
}
