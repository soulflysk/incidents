# ระบบจัดการเหตุการณ์ (Incident Management System)

ระบบจัดการเหตุการณ์สำหรับธนาคารแห่งประเทศไทย (ก.ล.ต.) พัฒนาด้วย .NET 8.0 และ Modern Web Technologies

## คุณสมบัติหลัก

### 📋 การบันทึกเหตุการณ์
- บันทึกข้อมูลผู้แจ้ง (ภายใน/ภายนอก ก.ล.ต.)
- รองรับการค้นหาพนักงานอัตโนมัติจากรหัสพนักงาน
- บันทึกประเภทบริการและรายละเอียดปัญหา
- กำหนดระดับความสำคัญของเหตุการณ์

### 👥 การมอบหมายงาน
- มอบหมายเหตุการณ์ให้เจ้าหน้าที่ 1st Level Support
- ส่งต่อเหตุการณ์ให้เจ้าหน้าที่ 2nd Level Support
- บันทึกประวัติการมอบหมายและเวลาที่ดำเนินการ

### 🔧 การบันทึกการแก้ไข
- บันทึกรายละเอียดการแก้ไขปัญหา
- บันทึก Component ที่เกิดปัญหา (Server/Hardware/Software)
- คำนวณและบันทึกเวลาที่ใช้ในการแก้ไข
- ติดตามสถานะการดำเนินงาน

### 📊 การรายงานและติดตาม
- ค้นหาและกรองเหตุการณ์ตามเงื่อนไขต่างๆ
- ดูรายละเอียดเหตุการณ์แบบละเอียด
- ติดตามสถานะและความคืบหน้า

## สถาปัตยกรรมระบบ

### Backend (.NET 8.0)
- **ASP.NET Core MVC** - Web Framework
- **Entity Framework Core** - ORM สำหรับจัดการฐานข้อมูล
- **SQL Server** - ฐานข้อมูลหลัก
- **JWT Authentication** - ระบบยืนยันตัวตน
- **Swagger/OpenAPI** - API Documentation

### Frontend
- **HTML5/CSS3/JavaScript** - Modern Web Technologies
- **Bootstrap 5** - Responsive UI Framework
- **Bootstrap Icons** - Icon Library
- **Responsive Design** - รองรับทุกขนาดหน้าจอ

## การติดตั้งและการใช้งาน

### ความต้องการระบบ
- .NET 8.0 SDK
- SQL Server 2019+ หรือ SQL Server Express
- Visual Studio 2022 หรือ VS Code

### การติดตั้ง

1. **Clone Repository**
   ```bash
   git clone <repository-url>
   cd incidents
   ```

2. **ตั้งค่าฐานข้อมูล**
   - สร้างฐานข้อมูลใน SQL Server
   - รันสคริปต์ `database_schema.sql`
   - อัปเดต connection string ใน `appsettings.json`

3. **ติดตั้ง Dependencies**
   ```bash
   dotnet restore
   ```

4. **รันแอปพลิเคชัน**
   ```bash
   # ใช้ batch file ที่เตรียมไว้
   build-and-run.bat
   
   # หรือรันด้วยคำสั่ง dotnet
   dotnet run
   ```

5. **เข้าใช้งาน**
   - เปิด browser ที่ `https://localhost:7001`
   - หรือ `http://localhost:5000` (ถ้าไม่ใช้ HTTPS)

### การใช้งาน
1. **เข้าสู่ระบบ** - ใช้ชื่อผู้ใช้และรหัสผ่านที่กำหนด
2. **บันทึกเหตุการณ์ใหม่** - คลิก "บันทึกเหตุการณ์ใหม่"
3. **มอบหมายงาน** - เลือกเหตุการณ์และคลิก "มอบหมาย"
4. **บันทึกการแก้ไข** - คลิก "บันทึกการแก้ไข" ในเหตุการณ์ที่ต้องการ

## โครงสร้างฐานข้อมูล

### ตารางหลัก
1. **employees** - ข้อมูลพนักงาน
2. **service_types** - ประเภทบริการ
3. **external_reporters** - ข้อมูลผู้แจ้งภายนอก
4. **incidents** - ข้อมูลเหตุการณ์
5. **incident_assignments** - การมอบหมายงาน
6. **incident_resolutions** - การแก้ไขปัญหา
7. **incident_status_history** - ประวัติการเปลี่ยนสถานะ

