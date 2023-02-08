DROP DATABASE QL_THIETBISTEAM
create database QL_THIETBISTEAM
go
use QL_THIETBISTEAM

CREATE TABLE PHANQUYEN
(
	ID_PhanQuyen INT NOT NULL,
	TenPQ NVARCHAR(50),
    CONSTRAINT PK_PHANQUYEN PRIMARY KEY (ID_PhanQuyen)
)


CREATE TABLE NHANVIEN
(
	MaNV INT IDENTITY(1,1) NOT NULL,
	TenNV NVARCHAR(50),
	NgaySinh DATETIME,
	GioiTinh NVARCHAR(5),
	Email NVARCHAR(100),
	SoDT CHAR(12),
    HinhAnh NCHAR(100),
	TenDN NVARCHAR(50),
	MatKhau NVARCHAR(30),
	ID_PhanQuyen INT,
    CONSTRAINT PK_NHANVIEN PRIMARY KEY CLUSTERED  (MaNV ASC),
	CONSTRAINT FK_NHANVIEN_PHANQUYEN FOREIGN KEY (ID_PhanQuyen) REFERENCES PHANQUYEN(ID_PhanQuyen)
)

CREATE TABLE KHACHHANG
(
	MaKH INT  IDENTITY(1,1) NOT NULL,
	TenKH NVARCHAR(50),
	DiaChi NVARCHAR(250),
	SDT NVARCHAR(50),
	Email NVARCHAR(50),
	NgaySinh DATETIME,
	GioiTinh NVARCHAR(10),
	NgayTao datetime,
	TenDN NVARCHAR(50),
	MatKhau NVARCHAR(30),
	CONSTRAINT PK_KhachHang PRIMARY KEY CLUSTERED  (MaKH  ASC )
)

CREATE TABLE LIENHE(
	MaLH int IDENTITY(1,1) NOT NULL,
	Ho nvarchar(50) NULL,
	Ten nvarchar(50) NULL,
	Email varchar(100) NULL,
	DienThoai varchar(50) NULL,
	NoiDung nvarchar(500) NULL,
	NgayCapNhat smalldatetime NULL,
	CONSTRAINT PK_LIENHE PRIMARY KEY CLUSTERED  (MaLH ASC )
)

CREATE TABLE LOAISANPHAM
( 
	MaLoai INT IDENTITY(1,1) NOT NULL,
	TenLoai NVARCHAR(50) NULL,
	 CONSTRAINT PK_LoaiSanPhams PRIMARY KEY CLUSTERED  (MaLoai ASC )	
)


CREATE TABLE NHACUNGCAP
(
	MaNCC nchar(10) not null,
	TenNCC NVARCHAR(100),
	DiaChi NVARCHAR(250),
	DienThoai Char(20),
	constraint PK_NHACUNGCAP primary key (MaNCC) 
)

CREATE TABLE THONGTINSANPHAM
(
	MaSanPham INT  IDENTITY(1,1) NOT NULL,
	MaLoai  INT,
	MaNCC nchar(10) not null,
	TenSanPham NVARCHAR(100),
	GiaSanPham FLOAT,
	MoTa NVARCHAR(4000),
	HinhAnh  NVARCHAR(200),
	GiamGia FLOAT,
	SLTon INT,
	CONSTRAINT PK_THONGTINSACH PRIMARY KEY CLUSTERED  (MaSanPham  ASC ),
	CONSTRAINT FK_THONGTINSACH_LOAISANPHAM FOREIGN KEY (MaLoai) REFERENCES LOAISANPHAM (MaLoai),
	CONSTRAINT FK_THONGTINSACH_NHACUNGCAP FOREIGN KEY (MaNCC) REFERENCES NHACUNGCAP(MaNCC)
)

CREATE TABLE DonDatHangNCC
(
	MaDonDatHangNCC INT IDENTITY(1,1) NOT NULL,
	MaNCC nchar(10),
	MaNV INT ,
	NgayLap DATETIME,
	TongSL int,
	TongTien FLOAT,
	TrangThai int,
	constraint PK_DonDatHangNCC primary key (MaDonDatHangNCC),
    constraint FK_DonDatHangNCC_NHANVIEN foreign key(MaNV) references NHANVIEN(MaNV),
    constraint FK_DonDatHangNCC_NHACUNGCAP foreign key(MaNCC) references NHACUNGCAP(MaNCC)
)

CREATE TABLE CT_DonDatHangNCC
(
	MaCT_DonDatHangNCC INT IDENTITY(1,1) NOT NULL,
	MaDonDatHangNCC int not null,
	MaSanPham INT not null,
	Soluong INT,
	DonGiaDat FLOAT,
	TongTien FLOAT,
	constraint PK_CT_DonDatHangNCC  primary key (MaCT_DonDatHangNCC, MaDonDatHangNCC),
    constraint FK_CT_DonDatHangNCC_DDHNCC foreign key (MaDonDatHangNCC) references DonDatHangNCC(MaDonDatHangNCC),
	constraint FK_CT_DonDatHangNCC_TTSP foreign key (MaSanPham) references THONGTINSANPHAM(MaSanPham)
)

CREATE TABLE PHIEUNHAPHANG
(
	MaPhieuNhapHang INT NOT NULL,
	MaNCC nchar(10),
	MaNV INT ,
	NgayLap_PN DATETIME,
	TongSL int,
	TongTien_NH FLOAT,
	constraint PK_PHIEUNHAPHANG primary key (MaPhieuNhapHang),
    constraint FK_PHIEUNHAPHANG_NHANVIEN foreign key(MaNV) references NHANVIEN(MaNV),
    constraint FK_PHIEUNHAPHANG_NHACUNGCAP foreign key(MaNCC) references NHACUNGCAP(MaNCC),
	constraint FK_PHIEUNHAPHANG_DonDatHangNCC foreign key(MaPhieuNhapHang) references DonDatHangNCC(MaDonDatHangNCC)
)

