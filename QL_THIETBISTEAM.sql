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
	NgaySinh DATE,
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
	DiaChi NVARCHAR(50),
	SDT NVARCHAR(50),
	Email NVARCHAR(50),
	NgaySinh DATE,
	GioiTinh NVARCHAR(10),
	NgayTao datetime,
	TenDN NVARCHAR(50),
	MatKhau NVARCHAR(30),
	CONSTRAINT PK_KhachHang PRIMARY KEY CLUSTERED  (MaKH  ASC )
)

--ALTER TABLE KHACHHANG
--ALTER COLUMN NgaySinh DATE



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


CREATE TABLE NHACUNGCAP(
	MaNCC nchar(10) not null,
	TenNCC NVARCHAR(100),
	DiaChi NVARCHAR(250),
	DienThoai INT,
	constraint PK_NHACUNGCAP primary key (MaNCC) 
)
	


CREATE TABLE THONGTINSANPHAM
(
	MaSanPham INT  IDENTITY(1,1) NOT NULL,
	MaLoai  INT,
	MaNCC nchar(10) not null,
	TenSanPham NVARCHAR(100),
	GiaSanPham FLOAT,
	MoTa NVARCHAR(800),
	HinhAnh  NVARCHAR(200),
	GiamGia FLOAT,
	SLTon INT,
	CONSTRAINT PK_THONGTINSACH PRIMARY KEY CLUSTERED  (MaSanPham  ASC ),
	CONSTRAINT FK_THONGTINSACH_LOAISANPHAM FOREIGN KEY (MaLoai) REFERENCES LOAISANPHAM (MaLoai),
	CONSTRAINT FK_THONGTINSACH_NHACUNGCAP FOREIGN KEY (MaNCC) REFERENCES NHACUNGCAP(MaNCC)
)

select ISNULL(COUNT(MaLoai), 0 )
from THONGTINSANPHAM
where MaLoai =3

CREATE TABLE PHIEUNHAPHANG
(
	MaPhieuNhapHang INT IDENTITY(1,1) NOT NULL,
	MaNCC nchar(10),
	MaNV INT ,
	NgayLap_PN date,
	TongSL int,
	TongTien_NH FLOAT,
	constraint PK_PHIEUNHAPHANG primary key (MaPhieuNhapHang),
    constraint FK_PHIEUNHAPHANG_NHANVIEN foreign key(MaNV) references NHANVIEN(MaNV),
    constraint FK_PHIEUNHAPHANG_NHACUNGCAP foreign key(MaNCC) references NHACUNGCAP(MaNCC)
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
	--TinhTrang BIT,
	TinhTrang INT,
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
	DiaChi NVARCHAR(50),
	SDT NVARCHAR(50),
	NgayTao datetime,
	CONSTRAINT PK_PHIEUGIAOHANG PRIMARY KEY CLUSTERED  (MaGH  ASC),
    constraint FK_PHIEUGIAOHANG_PDH foreign key(MaPhieuDH) references PHIEUDATHANG(MaPhieuDH)
)
---------------NHAP CƠ SỞ DỮ LIỆU
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
INSERT INTO NhanVien VALUES(N'Do Gia Huy','23/7/2001',N'Nam',N'giahuydo@gmail.com','0356322754',N'NV1.JPG','GIABO','12345',2)
INSERT INTO NhanVien VALUES(N'Nguyen Thanh Loc','01/5/2001',N'Nam',N'locdaubuoi@gmail.com','0355467282',N'NV2.JPG','THANHLOC','12345',2)
INSERT INTO NhanVien VALUES(N'Le Xuan Huy','12/8/2001',N'Nam',N'huyle@gmail.com','0355467282',N'NV2.JPG','XUANHUY','12345',1)
INSERT INTO NhanVien VALUES(N'Admin','01/01/2001',N'Nam',N'admin@gmail.com',0355467282,N'NV2.JPG','admin','12345',1)
INSERT INTO NhanVien VALUES(N'Bùi Văn Khoa','02/08/2001',N'Nam',N'buivankhoa@gmail.com','0355923282',N'NV2.JPG','khoabui','12345',2)


--************BẢNG KHÁCH HÀNG
SET DATEFORMAT DMY
INSERT INTO KHACHHANG VALUES(N'Đỗ Gia Huy',N'TP.HCM','0763512628',N'giahuydo01@gmail.com',N'25/1/2001',N'Nam',N'13/10/2020','giahuydo','12345')
INSERT INTO KHACHHANG VALUES(N'Nguyễn Thành Lộc',N'TP.HCM','0987654321',N'locdaubuoi@gmail.com',N'25/12/2001',N'Nam',N'12/10/2020','loc','12345')
INSERT INTO KHACHHANG VALUES(N'Nguyễn Thành Lộc',N'TP.HCM','0987654321',N'gapdaudomdo01@gmail.com',N'25/12/2001',N'Nam',N'12/10/2020','locu','12345')


