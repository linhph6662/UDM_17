# 🎮 Đồ án: Lập Trình Mạng - Game Cờ Caro (UDM_17)

**Môn học:** Lập trình mạng (Network Programming)  
**Ngôn ngữ & Nền tảng:** C# .NET, Windows Forms  
**Giao thức mạng:** TCP/IP Socket  

---

## 🎯 Mục Tiêu & Yêu Cầu Đề Tài (Tính Năng Chính)
Dựa trên yêu cầu của đồ án, hệ thống cần đáp ứng các tính năng cốt lõi sau:

* **🌐 Kết nối Client-Server:** Client có khả năng kết nối ổn định đến Server thông qua giao thức TCP/IP.
* **⚔️ Cơ chế Thách đấu:** Người chơi có thể gửi yêu cầu (request) thách đấu tới người chơi khác đang trực tuyến thông qua sự điều phối của Server.
* **⏳ Thời gian suy nghĩ:** Tích hợp bộ đếm ngược (countdown timer) cho mỗi lượt đi của người chơi.
* **🗄️ Lưu trữ lịch sử:** Server tự động ghi lại nhật ký và kết quả của các trận đấu đã diễn ra.
* **⚖️ Quản lý lượt chơi:** Server kiểm soát logic lượt đi và tính toán thời gian suy nghĩ của từng Player để đảm bảo tính công bằng.

---

## 🚀 Tiến Độ Hiện Tại (Những Gì Đã Hoàn Thành)
Dự án hiện tại đã xây dựng thành công bộ khung cơ bản và hoàn thiện logic trò chơi đối kháng 1-vs-1:

### 1. Phía Máy chủ (Server)
* ✅ **TCP Socket Listener:** Lắng nghe và chấp nhận kết nối tại cổng `1234` (`127.0.0.1`).
* ✅ **Multi-threading:** Mở luồng (`Thread`) độc lập cho mỗi Client kết nối để xử lý dữ liệu song song.
* ✅ **Auto Matchmaking (Ghép cặp):** Chờ đủ 2 người chơi kết nối mới bắt đầu cho phép gửi/nhận dữ liệu.
* ✅ **Role Assignment (Phân vai tự động):** Máy chủ tự động chỉ định người vào trước là `X` (được đi trước), người vào sau là `O` (đi sau) và thông báo về cho Client.
* ✅ **Data Routing:** Chuyển tiếp chính xác tọa độ nước đi từ người chơi này sang màn hình người chơi kia.
* ✅ **Server Log:** Giao diện trực quan hiển thị trạng thái kết nối và nhật ký hệ thống.

### 2. Phía Người chơi (Client)
* ✅ **Giao diện hiện đại (Flat UI):** Tự động sinh bàn cờ 15x15 bằng code với thiết kế phẳng. Tích hợp thanh trạng thái (Status Bar) chỉ báo màu sắc theo vai trò (X: Đỏ, O: Xanh).
* ✅ **Thuật toán cốt lõi (Game Logic):** Viết thuật toán duyệt ma trận 4 hướng (Ngang, Dọc, Chéo xuôi, Chéo ngược) để tự động phát hiện 5 quân cờ liên tiếp và phân định Thắng/Thua.

---

## 🛠️ Hướng Dẫn Khởi Chạy (Testing)
Hệ thống hiện tại yêu cầu chạy 1 Server và 2 Client song song. Thực hiện các bước sau trong Visual Studio:

1.  Mở file `UDM_17.sln`.
2.  **Bật Server:** Nhấp chuột phải vào project `UDM_17_Server` -> Chọn **Debug** -> **Start Without Debugging**. Bấm nút *"Bật Server UDM_17"*.
3.  **Bật Client 1:** Nhấp chuột phải vào project `UDM_17_Client` -> Chọn **Debug** -> **Start Without Debugging**. Bấm nút *"Kết nối Server"*.
4.  **Bật Client 2:** Lặp lại bước 3 để mở thêm cửa sổ người chơi thứ hai. Bấm nút *"Kết nối Server"*.
5.  Giao diện 2 Client sẽ tự động cập nhật ai là X, ai là O. Trò chơi bắt đầu!

---
