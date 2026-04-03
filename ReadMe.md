# 🎮 Đồ án: Lập Trình Mạng - Game Cờ Caro (UDM_17)

**Môn học:** Lập trình mạng (Network Programming)  
**Ngôn ngữ & Nền tảng:** C# .NET, Windows Forms  
**Giao thức mạng:** TCP/IP Socket  

---

## Mục Tiêu & Yêu Cầu Đề Tài 
Dựa trên yêu cầu của đồ án, hệ thống cần đáp ứng các tính năng cốt lõi sau:

* **Kết nối Client-Server:** Client có khả năng kết nối ổn định đến Server thông qua giao thức TCP/IP.
* **Cơ chế Thách đấu:** Người chơi có thể gửi yêu cầu (request) thách đấu tới người chơi khác đang trực tuyến thông qua sự điều phối của Server.
* **Thời gian suy nghĩ:** Tích hợp bộ đếm ngược (countdown timer) cho mỗi lượt đi của người chơi.
* **Lưu trữ lịch sử:** Server tự động ghi lại nhật ký và kết quả của các trận đấu đã diễn ra.
* **Quản lý lượt chơi:** Server kiểm soát logic lượt đi và tính toán thời gian suy nghĩ của từng Player để đảm bảo tính công bằng.

---

## Tiến Độ Hiện Tại 
Dự án hiện tại đã xây dựng thành công bộ khung cơ bản và hoàn thiện logic trò chơi đối kháng 1-vs-1:

### 1. Phía Máy chủ (Server)
* **TCP Socket Listener:** Lắng nghe và chấp nhận kết nối tại cổng `1234` (`127.0.0.1`).
* **Multi-threading:** Mở luồng (`Thread`) độc lập cho mỗi Client kết nối để xử lý dữ liệu song song.
* **Auto Matchmaking (Ghép cặp):** Chờ đủ 2 người chơi kết nối mới bắt đầu cho phép gửi/nhận dữ liệu.
* **Role Assignment (Phân vai tự động):** Máy chủ tự động chỉ định người vào trước là `X` (được đi trước), người vào sau là `O` (đi sau) và thông báo về cho Client.
* **Data Routing:** Chuyển tiếp chính xác tọa độ nước đi từ người chơi này sang màn hình người chơi kia.
* **Server Log:** Giao diện trực quan hiển thị trạng thái kết nối và nhật ký hệ thống.

### 2. Phía Người chơi (Client)
* **Giao diện hiện đại (Flat UI):** Tự động sinh bàn cờ 15x15 bằng code với thiết kế phẳng. Tích hợp thanh trạng thái (Status Bar) chỉ báo màu sắc theo vai trò (X: Đỏ, O: Xanh).
* **Thuật toán cốt lõi (Game Logic):** Viết thuật toán duyệt ma trận 4 hướng (Ngang, Dọc, Chéo xuôi, Chéo ngược) để tự động phát hiện 5 quân cờ liên tiếp và phân định Thắng/Thua.

---