INSERT INTO KHACHHANG VALUES(N'Nguyễn Thành Lộc',N'TP.HCM','0987654321',N'conca8048@gmail.com',N'25/12/2001',N'Nam',N'12/10/2020','locumap','12345')
select * from KHACHHANG



--*****************BANG LIEN HE
SET DATEFORMAT DMY
INSERT LIENHE VALUES (N'a', N'a', N'a@gmail.com', N'123456', N'đây là nội dung thứ 2', CAST(N'2016-07-01 00:00:00' AS SmallDateTime))
INSERT LIENHE VALUES (N'b', N'b', N'test@gmail.com', N'1', N'đây là nội dung thứ 3', CAST(N'2016-07-01 00:00:00' AS SmallDateTime))
select * from LIENHE



--************BẢNG LOẠI SẢN PHÂM
INSERT LOAISANPHAM(TenLoai) VALUES ( N'Steam Cấp Mầm Non')
INSERT LOAISANPHAM(TenLoai) VALUES ( N'Steam Cấp 1')
INSERT LOAISANPHAM(TenLoai) VALUES ( N'Steam Cấp 2')
INSERT LOAISANPHAM(TenLoai) VALUES ( N'Steam Cấp 3')

select * from LOAISANPHAM


--------------------them du lieu bang NHACUNGCAP
INSERT INTO NhaCungCap
VALUES('NCC01', N'Nhà cung cấp NEW BRAIN QUẬN 3', N'034 Trường Sa, phường 12, TP.HCM',0123684273);
INSERT INTO NhaCungCap
VALUES	('NCC02',N'Nhà cung cấp NEW BRAIN Quận Tân Bình ',N'Số 13, Bàu Cát 6, Phường 14, TP.HCM',0283810958);
INSERT INTO NhaCungCap
VALUES	('NCC03',N'Nhà cung cấp NEW BRAIN Quận 7 ',N'Vietopia, 02-04 Đường số 9, Tân Hưng, TP.HCM',0924419885);
INSERT INTO NhaCungCap
VALUES	('NCC04',N'Nhà cung cấp NEW BRAIN Quận Nam Từ Liêm',N'Số 8, B9, KĐT Mỹ Đình 1, Mỹ Đình, Hà Nội',0823481910);
INSERT INTO NhaCungCap
VALUES	('NCC05',N'Nhà cung cấp NEW BRAIN Quận Hà Đông',N'Số nhà 1162, đường Quang Trung, Phường Yên Nghĩa, Hà Nội.',0232391209);
INSERT INTO NhaCungCap
VALUES	('NCC06',N'Nhà cung cấp NEW BRAIN Quận Nam Từ Liêm',N'Số 8, B9, KĐT Mỹ Đình 1, Mỹ Đình, Hà Nội',0983931001);
INSERT INTO NhaCungCap
VALUES('NCC07',N'Nhà cung cấp NEW BRAIN Đà Nẵng',N'Số 73, Phó Đức Chính, Mân Thái, Sơn Trà, Đà Nẵng',0899633869);

select*from NHACUNGCAP


