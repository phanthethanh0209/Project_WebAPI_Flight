using AutoMapper;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IDocumentTypeService
    {
        Task<IEnumerable<ListDocTypeDTO>> GetAllDocumentType(int page = 1);
        Task<CreateDocTypeDTO> GetDocumentType(int id);
        Task<(bool Success, string ErrorMessage)> CreateDocumentType(CreateDocTypeDTO createDTO);
        Task<(bool Success, string ErrorMessage)> UpdateDocumentType(int id, CreateDocTypeDTO updateFlightDTO);
        Task<(bool Success, string ErrorMessage)> DeleteDocumentType(int id);

    }
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public DocumentTypeService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateDocumentType(CreateDocTypeDTO createDTO)
        {
            DocumentType newType = _mapper.Map<DocumentType>(createDTO);

            await _repository.DocumentType.CreateAsync(newType);
            return (true, null);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteDocumentType(int id)
        {
            DocumentType type = await _repository.DocumentType.GetByIdAsync(u => u.TypeID == id);
            if (type == null)
                return (false, "Document type not found.");

            await _repository.DocumentType.DeleteAsync(type);
            return (true, null);
        }

        public async Task<IEnumerable<ListDocTypeDTO>> GetAllDocumentType(int pageNumber)
        {
            IEnumerable<DocumentType> types = await _repository.DocumentType.GetAllWithPaginationAsync(null, pageNumber);
            return _mapper.Map<IEnumerable<ListDocTypeDTO>>(types);
        }

        public async Task<CreateDocTypeDTO> GetDocumentType(int id)
        {
            DocumentType type = await _repository.DocumentType.GetByIdAsync(x => x.TypeID == id);
            if (type == null)
            {
                return null;
            }
            return _mapper.Map<CreateDocTypeDTO>(type);
        }

        public async Task<(bool Success, string ErrorMessage)> UpdateDocumentType(int id, CreateDocTypeDTO updateTypeDTO)
        {
            DocumentType type = await _repository.DocumentType.GetByIdAsync(m => m.TypeID == id);
            if (type == null)
            {
                return (false, "Document Type not found");
            }

            _mapper.Map(updateTypeDTO, type);
            await _repository.DocumentType.UpdateAsync(type);
            return (true, null);
        }
    }
}
