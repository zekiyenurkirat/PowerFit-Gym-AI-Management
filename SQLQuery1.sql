-- Önce bu kullanıcıya bağlı Rol kayıtlarını silelim (Güvenlik için)
DELETE FROM AspNetUserRoles 
WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE Email = 'admin@spor.com');

-- Varsa bu kişinin Üye tablosundaki kaydını silelim (Randevuları varsa hata verebilir)
DELETE FROM Uyeler 
WHERE IdentityUserId IN (SELECT Id FROM AspNetUsers WHERE Email = 'admin@spor.com');

-- En son kullanıcıyı silelim
DELETE FROM AspNetUsers 
WHERE Email = 'admin@spor.com';
