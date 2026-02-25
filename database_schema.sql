-- Incident Management System Database Schema
-- Designed for ก.ล.ต. (Bank of Thailand) incident tracking

-- ตารางพนักงาน (Employees Table)
CREATE TABLE employees (
    employee_id VARCHAR(20) PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    phone VARCHAR(20),
    department VARCHAR(100),
    position VARCHAR(100),
    support_level ENUM('1st Level', '2nd Level', 'Management') NOT NULL,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- ตารางประเภทบริการ (Service Types Table)
CREATE TABLE service_types (
    service_type_id INT AUTO_INCREMENT PRIMARY KEY,
    service_name VARCHAR(200) NOT NULL UNIQUE,
    description TEXT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ตารางผู้แจ้งภายนอก (External Reporters Table)
CREATE TABLE external_reporters (
    reporter_id INT AUTO_INCREMENT PRIMARY KEY,
    email VARCHAR(150) NOT NULL,
    full_name VARCHAR(200) NOT NULL,
    company_name VARCHAR(200),
    phone VARCHAR(20),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_email (email)
);

-- ตารางเหตุการณ์ (Incidents Table)
CREATE TABLE incidents (
    incident_id INT AUTO_INCREMENT PRIMARY KEY,
    incident_number VARCHAR(20) UNIQUE NOT NULL,
    notification_date DATETIME NOT NULL,
    
    -- ข้อมูลผู้แจ้ง
    reporter_type ENUM('ภายใน ก.ล.ต.', 'ภายนอก ก.ล.ต.') NOT NULL,
    employee_id VARCHAR(20),
    external_reporter_id INT,
    
    -- รายละเอียดเหตุการณ์
    service_type_id INT NOT NULL,
    problem_description TEXT NOT NULL,
    
    -- สถานะ
    status ENUM('รับแจ้ง', 'ดำเนินการ', 'รอดำเนินการ', 'แก้ไขแล้ว', 'ปิดเรื่อง') DEFAULT 'รับแจ้ง',
    priority ENUM('ต่ำ', 'ปานกลาง', 'สูง', 'วิกฤต') DEFAULT 'ปานกลาง',
    
    -- เวลา
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (employee_id) REFERENCES employees(employee_id),
    FOREIGN KEY (external_reporter_id) REFERENCES external_reporters(reporter_id),
    FOREIGN KEY (service_type_id) REFERENCES service_types(service_type_id),
    
    INDEX idx_incident_number (incident_number),
    INDEX idx_notification_date (notification_date),
    INDEX idx_status (status),
    INDEX idx_reporter_type (reporter_type)
);

-- ตารางการมอบหมายงาน (Incident Assignments Table)
CREATE TABLE incident_assignments (
    assignment_id INT AUTO_INCREMENT PRIMARY KEY,
    incident_id INT NOT NULL,
    assigned_to_employee_id VARCHAR(20) NOT NULL,
    assigned_by_employee_id VARCHAR(20) NOT NULL,
    assigned_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    assignment_level ENUM('1st Level', '2nd Level') NOT NULL,
    notes TEXT,
    
    FOREIGN KEY (incident_id) REFERENCES incidents(incident_id) ON DELETE CASCADE,
    FOREIGN KEY (assigned_to_employee_id) REFERENCES employees(employee_id),
    FOREIGN KEY (assigned_by_employee_id) REFERENCES employees(employee_id),
    
    INDEX idx_incident_id (incident_id),
    INDEX idx_assigned_to (assigned_to_employee_id),
    INDEX idx_assigned_at (assigned_at)
);

-- ตารางการแก้ไขปัญหา (Incident Resolutions Table)
CREATE TABLE incident_resolutions (
    resolution_id INT AUTO_INCREMENT PRIMARY KEY,
    incident_id INT NOT NULL,
    resolved_by_employee_id VARCHAR(20) NOT NULL,
    
    -- รายละเอียดการแก้ไข
    component_name VARCHAR(200) NOT NULL, -- ชื่อ Server/Hardware/Software/Application
    resolution_details TEXT NOT NULL,     -- รายละเอียดการแก้ไขปัญหา
    
    -- ช่วงเวลาการแก้ไข
    estimated_start_date DATE,            -- วันที่คาดว่าจะแก้ปัญหา
    estimated_end_date DATE,              -- วันที่คาดว่าจะแก้เสร็จ
    actual_resolution_date DATETIME,      -- วันที่แก้ไขแล้วเสร็จ
    
    -- เวลาที่ใช้
    hours_spent DECIMAL(5,2),             -- จำนวนเวลาที่ใช้แก้ปัญหา (ชั่วโมง)
    
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    FOREIGN KEY (incident_id) REFERENCES incidents(incident_id) ON DELETE CASCADE,
    FOREIGN KEY (resolved_by_employee_id) REFERENCES employees(employee_id),
    
    INDEX idx_incident_id (incident_id),
    INDEX idx_resolved_by (resolved_by_employee_id),
    INDEX idx_resolution_date (actual_resolution_date)
);

-- ตารางติดตามสถานะ (Incident Status History Table)
CREATE TABLE incident_status_history (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    incident_id INT NOT NULL,
    status ENUM('รับแจ้ง', 'ดำเนินการ', 'รอดำเนินการ', 'แก้ไขแล้ว', 'ปิดเรื่อง') NOT NULL,
    changed_by_employee_id VARCHAR(20) NOT NULL,
    changed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    notes TEXT,
    
    FOREIGN KEY (incident_id) REFERENCES incidents(incident_id) ON DELETE CASCADE,
    FOREIGN KEY (changed_by_employee_id) REFERENCES employees(employee_id),
    
    INDEX idx_incident_id (incident_id),
    INDEX idx_changed_at (changed_at)
);

-- Insert default service types
INSERT INTO service_types (service_name, description) VALUES
('MS Office', 'บริการด้าน Microsoft Office (Word, Excel, PowerPoint, Outlook)'),
('PC/Mobile Device/Notebook/Printer', 'บริการด้านอุปกรณ์คอมพิวเตอร์และอุปกรณ์ต่อพ่วง'),
('ขอติดตั้ง Software', 'การติดตั้งซอฟต์แวร์ต่างๆ'),
('ขอบริการเกี่ยวกับบัญชีผู้ใช้งาน', 'บริการด้านบัญชีผู้ใช้ รหัสผ่าน และสิทธิ์การใช้งาน'),
('ระบบงานภายนอก', 'ระบบงานที่อยู่ภายนอกองค์กร'),
('ระบบงานภายใน', 'ระบบงานที่อยู่ภายในองค์กร');

-- Create view for incident summary
CREATE VIEW incident_summary AS
SELECT 
    i.incident_id,
    i.incident_number,
    i.notification_date,
    i.reporter_type,
    CASE 
        WHEN i.reporter_type = 'ภายใน ก.ล.ต.' THEN CONCAT(e.first_name, ' ', e.last_name)
        WHEN i.reporter_type = 'ภายนอก ก.ล.ต.' THEN er.full_name
    END as reporter_name,
    CASE 
        WHEN i.reporter_type = 'ภายใน ก.ล.ต.' THEN e.email
        WHEN i.reporter_type = 'ภายนอก ก.ล.ต.' THEN er.email
    END as reporter_email,
    st.service_name,
    i.problem_description,
    i.status,
    i.priority,
    i.created_at,
    i.updated_at
FROM incidents i
LEFT JOIN employees e ON i.employee_id = e.employee_id
LEFT JOIN external_reporters er ON i.external_reporter_id = er.reporter_id
LEFT JOIN service_types st ON i.service_type_id = st.service_type_id;
