using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentDTO>> GetAllDocument();
        //Task<FlightDTO> GetFLight(string name);
        Task<(bool Success, string ErrorMessage)> CreateDocument(CreateDocumentDTO createDTO);
        //Task<(bool Success, string ErrorMessage)> UpdateFlight(int id, CreateFlightDTO updateFlightDTO);
        Task<(bool Success, string ErrorMessage)> DeleteDocument(int id);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public DocumentService(IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateDocument(CreateDocumentDTO createDTO)
        {
            Document newDoc = _mapper.Map<Document>(createDTO);

            string fileResult = await WriteFile(createDTO.DocName);
            if (string.IsNullOrEmpty(fileResult))
            {
                return (false, "Error saving file.");
            }

            newDoc.DocName = fileResult;

            await _repository.Document.CreateAsync(newDoc);
            return (true, null);
        }
        private async Task<string> WriteFile(IFormFile file)
        {
            string fileName = file.FileName;
            try
            {
                //Đường dẫn của thư mục 'Upload/Files' được tạo từ thư mục hiện tại
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath); // tạo thư mục
                }

                string exactpath = Path.Combine(filePath, fileName); // lấy đg dẫn đến file được lưu trữ
                //ghi file vào đường dẫn đã xác định
                using (FileStream stream = new(exactpath, FileMode.Create))
                {
                    //Sao chép nội dung file từ đối tượng IFormFile vào luồng FileStream
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }

            return fileName;
        }

        private async Task<(byte[] FileData, string ContentType, string FileName)> DownloadFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", fileName);

            // Lấy kiểu nội dung file
            FileExtensionContentTypeProvider provider = new();
            if (!provider.TryGetContentType(filePath, out string? contentType))
            {
                contentType = "application/octet-stream";
            }

            // Đọc file thành mảng byte
            byte[] bytes = await File.ReadAllBytesAsync(filePath);

            // Trả về file data, kiểu nội dung và tên file
            return (bytes, contentType, Path.GetFileName(filePath));
        }

        public async Task<IEnumerable<DocumentDTO>> GetAllDocument()
        {
            IEnumerable<Document> documents = await _repository.Document.GetAllWithPaginationAsync();
            return _mapper.Map<IEnumerable<DocumentDTO>>(documents);
        }

        public async Task<(bool Success, string ErrorMessage)> DeleteDocument(int id)
        {
            Document document = await _repository.Document.GetByIdAsync(u => u.DocID == id);
            if (document == null)
                return (false, "Document not found.");

            await _repository.Document.DeleteAsync(document);
            return (true, null);
        }
    }

}
