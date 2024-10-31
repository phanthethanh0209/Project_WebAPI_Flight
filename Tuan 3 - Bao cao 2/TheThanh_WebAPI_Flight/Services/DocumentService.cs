using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentDTO>> GetAllDocument(int page = 1);
        Task<DocumentDTO> GetDocByID(int docID);
        Task<(bool Success, string ErrorMessage)> CreateDocument(CreateDocumentDTO createDTO);
        Task<(byte[] FileData, string ContentType, string FileName)> DownloadFile(int docID);

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

            Flight flight = await _repository.Flight.GetByIdAsync(u => u.FlightID == createDTO.FlightID);

            string fileResult = await WriteFile(createDTO.DocName, flight.FlightNo);
            if (string.IsNullOrEmpty(fileResult))
            {
                return (false, "Error saving file.");
            }

            newDoc.DocName = fileResult;

            await _repository.Document.CreateAsync(newDoc);
            return (true, null);
        }
        private async Task<string> WriteFile(IFormFile file, string flightID)
        {
            string fileName = file.FileName;
            try
            {
                //Đường dẫn của thư mục 'Upload/Files' được tạo từ thư mục hiện tại
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", flightID);

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

        public async Task<(byte[] FileData, string ContentType, string FileName)> DownloadFile(int docID)
        {
            Document doc = await _repository.Document.GetByIdAsync(u => u.DocID == docID);

            // Lấy chuyến bay tương ứng với tài liệu này
            Flight flight = await _repository.Flight.GetByIdAsync(f => f.FlightID == doc.FlightID);

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", flight.FlightNo, doc.DocName);

            // Lấy kiểu nội dung file (dùng để ánh xạ các phần mở rộng file (như .txt, .jpg, .pdf) sang kiểu nội dung phù hợp (như text/plain, image/jpeg, application/pdf))
            FileExtensionContentTypeProvider provider = new();
            if (!provider.TryGetContentType(filePath, out string? contentType)) // lấy kiểu file dựa trên phần mở rộng
            {
                contentType = "application/octet-stream";
            }

            // Đọc file thành mảng byte để hệ thống có thể xử lý và truyền tải
            byte[] bytes = await File.ReadAllBytesAsync(filePath);

            // Trả về file data, kiểu nội dung và tên file
            return (bytes, contentType, Path.GetFileName(filePath));
        }

        public async Task<IEnumerable<DocumentDTO>> GetAllDocument(int pageNumber)
        {
            IEnumerable<Document> documents = await _repository.Document.GetAllWithPaginationAsync(null, pageNumber, 3, doc => doc.Flights, doc => doc.DocumentTypes);

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

        public async Task<DocumentDTO> GetDocByID(int docID)
        {
            Document doc = await _repository.Document.GetByIdAsync(x => x.DocID == docID, doc => doc.Flights, doc => doc.DocumentTypes);
            if (doc == null)
            {
                return null;
            }
            return _mapper.Map<DocumentDTO>(doc);
        }
    }

}