### ความสัมพันธ์ระหว่างตาราง
- incidents → employees (ผู้แจ้งภายใน)
- incidents → external_reporters (ผู้แจ้งภายนอก)
- incidents → service_types (ประเภทบริการ)
- incident_assignments → incidents (การมอบหมาย)
- incident_resolutions → incidents (การแก้ไข)

## API Endpoints

### Incidents
- `GET /api/incidents` - ดูรายการเหตุการณ์ทั้งหมด
- `GET /api/incidents/{id}` - ดูรายละเอียดเหตุการณ์
- `POST /api/incidents` - สร้างเหตุการณ์ใหม่
- `PUT /api/incidents/{id}` - แก้ไขเหตุการณ์
- `DELETE /api/incidents/{id}` - ลบเหตุการณ์

### Assignments
- `POST /api/assignments` - มอบหมายเหตุการณ์
- `GET /api/assignments/{incidentId}` - ดูประวัติการมอบหมาย

### Resolutions
- `POST /api/resolutions` - บันทึกการแก้ไขปัญหา
- `GET /api/resolutions/{incidentId}` - ดูรายละเอียดการแก้ไข

## คุณสมบัติพิเศษ

### 🎨 Responsive Design
- รองรับการใช้งานบนอุปกรณ์ทุกขนาด
- ปรับขนาดหน้าจออัตโนมัติตามอุปกรณ์
- รองรับการใช้งานบนมือถือและแท็บเล็ต

### ✅ Data Validation
- ตรวจสอบความถูกต้องของข้อมูลก่อนบันทึก
- แจ้งเตือนเมื่อข้อมูลไม่ครบถ้วน
- ตรวจสอบรูปแบบอีเมลและเบอร์โทรศัพท์

### 💾 Auto-save
- บันทึกข้อมูลอัตโนมัติทุก 30 วินาที
- โหลดข้อมูลฉบับร่างเมื่อเปิดหน้าใหม่
- ป้องกันการสูญหายของข้อมูล

### 🔔 Notifications
- แจ้งเตือนเมื่อดำเนินการสำเร็จ
- แจ้งเตือนเมื่อเกิดข้อผิดพลาด
- แสดงสถานะการดำเนินการแบบ Real-time

## การพัฒนาต่อ

### Authentication & Authorization
- เพิ่มระบบยืนยันตัวตนผู้ใช้
- กำหนดสิทธิ์การใช้งานตามระดับ
- บันทึกประวัติการเข้าใช้งาน

### Advanced Features
- ระบบแจ้งเตือนทางอีเมล
- รายงานสถิติและกราฟ
- การส่งออกข้อมูลเป็น PDF/Excel
- การแนบไฟล์และรูปภาพ
- Dashboard สำหรับผู้บริหาร

### Performance Optimization
- Caching สำหรับข้อมูลที่ใช้บ่อย
- Database indexing สำหรับการค้นหา
- Lazy loading สำหรับข้อมูลขนาดใหญ่

## การแก้ไขปัญหา

### ปัญหาที่พบบ่อย
1. **Build Error** - ลบโฟลเดอร์ `bin` และ `obj` แล้วลอง build ใหม่
2. **Database Connection** - ตรวจสอบ connection string ใน `appsettings.json`
3. **Permission Error** - รันด้วยสิทธิ์ Administrator

### Logging
- ตรวจสอบ log ใน `logs/` folder
- ใช้ Serilog สำหรับการบันทึกข้อมูลระดับ advanced

## เทคโนโลยีที่ใช้

### Backend
- **.NET 8.0** - Framework หลัก
- **ASP.NET Core MVC** - Web Framework
- **Entity Framework Core 8.0** - ORM
- **SQL Server** - Database
- **JWT Bearer** - Authentication
- **Swagger** - API Documentation

### Frontend
- **HTML5** - โครงสร้างหน้าเว็บ
- **CSS3** - การออกแบบและสไตล์
- **JavaScript (ES6+)** - การทำงานแบบ Interactive
- **Bootstrap 5** - Framework สำหรับ Responsive Design
- **Bootstrap Icons** - ไอคอนสำหรับ UI

## การสนับสนุน

สำหรับข้อมูลเพิ่มเติมหรือปัญหาการใช้งาน กรุณาติดต่อ:
- ฝ่ายสนับสนุนด้านเทคนิค
- อีเมล: support@bot.or.th
- โทรศัพท์: 02-283-0000

---

**พัฒนาโดย:** ฝ่ายเทคโนโลยีสารสนเทศ ธนาคารแห่งประเทศไทย  
**เวอร์ชัน:** 2.0.0 (.NET 8.0)  
**ปรับปรุงล่าสุด:** กุมภาพันธ์ 2024