CREATE TABLE CT_PHIEUNHAPHANG
(
	MaCTPhieuNhapHang INT IDENTITY(1,1) NOT NULL,
	MaSanPham INT not null,
	MaPhieuNhapHang int not null,
	Sluong INT,
	DonGiaNhap FLOAT,
	TongTien FLOAT,
	constraint PK_CT_PHIEUNHAPHANG  primary key (MaCTPhieuNhapHang, MaPhieuNhapHang),
    constraint FK_CT_PHIEUNHAPHANG_PNH foreign key (MaPhieuNhapHang) references PHIEUNHAPHANG(MaPhieuNhapHang),
	constraint FK_CT_PHIEUNHAPHANG_TTSP foreign key (MaSanPham) references THONGTINSANPHAM(MaSanPham)
)

CREATE TABLE TINHTRANGDH
(
	TinhTrang INT NOT NULL,
	TenTinhTrang NVARCHAR(50),
    CONSTRAINT PK_TINHTRANGDH PRIMARY KEY (TinhTrang)
)


CREATE TABLE PHIEUDATHANG
(
	MaPhieuDH INT IDENTITY(1,1) NOT NULL,
	MaKH INT,
	NgayDat datetime,
	Tong_SL_Dat INT,
	ThanhTien FLOAT,
	TinhTrang INT,
	PhiShip float,
	CONSTRAINT PK_PHIEUDATHANG PRIMARY KEY CLUSTERED  (MaPhieuDH  ASC),
    constraint FK_PHIEUDATHANG_KH foreign key(MaKH) references KHACHHANG(MaKH),
	CONSTRAINT FK_PHIEUDATHANG_TINHTRANGDH FOREIGN KEY (TinhTrang) REFERENCES TINHTRANGDH(TinhTrang)
)

CREATE TABLE CT_PHIEUDATHANG
(
	MaPhieuDH INT not null,
	MaSanPham INT not null ,
	SoLuong INT,
	DonGia FLOAT,
    constraint PK_CT_PHIEUDATHANG primary key (MaPhieuDH ,MaSanPham),
    constraint FK_CT_PHIEUDH_PDT foreign key(MaPhieuDH) references PHIEUDATHANG(MaPhieuDH),
    constraint FK_CT_PHIEUDH_TTSP foreign key(MaSanPham) references THONGTINSANPHAM(MaSanPham)
)

CREATE TABLE PHIEUGIAOHANG
(
	MaGH INT  IDENTITY(1,1) NOT NULL,
	MaPhieuDH INT NOT NULL,
	TenKH NVARCHAR(50),
	Email NVARCHAR(50),
	DiaChi NVARCHAR(250),
	SDT NVARCHAR(50),
	NgayTao datetime,
	CONSTRAINT PK_PHIEUGIAOHANG PRIMARY KEY CLUSTERED  (MaGH  ASC),
    constraint FK_PHIEUGIAOHANG_PDH foreign key(MaPhieuDH) references PHIEUDATHANG(MaPhieuDH)
)


CREATE TABLE SALE
(
	MASL INT  IDENTITY(1,1) NOT NULL,
	TENSL NVARCHAR(MAX),
	NGAYBATDAU DATETIME,
	NGAYKETTHUC DATETIME
	constraint PK_SL primary key (MASL)
)

CREATE TABLE SPSALE
(
	MASPSALE INT IDENTITY(1,1) NOT NULL,
	MASL INT NOT NULL,
	MaSanPham INT NOT NULL,
	GIAMGIA FLOAT,
	constraint PK_SPSL primary key (MASPSALE),
	constraint FK_SL_SPSL foreign key(MASL) references SALE(MASL),
    constraint  FK_SANPHAM_SPSL foreign key(MaSanPham) references THONGTINSANPHAM(MaSanPham)
)
---------------NHAP CƠ SỞ DỮ LIỆU
--************SALE
set dateformat dmy
Insert into SALE
values(N'tết','22/1/2023','23/2/2023')
--************PHÂN QUYỀN
INSERT INTO PHANQUYEN VALUES(1,'Admin')
INSERT INTO PHANQUYEN VALUES(2,N'Nhân Viên Bán Hàng')
INSERT INTO PHANQUYEN VALUES(3,N'Nhân Viên Nhập Hàng')


--************TÌNH TRẠNG ĐƠN HÀNG
INSERT INTO TINHTRANGDH VALUES(-1,N'Chưa Xác Nhận')
INSERT INTO TINHTRANGDH VALUES(0,N'Xữ lý')
INSERT INTO TINHTRANGDH VALUES(1,N'Đã Đóng Gói')
INSERT INTO TINHTRANGDH VALUES(2,N'Đang Giao')
INSERT INTO TINHTRANGDH VALUES(3,N'Đang Thàng Công')

--************BẢNG NHÂN VIÊN 
SET DATEFORMAT DMY
INSERT INTO NhanVien VALUES(N'Đỗ Gia Huy','23/7/2001',N'Nam',N'giahuydo@gmail.com','0356322754',N'NV1.JPG','GIABO','12345',3)
INSERT INTO NhanVien VALUES(N'Nguyễn Thành Lộc','01/5/2001',N'Nam',N'locdaubuoi@gmail.com','0355467282',N'NV2.JPG','THANHLOC','12345',2)
INSERT INTO NhanVien VALUES(N'Lê Xuân Huy','12/8/2001',N'Nam',N'huyle@gmail.com','0355467282',N'NV2.JPG','XUANHUY','12345',1)
INSERT INTO NhanVien VALUES(N'Admin','01/01/2001',N'Nam',N'admin@gmail.com','0355467282',N'NV2.JPG','admin','12345',1)
INSERT INTO NhanVien VALUES(N'Bùi Văn Khoa','02/08/2001',N'Nam',N'buivankhoa@gmail.com','0355923282',N'NV2.JPG','khoabui','12345',2)