--*************BẢNG CHI TIẾT SẢN PHẨM---
select* from THONGTINSANPHAM
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC01', N'Khay Tam Giác Xếp Hình mở rộng tư duy sáng tạo ở bé', 150000, N'Nhiệm vụ của người chơi là sắp xếp các miếng ghép sao cho vừa vào khay tam giác xếp hình.Khay Tam Giác Xếp Hình mở rộng tư duy sáng tạo có đính kèm sẵn 10 thẻ công việc khác nhau cho trẻ. Mỗi thẻ có hai mặt và khay làm việc có nắp.Tất cả chi tiết được mài dũa tỉ mỉ, không góc nhọn an toàn tuyệt đối cho trẻ. Chất lượng cao.Khay Tam Giác Xếp Hình phù hợp cho trẻ mầm non mở rộng tư duy sáng tạo, trẻ mẫu giáo có độ tuổi trên 3+.', N'product8.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (2,'NCC02', N'Bộ Kiếng Đo Hình Chiếu Không Gian dạy toán hình học', 250000, N'Bộ Kiếng Đo Hình Chiếu Không Gian gồm một tấm kiếng mica màu đỏ trong loại tốt và một miếng nhựa xanh hỗ trợ. Kiếng Đo có kèm hướng dẫn hỗ trợ sử dụng chi tiết. Khi gắn tấm nhựa xanh vào, miếng mica hoạt động như một tấm kiếng thật, có khả năng phản chiếu vật thể. Hỗ trợ trong việc học Toán hình học: Tìm hiểu & phân tích các dạng hình học và khái niệm đồng dạng, tương đồng và đối xứng của các hình đó. Trên kiếng có in các giá trị con số như cây thước để vẽ và đo đạc.', N'product-cap1-1.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (3,'NCC03', N'Chương trình STEAM tiểu học – THCS 80 tiết', 300000, N'Bộ chương trình STEAM tiểu học – THCS là một gói STEAM hoàn chỉnh bao gồm thiết bị, giáo án, sách hướng dẫn. Nội dung trọng tâm xoay quanh chủ đề lớn NĂNG LƯỢNG XANH:Năng lượng mặt trời (20 bài thực hành) ,Năng lượng gió (20 bài thực hành) ,Năng lượng nước (20 bài thực hành) , Ánh sáng và Thấu kính (20 bài thực hành), Chương trình STEAM tiểu học – THCS được xây dựng đầy đủ, hoàn chỉnh, thuộc một trong 4 gói STEAM các cấp được xây dựng có hệ thống từ mầm non đến cấp 3.', N'product-ct80.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (4,'NCC04', N'Chương trình STEAM Tiểu Học-THCS-THPT 100 tiết', 350000, N'Chương trình STEAM tiểu học – THCS được xây dựng đầy đủ, hoàn chỉnh, thuộc một trong 4 gói STEAM các cấp được xây dựng có hệ thống từ mầm non đến cấp 3. Với hệ thống miếng ghép đa dạng gồm 100 buổi phục vụ hoạt động giáo dục trẻ tiểu học và THCS. Chương trình thiết kế theo phương pháp giáo dục chủ đạo là STEAM – đây cũng là phương pháp giáo dục tiên tiến đang chiếm ưu thế tại nhiều nước phát triển trên thế giới, giúp trẻ phát triển theo hướng khoa họ', N'product-ct120.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (1,'NCC05', N'Đồng Hồ Tập Xem Giờ Phút cho bé mầm non', 150000, N'Đồng hồ tập xem giờ phút cung cấp cho trẻ những hiểu biết về đặc điểm của đồng hồ và biết được các chức năng của chúng:Số ;   Kim ngắn – kim giờ;   Kim dài – kim phút. Đồng hồ bằng nhựa có kiểu dáng đơn giản và hình tròn dễ thương, thu hút sự chú ý của bé, là sản phẩm trang trí trong ngôi nhà.', N'product9.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (2,'NCC06', N'Bộ Que Tính Học Toán Cộng Trừ phạm vi 1 đến 100', 240000, N'Từ 3 tuổi, trí não bé đang trong giai đoạn bắt đầu phát triển mạnh mẽ, cũng là độ tuổi bé được làm quen với chữ cái và các con số vì vậy đây là thời điểm thích hợp để ba mẹ giúp bé làm quen và học tập. Bộ Que Tính Học Toán Cộng Trừ 100 Số giúp học sinh thực hành cộng, trừ trong phạm vi 10, cộng trừ (không nhớ) trong phạm vi 100. Đây là bộ thiết bị dạy phép cộng, phép trừ tiểu học lớp 1 lớp 2', N'product-cap1-5.png',0,10)
INSERT THONGTINSANPHAM(MaLoai,MaNCC,TenSanPham,GiaSanPham,MoTa,HinhAnh,GiamGia,SLTon) VALUES (2,'NCC07', N'Đồng Hồ Tập Xem Thời Gian hình cú cho bé', 230000, N'Đồng Hồ Tập Xem Thời Gian hình cú giáo dục trẻ biết quý trọng thời gian. Biết thời gian rất cần thiết đối với con người và ý thức học tập. Giáo viên mầm non sử dụng Đồng Hồ Dạy Học Hình Cú để phát triển kỹ năng quan sát, chú ý, ghi nhớ có chủ định và chơi trò chơi của bé.', N'product1.png',0,10)


select*from THONGTINSANPHAM


---------------------them du lieu b?ng HOA DON NHAP SACH VAO CUA HANG
SET DATEFORMAT DMY
INSERT INTO PHIEUNHAPHANG
VALUES ('NCC01',1,N'21/9/2022',4,430000);
SET DATEFORMAT DMY
INSERT INTO PHIEUNHAPHANG
VALUES('NCC02',2,N'21/4/2022',10,300000);


--******************************PHIEU DAT HANG
GO
SET DATEFORMAT DMY
INSERT PHIEUDATHANG(MaKH,NgayDat,Tong_SL_Dat,ThanhTien,TinhTrang) VALUES (1,N'12/3/2022', 1,250000, 3)
INSERT PHIEUDATHANG(MaKH,NgayDat,Tong_SL_Dat,ThanhTien,TinhTrang) VALUES (2,N'10/9/2022', 2,300000,3)


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
--select ISNULL(SUM(ThanhTien), 0 ) 
--from PHIEUDATHANG
--where MONTH(NgayDat)=1 and YEAR(NgayDat)=YEAR(GETDATE()) and TinhTrang=3
--select ISNULL(SUM(CT_PHIEUDATHANG.SoLuong), 0 )
--from CT_PHIEUDATHANG, THONGTINSANPHAM, LOAISANPHAM
--where CT_PHIEUDATHANG.MaSanPham=THONGTINSANPHAM.MaSanPham and THONGTINSANPHAM.MaLoai=LOAISANPHAM.MaLoai and LOAISANPHAM.MaLoai=4

select * from PHIEUGIAOHANG