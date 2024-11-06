using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;
using System.IO.Compression;
using System.Security.Claims;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.DTO;
using TheThanh_WebAPI_Flight.Repository;

namespace TheThanh_WebAPI_Flight.Services
{
    public interface IDocumentService
    {
        Task<IEnumerable<DocumentDTO>> GetAllDocument(int page = 1);
        Task<DocumentDTO> GetDocByID(int docID);
        Task<IEnumerable<DocumentDTO>> GetAllDocByTypeID(int typeId, int pageNumber);
        Task<IEnumerable<DocumentDTO>> GetAllDocByName(string docName, int pageNumber);
        Task<(bool Success, string ErrorMessage)> CreateDocument(CreateDocumentDTO createDTO);

        // download
        Task<(byte[] FileData, string ContentType, string FileName)> DownloadFile(int docID);
        Task<(byte[] FileData, string ContentType, string FileName)> DownloadFilesByFlightID(int docID);
        Task<(byte[] FileData, string ContentType, string FileName)> DownloadFilesByDocName(string docName, int pageNumber);

        //Task<(bool Success, string ErrorMessage)> UpdateFlight(int id, CreateFlightDTO updateFlightDTO);
        Task<(bool Success, string ErrorMessage)> DeleteDocument(int id);
    }

    public class DocumentService : IDocumentService
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DocumentService(IRepositoryWrapper repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool Success, string ErrorMessage)> CreateDocument(CreateDocumentDTO createDTO)
        {
            Document newDoc = _mapper.Map<Document>(createDTO);
            // Lấy HttpContext và kiểm tra nếu người dùng đã đăng nhập
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User?.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                Claim? userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    newDoc.UserID = userId;
                }
                else
                {
                    return (false, "Cannot retrieve the user ID from claims.");
                }
            }
            else
            {
                return (false, "User is not authenticated.");
            }

            Flight flight = await _repository.Flight.GetByIdAsync(u => u.FlightID == createDTO.FlightID);
            if (flight == null)
            {
                return (false, "flight has not been created yet");
            }

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

        public async Task<(byte[] FileData, string ContentType, string FileName)> DownloadFilesByFlightID(int flightId)
        {
            IEnumerable<Document> documents = await _repository.Document.GetAllAsync(f => f.FlightID == flightId);

            // Lấy chuyến bay tương ứng với tài liệu
            Flight flight = await _repository.Flight.GetByIdAsync(f => f.FlightID == flightId);
            if (flight == null)
            {
                throw new Exception("Flight not found.");
            }

            //Tạo tên file ZIP cho chuyến bay
            string zipFileName = $"{flight.FlightNo}_Documents.zip";
            //Tạo đường dẫn tạm trong hệ thống để lưu trữ file ZIP vừa tạo.
            string tempZipPath = Path.Combine(Path.GetTempPath(), zipFileName);

            //Tạo một stream cho file ZIP tại đường dẫn tạm, và mở chế độ nén cho phép thêm các file vào bên trong.
            using (FileStream zipFile = new(tempZipPath, FileMode.Create))
            using (ZipArchive zipArchive = new(zipFile, ZipArchiveMode.Create, leaveOpen: false))
            {
                foreach (Document document in documents)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", flight.FlightNo, document.DocName);

                    if (File.Exists(filePath))
                    {
                        //Tạo một entry mới trong file ZIP
                        ZipArchiveEntry entry = zipArchive.CreateEntry(document.DocName);
                        //Mở entry stream của entry trong file ZIP để ghi dữ liệu của file tài liệu.
                        using (Stream entryStream = entry.Open())
                        //Mở file tài liệu dưới dạng stream để đọc dữ liệu.
                        using (FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read))
                        {
                            //Sao chép dữ liệu từ file stream vào entry stream để ghi vào ZIP
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }
            }

            byte[] zipBytes = await File.ReadAllBytesAsync(tempZipPath);

            // Xóa file ZIP tạm sau khi đọc xong
            File.Delete(tempZipPath);

            return (zipBytes, "application/zip", zipFileName);
        }

        public async Task<(byte[] FileData, string ContentType, string FileName)> DownloadFilesByDocName(string docName, int pageNumber)
        {
            // Tìm kiếm các tài liệu theo tên với phân trang
            IEnumerable<Document> documents = await _repository.Document.GetAllWithPaginationAsync(
                x => x.DocName.Contains(docName),
                pageNumber,
                3
            );

            if (!documents.Any())
            {
                throw new Exception("No documents found with the specified name.");
            }

            // Tạo file ZIP tạm để lưu các tài liệu
            string zipFileName = $"{docName}_Documents.zip";
            string tempZipPath = Path.Combine(Path.GetTempPath(), zipFileName);

            using (FileStream zipFile = new(tempZipPath, FileMode.Create))
            using (ZipArchive zipArchive = new(zipFile, ZipArchiveMode.Create, leaveOpen: false))
            {
                foreach (Document document in documents)
                {
                    // Lấy chuyến bay đầu tiên tương ứng với tài liệu này (giả sử tất cả tài liệu cùng chuyến bay)
                    Flight flight = await _repository.Flight.GetByIdAsync(f => f.FlightID == document.FlightID);
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", flight.FlightNo, document.DocName);

                    if (File.Exists(filePath))
                    {
                        ZipArchiveEntry entry = zipArchive.CreateEntry(document.DocName);
                        using (Stream entryStream = entry.Open())
                        using (FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read))
                        {
                            await fileStream.CopyToAsync(entryStream);
                        }
                    }
                }
            }

            // Đọc file ZIP thành mảng byte
            byte[] zipBytes = await File.ReadAllBytesAsync(tempZipPath);

            // Xóa file ZIP tạm sau khi đã đọc xong
            File.Delete(tempZipPath);

            // Trả về file ZIP chứa dữ liệu
            return (zipBytes, "application/zip", zipFileName);
        }


        public async Task<IEnumerable<DocumentDTO>> GetAllDocument(int pageNumber)
        {
            IEnumerable<Document> documents = await _repository.Document.GetAllWithPaginationAsync(null, pageNumber, 3, doc => doc.Flights, doc => doc.DocumentTypes, u => u.Users);

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
            Document doc = await _repository.Document.GetByIdAsync(x => x.DocID == docID, doc => doc.Flights, doc => doc.DocumentTypes, u => u.Users);
            if (doc == null)
            {
                return null;
            }
            return _mapper.Map<DocumentDTO>(doc);
        }

        public async Task<IEnumerable<DocumentDTO>> GetAllDocByName(string docName, int pageNumber)
        {
            IEnumerable<Document> doc = await _repository.Document.GetAllWithPaginationAsync(x => x.DocName.Contains(docName), pageNumber, 3, doc => doc.Flights, doc => doc.DocumentTypes, u => u.Users);
            if (doc == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<DocumentDTO>>(doc);
        }

        public async Task<IEnumerable<DocumentDTO>> GetAllDocByTypeID(int typeId, int pageNumber)
        {
            IEnumerable<Document> doc = await _repository.Document.GetAllWithPaginationAsync(x => x.TypeID == typeId, pageNumber, 3, doc => doc.Flights, doc => doc.DocumentTypes, u => u.Users);
            if (doc == null)
            {
                return null;
            }
            return _mapper.Map<IEnumerable<DocumentDTO>>(doc);
        }
    }

}