--************BẢNG KHÁCH HÀNG
SET DATEFORMAT DMY
INSERT INTO KHACHHANG VALUES(N'Đỗ Gia Huy',N'Xã Thạnh Mỹ, Huyện Vĩnh Thạnh,Tỉnh Cần Thơ','0763512628',N'giahuydo01@gmail.com',N'25/1/2001',N'Nam',N'13/10/2022','giahuydo','12345')
INSERT INTO KHACHHANG VALUES(N'Nguyễn Thành Long',N'Xã Tân Kim, Huyện Cần Giuộc,Tỉnh Long An','0987654321',N'locdaubuoi@gmail.com',N'25/12/2001',N'Nam',N'10/09/2022','thanhlong','12345')
INSERT INTO KHACHHANG VALUES(N'Nguyễn Thanh Thảo',N'Xã Vĩnh Bình, Huyện Chợ Lách,Tỉnh Bến Tre','0987654321',N'nguyentahnhthao@gmail.com',N'25/12/2001',N'Nữ',N'07/09/2022','thanhthao','12345')
INSERT INTO KHACHHANG VALUES(N'Nguyễn Hoàng Hùng',N'Xã Tân Ân Tây, Huyện Ngọc Hiển,Tỉnh Cà Mau','0987654321',N'nguyenhoanghung@gmail.com',N'25/12/2001',N'Nam',N'12/10/2022','hoanghung','12345')
INSERT INTO KHACHHANG VALUES(N'Lê Xuân Huy',N'Xã Phước Hiệp, Huyện Mỏ Cày Nam,Tỉnh Bến Tre','0987387321',N'lexuanhuytvd@gmail.com',N'01/01/2001',N'Nam',N'01/01/2023','xuanhuy','12345')


--*****************BANG LIEN HE
SET DATEFORMAT DMY
INSERT LIENHE VALUES (N'Nguyễn', N'Anh', N'nguyenanh@gmail.com', N'0378527392', N'Tôi muốn đỗi lại hàng', CAST(N'2022-07-01 00:00:00' AS SmallDateTime))
INSERT LIENHE VALUES (N'Nguyễn', N'Khoa', N'nguyenkhoa@gmail.com', N'0987352784', N'Tôi muốn trả lại hàng', CAST(N'2022-07-02 00:00:00' AS SmallDateTime))



--************BẢNG LOẠI SẢN PHÂM
INSERT LOAISANPHAM(TenLoai) VALUES ( N'Steam Cấp Mầm Non')
INSERT LOAISANPHAM(TenLoai) VALUES ( N'Steam Cấp 1')
INSERT LOAISANPHAM(TenLoai) VALUES ( N'Steam Cấp 2')
INSERT LOAISANPHAM(TenLoai) VALUES ( N'Steam Cấp 3')


--------------------BANG NHACUNGCAP
INSERT INTO NhaCungCap
VALUES('NCC01', N'Nhà cung cấp NEW BRAIN QUẬN 3', N'034 Trường Sa, phường 12, TP.HCM','0123684273');
INSERT INTO NhaCungCap
VALUES	('NCC02',N'Nhà cung cấp NEW BRAIN Quận Tân Bình ',N'Số 13, Bàu Cát 6, Phường 14, TP.HCM','0283810958');
INSERT INTO NhaCungCap
VALUES	('NCC03',N'Nhà cung cấp NEW BRAIN Quận 7 ',N'Vietopia, 02-04 Đường số 9, Tân Hưng, TP.HCM','0924419885');
INSERT INTO NhaCungCap
VALUES	('NCC04',N'Nhà cung cấp NEW BRAIN Quận Nam Từ Liêm',N'Số 8, B9, KĐT Mỹ Đình 1, Mỹ Đình, Hà Nội','0823481910');
INSERT INTO NhaCungCap
VALUES	('NCC05',N'Nhà cung cấp NEW BRAIN Quận Hà Đông',N'Số nhà 1162, đường Quang Trung, Phường Yên Nghĩa, Hà Nội.','0232391209');
INSERT INTO NhaCungCap
VALUES	('NCC06',N'Nhà cung cấp NEW BRAIN Quận Nam Từ Liêm',N'Số 8, B9, KĐT Mỹ Đình 1, Mỹ Đình, Hà Nội','0983931001');
INSERT INTO NhaCungCap
VALUES('NCC07',N'Nhà cung cấp NEW BRAIN Đà Nẵng',N'Số 73, Phó Đức Chính, Mân Thái, Sơn Trà, Đà Nẵng','0899633869');



