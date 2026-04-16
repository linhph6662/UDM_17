# UDM_17 - Game Co Caro Online (Client/Server)

Mon Lap Trinh Mang, xay dung game co Caro 1v1 su dung mo hinh Client/Server voi TCP Socket, giao dien Windows Forms va luu tru SQLite.

## 1. Tong quan

- Kien truc: 3 project chinh
	- UDM_17.Server: quan ly ket noi, xac thuc, phong choi, luat tran dau, luu lich su.
	- UDM_17.Client: dang nhap, lobby, tao/vao phong, quick match, choi game va chat trong tran.
	- UDM_17.Core: dinh nghia packet, command va payload dung chung.
- Giao thuc: TCP + JSON, moi goi tin la 1 dong (newline-delimited JSON).
- Co so du lieu: SQLite (file udm17.db duoc tao cung thu muc chay server).
- Muc tieu: cung cap he thong choi Caro online 1v1, co dang nhap, bang xep hang co ban, log server, lich su tran.

## 2. Tinh nang da hoan thien

### 2.1 Server

- Lang nghe TCP cong 1234 tren tat ca interface (IPAddress.Any).
- Xu ly nhieu client dong thoi bang task nen.
- Dang ky, dang nhap tai khoan.
- Quick match (tu dong ghep cap 2 nguoi dang cho).
- Tao phong/vao phong theo RoomId.
- Dong bo tran dau:
	- Phan vai X/O.
	- Kiem tra nuoc di hop le.
	- Kiem tra thang/thua/hoa.
	- Chuyen tiep nuoc di va chat cho 2 ben.
- Sau khi ket thuc tran:
	- Cong diem nguoi thang (+10).
	- Luu MatchHistory.
	- Chuyen sang trang thai cho tran moi 15s.
	- Tu dong bat dau lai tran moi neu ca 2 nguoi van con trong phong.
- Dashboard server:
	- Tong tai khoan, so nguoi online.
	- Danh sach user.
	- Danh sach tran gan day.
	- Server log va loc log theo ngay.

### 2.2 Client

- Tu dong thu ket noi server toi da 5 lan khi mo lobby.
- Dang ky va dang nhap tai khoan.
- Lobby:
	- Quick match.
	- Tao phong.
	- Vao phong bang RoomId.
	- Danh sach phong dang mo.
- Form game:
	- Ban co 15x15.
	- Danh dau X/O, hien trang thai den luot.
	- Timer 30 giay moi luot.
	- Chat trong tran.
	- Nhan thong bao ket thuc va dem nguoc cho tran moi.
- Xem profile va ranking (hien tai la du lieu hien thi co ban/o mau).

## 3. Cau truc thu muc

```
UDM_17/
|-- ReadMe.md
`-- Code/
		|-- UDM_17.Core/
		|-- UDM_17.Server/
		`-- UDM_17.Client/
```

## 4. Cong nghe su dung

- C# / .NET 10
- Windows Forms
- TCP Socket
- SQLite (Microsoft.Data.Sqlite)
- Newtonsoft.Json

## 5. Yeu cau moi truong

- Windows 10/11
- .NET SDK 10.0 tro len
- Visual Studio 2022 hoac VS Code co C# extension

## 6. Huong dan chay du an

README nay uu tien huong dan chay ban dong goi da de san trong thu muc Extra/package.

### Cach 1 (khuyen nghi): Chay ban dong goi trong Extra

Duong dan:

- Extra/package/UDM_17.Server/
- Extra/package/UDM_17.Client/

Buoc chay:

1. Mo Extra/package/UDM_17.Server/ va chay file UDM_17.Server.exe.
2. Doi server khoi dong xong (trang thai dang chay, cong 1234).
3. Mo Extra/package/UDM_17.Client/ va chay file UDM_17.Client.exe.
4. Neu muon test 2 nguoi choi, mo them 1 instance client nua (chay lai UDM_17.Client.exe).
5. Dang nhap hoac dang ky, sau do Quick Match hoac tao/vao phong.

Luu y:

- Neu bi Windows Firewall hoi quyen, chon Allow access cho server.
- Server luu database udm17.db ngay trong thu muc server package khi chay.
- Ban co the dung file zip trong Extra/package (UDM_17.Server-win-x64.zip va UDM_17.Client-win-x64.zip) de copy sang may khac roi giai nen va chay exe tuong tu.

### Cach 2: Chay tu source

Thuc hien trong thu muc Code:

```powershell
cd .\Code
```

1. Chay server:

```powershell
dotnet run --project .\UDM_17.Server\UDM_17.Server.csproj
```

2. Chay client:

```powershell
dotnet run --project .\UDM_17.Client\UDM_17.Client.csproj
```

3. Neu can debug bang Visual Studio, mo solution UDM_17.slnx roi chay UDM_17.Server truoc, UDM_17.Client sau.

## 7. Xoa sach du lieu (reset hoan toan)

Neu ban muon xoa hoan toan lich su dau, chat, user va diem, lam nhu sau:

1. Tat tat ca UDM_17.Server.exe va UDM_17.Client.exe.
2. Xoa file udm17.db trong thu muc ban dang chay server.
	- Neu chay ban dong goi: xoa file trong Extra/package/UDM_17.Server/.
	- Neu chay tu source: xoa file trong thu muc output cua UDM_17.Server (bin/.../net10.0-windows/).
3. Chay lai server.
4. He thong se tu dong tao lai 2 tai khoan mau neu DB rong.

## 7.1 Tai khoan mau mac dinh

- Username: admin | Password: admin
- Username: guest | Password: guest

## 8. Giao thuc packet

Packet co dang:

```json
{
	"Cmd": "LOGIN",
	"Data": "{...json payload...}",
	"Sender": "username"
}
```

Mot so command chinh:

- REGISTER, LOGIN, AUTH_OK, AUTH_FAIL
- QUICK_MATCH, CREATE_ROOM, JOIN_ROOM, ROOM_CREATED, ROOM_LIST, MATCH_FOUND
- MOVE, CHAT, GAME_END, GAME_END_WAIT, OPPONENT_LEFT, LEAVE_ROOM, ERROR

## 9. Co so du lieu

File DB: udm17.db (tao tai thu muc runtime cua server).

Bang du lieu:

- Users: thong tin tai khoan, diem, avatar.
- ServerLogs: log he thong server.
- MatchHistory: lich su tran dau.
- ChatHistory: lich su chat trong phong.

## 10. Luong hoat dong co ban

1. Server khoi dong va lang nghe cong 1234.
2. Client ket noi server.
3. User dang nhap/dang ky.
4. User vao quick match hoac phong choi.
5. Server ghep cap va gui MATCH_FOUND.
6. Hai ben danh MOVE/CHAT.
7. Server ket luan GAME_END, cap nhat diem, luu lich su.
8. He thong cho 15s va tu dong bat dau tran moi neu du 2 nguoi.

## 11. Han che hien tai

- Mat khau dang luu plain text (chua hash/salt).
- Chua co server authority cho timeout client (timer tren client chu yeu de UI).
- Ranking/Profile hien tai con mot phan du lieu mau tren giao dien client.
- Chua co test tu dong (unit/integration).

## 12. Huong phat trien tiep

- Bam mat khau bang bcrypt/argon2.
- Hoan thien profile/ranking doc truc tiep tu server.
- Them reconnection va xu ly mat ket noi tot hon.
- Them anti-cheat va dong bo timer o server.
- Them test cho game rules va packet handling.


## 13. Ban quyen

Du an phuc vu hoc tap/noi bo cho mon Lap Trinh Mang.
