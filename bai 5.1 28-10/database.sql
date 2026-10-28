-- KỊCH BẢN TẠO DATABASE VÀ DỮ LIỆU ĐĂNG NHẬP
CREATE DATABASE LoginDB;
GO

USE LoginDB;
GO

CREATE TABLE TaiKhoan (
    Id INT IDENTITY(1, 1) PRIMARY KEY,
    TenDangNhap VARCHAR(50) NOT NULL,
    MatKhau VARCHAR(50) NOT NULL,
    HoTen NVARCHAR(100) NULL
);
GO

-- Chèn dữ liệu mẫu để đăng nhập
INSERT INTO TaiKhoan (TenDangNhap, MatKhau, HoTen)
VALUES 
('admin', '123123', N'Nguyễn Văn A'),
('user1', '123', N'Nguyễn Văn B');
GO