--*************BẢNG CHI TIẾT SẢN PHẨM---
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC01', N'Khay Tam Giác Xếp Hình mở rộng tư duy sáng tạo ở bé', 150000, N'Nhiệm vụ của người chơi là sắp xếp các miếng ghép sao cho vừa vào khay tam giác xếp hình.Khay Tam Giác Xếp Hình mở rộng tư duy sáng tạo có đính kèm sẵn 10 thẻ công việc khác nhau cho trẻ. Mỗi thẻ có hai mặt và khay làm việc có nắp.Tất cả chi tiết được mài dũa tỉ mỉ, không góc nhọn an toàn tuyệt đối cho trẻ. Chất lượng cao.Khay Tam Giác Xếp Hình phù hợp cho trẻ mầm non mở rộng tư duy sáng tạo, trẻ mẫu giáo có độ tuổi trên 3+.', N'product8.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (2,'NCC02', N'Bộ Kiếng Đo Hình Chiếu Không Gian dạy toán hình học', 250000, N'Bộ Kiếng Đo Hình Chiếu Không Gian gồm một tấm kiếng mica màu đỏ trong loại tốt và một miếng nhựa xanh hỗ trợ. Kiếng Đo có kèm hướng dẫn hỗ trợ sử dụng chi tiết. Khi gắn tấm nhựa xanh vào, miếng mica hoạt động như một tấm kiếng thật, có khả năng phản chiếu vật thể. Hỗ trợ trong việc học Toán hình học: Tìm hiểu & phân tích các dạng hình học và khái niệm đồng dạng, tương đồng và đối xứng của các hình đó. Trên kiếng có in các giá trị con số như cây thước để vẽ và đo đạc.', N'product-cap1-1.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (3,'NCC03', N'Chương trình STEAM tiểu học – THCS 80 tiết', 300000, N'Bộ chương trình STEAM tiểu học – THCS là một gói STEAM hoàn chỉnh bao gồm thiết bị, giáo án, sách hướng dẫn. Nội dung trọng tâm xoay quanh chủ đề lớn NĂNG LƯỢNG XANH:Năng lượng mặt trời (20 bài thực hành) ,Năng lượng gió (20 bài thực hành) ,Năng lượng nước (20 bài thực hành) , Ánh sáng và Thấu kính (20 bài thực hành), Chương trình STEAM tiểu học – THCS được xây dựng đầy đủ, hoàn chỉnh, thuộc một trong 4 gói STEAM các cấp được xây dựng có hệ thống từ mầm non đến cấp 3.', N'product-ct80.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (4,'NCC04', N'Chương trình STEAM Tiểu Học-THCS-THPT 100 tiết', 350000, N'Chương trình STEAM tiểu học – THCS được xây dựng đầy đủ, hoàn chỉnh, thuộc một trong 4 gói STEAM các cấp được xây dựng có hệ thống từ mầm non đến cấp 3. Với hệ thống miếng ghép đa dạng gồm 100 buổi phục vụ hoạt động giáo dục trẻ tiểu học và THCS. Chương trình thiết kế theo phương pháp giáo dục chủ đạo là STEAM – đây cũng là phương pháp giáo dục tiên tiến đang chiếm ưu thế tại nhiều nước phát triển trên thế giới, giúp trẻ phát triển theo hướng khoa họ', N'product-ct120.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC05', N'Đồng Hồ Tập Xem Giờ Phút cho bé mầm non', 150000, N'Đồng hồ tập xem giờ phút cung cấp cho trẻ những hiểu biết về đặc điểm của đồng hồ và biết được các chức năng của chúng:Số ;   Kim ngắn – kim giờ;   Kim dài – kim phút. Đồng hồ bằng nhựa có kiểu dáng đơn giản và hình tròn dễ thương, thu hút sự chú ý của bé, là sản phẩm trang trí trong ngôi nhà.', N'product9.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (2,'NCC06', N'Bộ Que Tính Học Toán Cộng Trừ phạm vi 1 đến 100', 240000, N'Từ 3 tuổi, trí não bé đang trong giai đoạn bắt đầu phát triển mạnh mẽ, cũng là độ tuổi bé được làm quen với chữ cái và các con số vì vậy đây là thời điểm thích hợp để ba mẹ giúp bé làm quen và học tập. Bộ Que Tính Học Toán Cộng Trừ 100 Số giúp học sinh thực hành cộng, trừ trong phạm vi 10, cộng trừ (không nhớ) trong phạm vi 100. Đây là bộ thiết bị dạy phép cộng, phép trừ tiểu học lớp 1 lớp 2', N'product-cap1-5.png',20,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (2,'NCC07', N'Đồng Hồ Tập Xem Thời Gian hình cú cho bé', 230000, N'Đồng Hồ Tập Xem Thời Gian hình cú giáo dục trẻ biết quý trọng thời gian. Biết thời gian rất cần thiết đối với con người và ý thức học tập. Giáo viên mầm non sử dụng Đồng Hồ Dạy Học Hình Cú để phát triển kỹ năng quan sát, chú ý, ghi nhớ có chủ định và chơi trò chơi của bé.', N'product1.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC07', N'Gói Bổ Sung 76 Quả Cân Bằng Nhựa', 490000, N'Sản phẩm bổ sung 76 quả cân nhựa:
																																							16 quả cân nhựa 20g;
																																							20 quả cân nhựa 10g;
																																							20 quả cân nhựa 5g;
																																							20 quả cân nhựa 1g.
																																							Tất cả các cân nhựa được đóng trong hộp nhựa trong suốt có khóa.
																																							Gói Quả Cân Nhựa Bổ Sung 1054P phù hợp cho trẻ 5+ tuổi trở lên.', N'product3.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC07', N'Bộ Đo Thể Tích và Dung Tích cho trẻ mầm non', 480000, N'Thông tin:Bậc: STEAM mầm non ----- Độ tuổi: 3+------- Dạy:Đo dung tích:
																																							Thiết bị Bộ Đo Thể Tích 1044 có 6 mô hình khối thể tích khác nhau (khối vuông, khối nón tròn, khối chóp tứ giác đều, khối chữ nhật, khối lăng trụ và khối cầu).
																																							Các hình khối có thiết kế kín và có một miệng lỗ dưới đáy mỗi khối mô hình, cho phép sử dụng các vật liệu như cát, hạt cườm hoặc chất lỏng (nước) đổ vào, khiến cho lớp thêm sinh động.
																																							Làm quen với các hình khối khác nhau, sâu hơn có thể dạy tới so sánh tỷ lệ thể tích giữa các hình khối.
																																							Bộ Đo Thể Tích Mầm Non #1044 Gigo phù hợp cho trẻ mầm non & mẫu giáo trên 3 tuổi. Chất liệu bằng nhựa cao cấp.
																																							Trẻ biết cách đo thể tích của nhiều đối tượng khác nhau bằng 1 đơn vị đo, biết đếm và đọc kết quả đếm.', N'product4.png',10,10)

INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (2,'NCC07', N'Đồ chơi thông minh Kỹ Sư Cơ Khí STEAM', 960000, N'Với thiết kế sinh động đáng yêu, kích thích trí tư duy, sáng tạo, bộ Đồ chơi thông minh Kỹ Sư Cơ Khí STEAM #1406 thuộc dòng đồ chơi sáng tạo mang tính hướng nghiệp thông minh. Kỹ Sư Cơ Khí STEAM sẽ là món quà không thể bỏ lỡ dành cho các bé trong giai đoạn phát triển tư duy và nhận thức.
																																								<br />
																																								1/ Giới thiệu về bộ Đồ chơi thông minh Kỹ Sư Cơ Khí STEAM #1406:
																																								<br />
																																								Bộ Đồ chơi thông minh Kỹ Sư Cơ Khí STEAM tìm hiểu về các loại máy móc đơn giản, cánh tay đòn, mặt phẳng nghiêng, bánh răng, dầm, bánh xe,…
																																								<br />
																																								Kỹ Sư Cơ Khí STEAM gồm 102 chi tiết miếng ghép, trong đó có một động cơ điện và hộp pin.
																																								<br />
																																								Dòng sản phẩm STEAM của Gigo được thiết kế riêng để mang đến cho trẻ những trải nghiệm thực tế. Các mô hình có thiết kế tập trung vào độ sắc nét và chức năng riêng.
																																								<br />
																																								Bộ Đồ chơi thông minh Kỹ Sư Cơ Khí STEAM bao gồm nhiều mô hình miếng ghép được làm từ chất liệu nhựa an toàn, không độc hại, thân thiện với môi trường và sức khỏe của bé. Các góc cạnh bo tròn kết hợp bề mặt trơn nhẵn, đồ chơi không gây trầy xước hay nguy hiểm cho bé khi vui chơi.
																																								<br />
																																								Việc trẻ chơi và hiểu sẽ thỏa mãn nhu cầu khám phá phát triển tiềm năng của chúng. Những giá trị giáo dục cho trẻ khi chơi bộ đồ chơi Kỹ Sư Cơ Khí STEAM này:
																																								<br />
																																								1. Phát triển ý tưởng;
																																								<br />
																																								2. Phát triển khả năng ngôn ngữ;
																																								<br />
																																								3. Phối hợp nhóm;
																																								<br />
																																								4. Sử dụng trí tưởng tượng.
																																								<br />
																																								Với bộ đồ chơi kỹ sư sửa chữa này bé sẽ tập trung được khả năng phản xạ của mình. Biết cách xây dựng hình ảnh và lắp ráp đồ vật.', N'product-cap1-3.png',0,10)

INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (2,'NCC07', N'Thiết bị dạy học Tháp Học Toán 3D STEAM', 230000, N'Thông tin :
																																						    <br />
																																							Bậc: STEAM cấp 1, STEAM mầm non
																																							<br />
																																							Độ tuổi: 5+
																																							<br />
																																							Dạy:  Toán cộng trừ nhân chia
																																							1/ Giới thiệu Thiết bị dạy học Tháp Học Toán 3D STEAM #1187:
																																							<br />
																																							Thiết bị dạy học Tháp Học Toán 3D STEAM gồm 236 chi tiết, trong đó có 9 cầu thang + 10 tháp + 10 đế cắm + 24 thử thách bằng thẻ nhựa.
																																							<br />
																																							Đây là một trò chơi khá vui, trong đó kết hợp các khối miếng ghép đặc biệt và phép toán.
																																							<br />
																																							Xuất phát từ ý tưởng nghề đưa thư ở địa danh hõm chảo Mafate trên đảo Réunion. Tại đây, người đưa thư phải leo lên xuống nhiều bậc thang để có thể hoàn thành việc gửi thư.
																																							<br />
																																							2/ Thiết bị dạy học Tháp Học Toán 3D STEAM mang tính giáo dục cao:
																																							<br />
																																							Thiết bị dạy học Tháp Học Toán 3D STEAM giúp phát triển tư duy logic và khả năng giải quyết vấn đề.
																																							<br />
																																							Bộ thiết bị phù hợp với cả tự học và học nhóm.
																																							<br />
																																							Các chủ đề học của Tháp Học Toán 3D STEAM #1187 bao gồm:
																																							<br />
																																								1. Hiểu được sự phân tách số lượng lên tới 10 bằng phương pháp Cuisenaire.
																																							<br />
																																								2. Phát triển kỹ năng toán học và lý luận logic.
																																							<br />
																																								3. Thực hiện nguyên tắc thuật toán và chuỗi suy luận.
																																							<br />
																																							Thông tin bổ sung
																																							<br />
																																							TRỌNG LƯỢNG	             1.45 kg
																																							<br />
																																							KÍCH THƯỚC	             37 × 8 × 4 cm', N'product-cap1-4.png',30,10)

INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC06', N'Bảng Dạy Học Mầm Non 72×56 có giá di động', 4500000, N' Bảng Dạy Học Mầm Non di động có chân bánh xe và có khung bao viền cạnh bằng nhựa định hình có kết cấu nhỏ gọn, thanh, tạo cảm giác nhẹ nhàng, kiểu dáng đẹp, phù hợp cho văn phòng nhỏ ,cao cấp, ít người, thông tin vừa phải.
																																								<br />
																																								-- Giới thiệu thiết bị giáo dục Bảng Dạy Học Mầm Non có giá treo di động 
																																								<br />
																																								. Thiết bị Bảng dạy học mầm non được thiết kế đa chức năng hỗ trợ việc dạy học. Bảng có các lỗ xếp dạng lưới để ghim Bảng Chữ Cái và các miếng ghép xếp hình.
																																								<br />
																																								. Bảng dạy học mầm non di động, có chân bánh xe, an toàn và kiên cố.
																																								<br />
																																								. Có sách hướng dẫn sử dụng đi kèm.
																																								<br />
																																								Bảng được làm từ chất liệu nhựa cao cấp. Phù hợp với trẻ mầm non, mẫu giáo. Độ tuổi trẻ trên 3 tuổi.
																																								<br />
																																								 -- Thiết kế của Bảng Dạy Học Mầm Non đa năng:
																																								<br />
																																								. Kích thước khung bảng: Dài 84 cm x Rộng 70 cm.
																																								<br />
																																								. Kích thước sử dụng của bảng: Dài 72 cm x Rộng 56 cm.
																																								<br />
																																								. Bảng Dạy Học Mầm Non đa năng có thể sử dụng chung với Bảng Chữ Cái Tiếng Anh #1401 và 12 Thẻ Hình Học Từ Vựng Tiếng Anh #1402.', N'product10.png',0,10)

INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC06', N'Thùng Xâu Luồn Hạt Nhựa Lớn 106 chi tiết', 1600000, N' Thông tin :
																																								<br />
																																								Bậc: STEAM mầm non
																																								<br />
																																								Độ tuổi: 3+
																																								<br />
																																								Dạy:  Kỹ năng tay & mắt
																																								<br />
																																								-- I/ Giới thiệu thiết bị giáo dục Xâu chuỗi hạt lớn 106 chi tiết nhiều màu 1040:
																																								<br />
																																								. Thùng xâu chuỗi hạt lớn gồm 100 chi tiết hình dạng khác nhau (Hình vuông, hình cầu, hình trụ, hình elip).
																																								<br />
																																								. Các hạt có 6 màu và đường kính lớn hơn 33mm, có thể tránh được tình trạng bé vô ý nuốt phải.
																																								<br />
																																								. Xâu chuỗi hạt lớn dùng dạy phân loại màu sắc, hình dạng, xếp thứ tự, xúc giác và nhiều trò chơi dạy học khác.
																																								<br />
																																								. Chất liệu hạt bằng nhựa tuyệt đối an toàn cho trẻ.
																																								<br />
																																								. Thiết bị giáo dục Xâu chuỗi hạt lớn 106 chi tiết #1040 phù hợp với trẻ 3+.
																																								<br />
																																								II/ Giáo án xâu hạt lớn cho trẻ 25 – 36 tháng tuổi
																																								<br />
																																								. Trẻ biết cầm hạt xâu thành vòng, trẻ xâu được vòng hoàn chỉnh và biết gọi tên sản phẩm của mỉnh.
																																								<br />
																																								1/ Kiến thức và kỹ năng
																																								<br />
																																								. Rèn kỹ năng xâu hạt cho trẻ.
																																								<br />
																																								. Phát triển vận động cho trẻ và rèn sự khéo léo, nhanh nhẹn của các ngón tay.
																																								<br />
																																								. Giáo dục trẻ chơi xong biết cất hạt vào nơi qui định.
																																								<br />
																																								. Rèn luyện đức tính kiên nhẫn cho trẻ.
																																								<br />
																																								. Phát triển ngôn ngữ cho trẻ: Trẻ nói tên sản phẩm của mình là “Chiếc vòng”, hạt vòng màu đỏ, màu xanh.
																																								<br />
																																								2/ Chuẩn bị
																																								<br />
																																								. Mỗi trẻ 1 rổ hạt.
																																								<br />
																																								. Dây xỏ.', N'product11.png',0,10)

INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC06', N'Hộp 31 Bảng Chữ Cái Tiếng Anh bằng nhựa', 990000, N' Thông tin :
																																								<br />
																																								Bậc: STEAM mầm non
																																								<br />
																																								Độ tuổi: 3+
																																								<br />
																																								Dạy:  Ngôn ngữ
																																								<br />
																																								-- Các giáo sư ngôn ngữ và não bộ từ các trường đại học của Anh và Mỹ đã nghiên cứu và chỉ ra rằng: Việc thực hành nói song ngữ có thể được thực hiện từ rất sớm, ngưỡng tối ưu là từ 9 tháng đến 6 tuổi. 
																																								Đây là giai đoạn não bộ trẻ phát triển với tốc độ nhanh chóng, thẩm thấu mọi thông tin xung quanh với tốc độ chóng mặt. 
																																								Học Bảng Chữ Cái Tiếng Anh in thường trong giai đoạn này chính là điều tốt nhất mà bố mẹ nên dành cho các bé.
																																								<br />
																																								1/ Giới thiệu thiết bị dạy học mầm non Bảng Chữ Cái Tiếng Anh 31:
																																								<br />
																																								. Bảng Chữ Cái Tiếng Anh có 31 chữ cái tiếng Anh in thường (2 bộ nguyên âm và 1 bộ phụ âm).
																																								<br />
																																								. Thiết bị mầm non Bảng Chữ Cái Tiếng Anh được làm từ chất liệu nhựa chất lượng cao, đảm bảo cho sự an toàn của bé.
																																								<br />
																																								. Bề mặt Chữ Cái Tiếng Anh nhẵn mịn với các góc cạnh bo tròn không sắc nhọn, không gây tổn thương đến làn da nhạy cảm của bé.
																																								<br />
																																								. Độ tuổi phù hợp: Trẻ trên 3 tuổi.
																																								<br />
																																								2/ Thiết kế Bảng Chữ Cái Tiếng Anh:
																																								<br />
																																								. Bảng chữ in lớn và dài với các chữ cái to cỡ bàn tay của bé, giúp bé nhận biết màu sắc, hình dáng, mặt các chữ cái.
																																								<br />
																																								. Các chữ cái in màu hấp dẫn, vui mắt, bé dễ dàng tháo ra đặt vào.
																																								<br />
																																								. Mặt trước là 31 chữ cái. Mặt sau có móc để gắn lên bảng
																																								<br />
																																								. Bảng Chữ Cái Tiếng Anh #1401 có thể sử dụng chung với Bảng Dạy Học Mầm Non #1177-1 và 12 Thẻ Hình Học Từ Vựng Tiếng Anh.
																																								<br />
																																								. Các miếng ghép chữ cái có kích thước vuông: 80mm x 80mm.', N'product12.png',0,10)


INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC06', N'Bánh Răng Trang Trí Tường 30 mô hình STEAM', 7200000, N' Thông tin :
																																								<br />
																																								Bậc: STEAM mầm non
																																								<br />
																																								Độ tuổi: 3+
																																								<br />
																																								Dạy:  STEAM mầm non
																																								<br />
																																								 1/  Giới thiệu giáo cụ Bánh Răng Trang Trí Tường 30 mô hình STEAM:
																																								<br />
																																								. Thiết bị dạy học mới nhất của Gigo: Bánh Răng Trang Trí Tường 30 mô hình STEAM là sản phẩm thực tiễn 3 chiều để dạy học STEAM. Đặc biệt là các kỹ năng cơ khí,
																																								bé có thể phát triển kỹ năng này qua việc lắp ráp các mô hình trong sách hướng dẫn.
																																								<br />
																																								. Mỗi chủ đề / mô hình lắp ráp có thể chơi trên tường và trên sàn nhà. Trẻ có thể tự chơi hoặc dưới sự hướng dẫn của giáo viên.
																																								<br />
																																								. Thông qua các câu chuyện minh họa, giáo viên có thể được dạy các khái niệm cơ bản như chữ cái, tính toán với các con số, phát triển tính cách nhân vật và các hình ảnh với nội dung khác nhau.
																																								<br />
																																								 2/  Bánh Răng Trang Trí Tường 30 mô hình STEAM mang tính giáo dục cao:
																																								<br />
																																								Các mô hình lắp ráp và trò chơi sẽ hướng dẫn bé làm quen với:
																																								<br />
																																								. Nguyên tắc vật lý đơn giản & ứng dụng khoa học thông qua chơi.
																																								<br />
																																								. Phương pháp lắp ráp sáng tạo và chủ đề bối cảnh mô hình để truyền cảm hứng cho trí tưởng tượng.
																																								<br />
																																								. Miếng ghép có thiết kế tiêu chuẩn, dễ dàng lắp ráp, bao gồm bánh răng, miếng ghép truyền động và hệ thống điều khiển.
																																								<br />
																																								Thông tin bổ sung
																																								<br />
																																								. TRỌNG LƯỢNG	6.10 kg
																																								<br />
																																								. KÍCH THƯỚC	59.3 × 40.6 × 23.1 cm', N'product13.png',0,10)




INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC06', N'8 Bảng Chun Học Toán Hai Mặt bằng dây thun', 660000, N' Thông tin :
																																								<br />
																																								Bậc: STEAM mầm non
																																								<br />
																																								Độ tuổi: 3+
																																								<br />
																																								Dạy:  Hình học
																																								<br />
																																								 1/  Giới thiệu thiết bị giáo dục Bộ 8 Bảng Chun Học Toán Hai Mặt
																																								<br />
																																								. Bảng Chun Học Toán Hai Mặt gồm combo 8 bảng chun hai mặt bằng nhựa màu vàng tươi sáng và 200 sợi dây chun cao su nhiều màu, kèm sách hướng dẫn.
																																								<br />
																																								. Trên cả hai mặt của bảng chun có các mấu (tù đầu)
																																								<br />
																																									. Một mặt dạng lưới 5X5, các mấu (tù đầu) xếp thẳng hàng dọc và ngang để mắc chun;
																																								<br />
																																									. Mặt kia gồm 12 mấu (tù đầu) được xếp theo dạng hình tròn
																																								<br />
																																								. Thiết bị Bảng Chun Học Toán làm từ chất liệu polypropylene bền bỉ đảm bảo tiêu chuẩn an toàn và hợp quy chuẩn.
																																								<br />
																																								. Sản phẩm phù hợp cho trẻ trên 3 tuổi.
																																								<br />
																																								--Mục đích học tập của thiết bị giáo dục Bộ 8 Bảng Chun Học Toán Hai Mặt 1601--
																																								<br />
																																								Bảng Chun Học Toán giúp bé:
																																								<br />
																																								. Học về hình dạng, số, chữ;
																																								<br />
																																								. Giúp khéo léo sử dụng đôi tay;
																																								<br />
																																								. So sánh các điểm tương đồng, sự khác biệt về hình dạng và kích cỡ;
																																								<br />
																																								. Phát triển trí não và khả năng sáng tạo.
																																								<br />
																																								. Qua việc căng chun, tạo hình giúp bé đếm và làm việc đối xứng, đồng dạng, chu vi, diện tích, các phân số và góc.
																																								<br />
																																								. Bảng Chun Học Toán thích hợp trong việc dạy học cũng như cha mẹ cùng chơi với bé, khám phá, so sánh, tạo ra các hình dạng thiết kế hình học.
																																								<br />
																																								Thông tin bổ sung
																																								<br />
																																								TRỌNG LƯỢNG	1.28 kg
																																								<br />
																																								KÍCH THƯỚC	18.5 × 18.5 × 18.5 cm', N'product14.png',0,10)









INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC05', N'Thiết Bị Cân Đo Trọng Lượng mầm non', 1700000, N'Thông tin :
																																								<br />
																																								Bậc: STEAM mầm non
																																								<br />
																																								Độ tuổi: 5+
																																								<br />
																																								Dạy:  Khối lượng, dung tích
																																								1/ Giới thiệu về thiết bị dạy học mầm non Cân trọng lượng dạy học mẫu giáo 1054:
																																								    <br />
																																									. Thiết bị giáo dục Cân trọng lượng dạy học gồm 1 chiếc cân, 2 khay trong suốt có nắp, 11 quả cân kim loại và 14 quả cân nhựa.
																																									<br />
																																									. Cân trọng lượng dạy học dùng cho trẻ học quan sát và nhận thức về độ nghiên, độ nặng của một vật.
																																									<br />
																																									. Hai hộp nhựa lớn gắn 2 bên chiếc cân, một bên chứa nước hoặc các vật dụng, một bên đặt các quả cân, trẻ tự tay thí nghiệm sẽ mang lại các trải nghiệm thú vị.
																																									<br />
																																									. Cân trọng lượng dạy học cho bé mầm non và mẫu giáo, phù hợp trẻ 5+ trở lên.
																																									<br />
																																									. Thông tin bổ sung
																																							    <br />
																																								TRỌNG LƯỢNG	1.59 kg
																																								<br />
																																								KÍCH THƯỚC	46 × 17 × 17 cm', N'product15.png',10,10)



INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC05', N'Đế Gắn Miếng Ghép đa năng – Tường Gigo', 2100000, N'Thông tin :
																																								<br />
																																								Bậc: STEAM mầm non
																																								<br />
																																								Độ tuổi: 3+
																																								<br />
																																								Dạy:  Trang trí
																																								<br />
																																								Xuất phát từ nhu cầu lắp ráp các mô hình lớn hơn, phục vụ nhu cầu thi đấu hay mục đích giới thiệu trang trí nên Đế Gắn Miếng Ghép #T036R đã ra đời.
																																								<br />
																																								1/ Giới thiệu thiết bị Đế Gắn Miếng Ghép đa năng – Tường Gigo 58 chi tiết T036R
																																								<br />
																																								Đóng gói: 6 miếng / bộ.
																																								<br />
																																								Kích thước: 30cm(D) * 20cm(R) * 12cm(C) / miếng.
																																								<br />
																																								Diện tích yêu cầu: Lắp ráp linh hoạt, phù hợp mọi diện tích.
																																								<br />
																																								Vị trí sử dụng: Sản phẩm có thể đính lên tường, đặt dưới đất tùy mục đích sử dụng.
																																								<br />
																																								Chức năng:
																																								<br />
																																									. Dùng làm hoạt động thi đua, quảng cáo, sự kiện, lễ tết, thông báo;
																																									<br />
																																									. Thích hợp cắm với tất cả miếng ghép Gigo;
																																									<br />
																																									. Dễ dàng tháo lắp.
																																									 <br />
																																								2/ Hướng dẫn sử dụng Đế Gắn Miếng Ghép đa năng – Tường Gigo #T036R
																																								<br />
																																									1. Gắn tường hoặc làm đế cắm cho các cuộc thi, hoạt động có sử dụng đến mô hình lớn.
																																								<br />
																																									2. Kết hợp sáng tạo cùng với các miếng ghép Gigo ở các sản phẩm bất kỳ.
																																								<br />
																																									3. Sử dụng kết hợp với bộ Bánh Răng Trang Trí Tường 30 mô hình:
																																							    <br />
																																								Thông tin bổ sung
																																								<br />
																																								TRỌNG LƯỢNG	2.75 kg
																																								<br />
																																								KÍCH THƯỚC	23.3 × 20.2 × 33.6 cm', N'product16.png',20,10)



INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC05', N'240 Miếng Nhựa Xếp Hình Khối Không Gian 3D', 3600000, N'Thông tin :
																																								<br />
																																								Bậc: STEAM mầm non
																																								<br />
																																								Độ tuổi: 4+
																																								<br />
																																								Dạy:  Không gian
																																								<br />
																																								1/ Giới thiệu về thiết bị mầm non Bộ Xếp Hình Khối 3D 240 miếng ghép nhiều màu #1216:
																																								<br />
																																								    . Gồm 240 miếng X GEO trong mờ và sách hướng dẫn in màu 16 trang.
																																									<br />
																																									. Mục đích phát triển kỹ năng học tập của trẻ đối với hệ thống kiến thức về kết cấu.
																																									<br />
																																									. X GEO là một hệ thống xây dựng hình học với 7 loại cơ bản dễ dàng tháo rời nhờ bản lề.
																																									<br />
																																									. Mỗi miếng X GEO có một lỗ ở trung tâm để dễ dàng nhận dạng, xử lý.
																																									<br />
																																									. Lắp ráp cùng lúc các mô hình lăng kính, hình chóp, khối đa diện đều và khối Archimedean (trừ mô hình A) để dễ quan sát, so sánh và thảo luận trong lớp.
																																									<br />
																																									. Độ tuổi phù hợp: Trẻ trên 4 tuổi.
																																							    <br />
																																								Thông tin bổ sung
																																								<br />
																																								TRỌNG LƯỢNG	3.91 kg
																																								<br />
																																								KÍCH THƯỚC	37 × 23 × 16 cm', N'product17.png',0,10)


CREATE PROC Update_SL_Ton
	@MaCTPhieuNhapHang int,
    @MaSP int,
	@MaPhieuNhapHang int
AS
    update THONGTINSANPHAM
	set SLTon = (select CT_PHIEUNHAPHANG.Sluong from CT_PHIEUNHAPHANG where CT_PHIEUNHAPHANG.MaCTPhieuNhapHang= @MaCTPhieuNhapHang and CT_PHIEUNHAPHANG.MaSanPham=@MaSP and CT_PHIEUNHAPHANG.MaPhieuNhapHang=@MaPhieuNhapHang) + SLTon
	where MaSanPham=@MaSP
GO


CREATE PROC Update_TongSL_PN
		@MaPhieuNH nchar(10)
AS
	update PHIEUNHAPHANG
	set TongSL=(select COUNT(MaPhieuNhapHang) 
				from CT_PHIEUNHAPHANG 
				where CT_PHIEUNHAPHANG.MaPhieuNhapHang=@MaPhieuNH
				group by MaPhieuNhapHang )
	where PHIEUNHAPHANG.MaPhieuNhapHang=@MaPhieuNH
GO

CREATE PROC Update_TongTien_PN
		@MaPhieuNH nchar(10)
AS
	update PHIEUNHAPHANG
	set TongTien_NH=(select SUM(CT_PHIEUNHAPHANG.TongTien)
				from CT_PHIEUNHAPHANG 
				where CT_PHIEUNHAPHANG.MaPhieuNhapHang=@MaPhieuNH)
	where PHIEUNHAPHANG.MaPhieuNhapHang=@MaPhieuNH
GO

CREATE PROC Update_TongSL_DatHangNCC
		@MaDonDHNCC nchar(10)
AS
update DonDatHangNCC
	set TongSL=(select COUNT(MaDonDatHangNCC) 
				from CT_DonDatHangNCC 
				where CT_DonDatHangNCC.MaDonDatHangNCC=@MaDonDHNCC
				group by MaDonDatHangNCC )
	where DonDatHangNCC.MaDonDatHangNCC=@MaDonDHNCC
GO

CREATE PROC Update_TongTien_DatHangNCC
		@MaDonDHNCC nchar(10)
AS
update DonDatHangNCC
	set TongTien=(select SUM(CT_DonDatHangNCC.TongTien)
				from CT_DonDatHangNCC 
				where CT_DonDatHangNCC.MaDonDatHangNCC=@MaDonDHNCC)
	where DonDatHangNCC.MaDonDatHangNCC=@MaDonDHNCC
GO

CREATE PROC Update_TrangThai_DatHangNCC
		@MaDonDHNCC int
AS
	update DonDatHangNCC
	set TrangThai = 0
	where MaDonDatHangNCC = @MaDonDHNCC
<<<<<<< HEAD
select * from CT_PHIEUDATHANG,SPSALE where CT_PHIEUDATHANG.MaSanPham = SPSALE.MaSanPham
SELECT * FROM PHIEUDATHANG,THONGTINSANPHAM where CT_PHIEUDATHANG.MaSanPham = THONGTINSANPHAM.MaSanPham
<<<<<<< Updated upstream
select * from SPSALE
=======
select * from CT_PHIEUDATHANG
>>>>>>> Stashed changes
delete SPSALE where MaSanPham = 2
ALTER TABLE THONGTINSANPHAM
ALTER COLUMN GIAMGIA int




=======
<<<<<<< Updated upstream
GO
>>>>>>> e9551886938feb1d3caa54bb8fdc57b7ddf2fe52
=======

>>>>>>> Stashed changes
