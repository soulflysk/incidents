// Incident Management System JavaScript

// Global variables
let currentIncidentId = null;
let incidents = [];
let employees = [];

// Initialize the application
document.addEventListener('DOMContentLoaded', function() {
    initializeApp();
});

function initializeApp() {
    // Set current date/time for notification date
    const now = new Date();
    document.getElementById('notificationDate').value = now.toISOString().slice(0, 16);
    
    // Load sample data
    loadSampleData();
    
    // Setup form validation
    setupFormValidation();
    
    // Setup event listeners
    setupEventListeners();
}

function loadSampleData() {
    // Sample incidents data
    incidents = [
        {
            id: 'INC-2024-001',
            notificationDate: '2024-02-25T09:30:00',
            reporterType: 'ภายใน ก.ล.ต.',
            reporterName: 'สมชาย ใจดี',
            reporterEmail: 'somchai@bot.or.th',
            serviceType: 'MS Office',
            problemDescription: 'ไม่สามารถเปิดไฟล์ Excel ได้ ขึ้นข้อความว่าไฟล์เสียหาย',
            status: 'ดำเนินการ',
            priority: 'ปานกลาง',
            assignedTo: ['EMP001'],
            createdAt: '2024-02-25T09:30:00'
        },
        {
            id: 'INC-2024-002',
            notificationDate: '2024-02-25T10:15:00',
            reporterType: 'ภายนอก ก.ล.ต.',
            reporterName: 'บริษัท ABC',
            reporterEmail: 'support@abc.com',
            serviceType: 'PC/Mobile Device/Notebook/Printer',
            problemDescription: 'คอมพิวเตอร์เปิดไม่ติด มีเสียงดังจากเครื่อง',
            status: 'รับแจ้ง',
            priority: 'วิกฤต',
            assignedTo: [],
            createdAt: '2024-02-25T10:15:00'
        }
    ];
    
    // Sample employees data
    employees = [
        { id: 'EMP001', name: 'สมชาย ใจดี', level: '1st Level', email: 'somchai@bot.or.th' },
        { id: 'EMP002', name: 'มานี รักดี', level: '1st Level', email: 'manee@bot.or.th' },
        { id: 'EMP003', name: 'วิทยา เชี่ยวชาญ', level: '2nd Level', email: 'witaya@bot.or.th' },
        { id: 'EMP004', name: 'ปรียา เก่ง', level: '2nd Level', email: 'priya@bot.or.th' }
    ];
}

function setupEventListeners() {
    // Form submission
    document.getElementById('incidentForm').addEventListener('submit', function(e) {
        e.preventDefault();
        saveIncident();
    });
    
    // Search functionality
    document.getElementById('searchIncident').addEventListener('input', function(e) {
        filterIncidents();
    });
    
    // Filter changes
    document.getElementById('filterStatus').addEventListener('change', filterIncidents);
    document.getElementById('filterPriority').addEventListener('change', filterIncidents);
    document.getElementById('filterDate').addEventListener('change', filterIncidents);
}

function setupFormValidation() {
    // Bootstrap form validation
    const forms = document.querySelectorAll('.needs-validation');
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        });
    });
}

// View Management
function showIncidentList() {
    hideAllViews();
    document.getElementById('incidentListView').style.display = 'block';
    refreshIncidentList();
}

function showNewIncident() {
    hideAllViews();
    document.getElementById('newIncidentView').style.display = 'block';
    resetForm();
}

function showAssignments() {
    hideAllViews();
    document.getElementById('assignmentView').style.display = 'block';
}

function hideAllViews() {
    document.querySelectorAll('.content-section').forEach(section => {
        section.style.display = 'none';
    });
}

// Reporter Type Toggle
function toggleReporterFields() {
    const reporterType = document.getElementById('reporterType').value;
    const internalFields = document.getElementById('internalReporterFields');
    const externalFields = document.getElementById('externalReporterFields');
    
    if (reporterType === 'ภายใน ก.ล.ต.') {
        internalFields.style.display = 'block';
        externalFields.style.display = 'none';
        document.getElementById('employeeId').setAttribute('required', 'required');
        document.getElementById('externalEmail').removeAttribute('required');
        document.getElementById('externalName').removeAttribute('required');
    } else if (reporterType === 'ภายนอก ก.ล.ต.') {
        internalFields.style.display = 'none';
        externalFields.style.display = 'block';
        document.getElementById('employeeId').removeAttribute('required');
        document.getElementById('externalEmail').setAttribute('required', 'required');
        document.getElementById('externalName').setAttribute('required', 'required');
    } else {
        internalFields.style.display = 'none';
        externalFields.style.display = 'none';
    }
}

// Employee Lookup
function lookupEmployee() {
    const employeeId = document.getElementById('employeeId').value;
    const employee = employees.find(emp => emp.id === employeeId);
    
    if (employee) {
        document.getElementById('employeeName').value = employee.name;
        document.getElementById('employeeName').classList.remove('is-invalid');
        document.getElementById('employeeName').classList.add('is-valid');
    } else {
        document.getElementById('employeeName').value = 'ไม่พบข้อมูลพนักงาน';
        document.getElementById('employeeName').classList.add('is-invalid');
    }
}

// Incident Management
function saveIncident() {
    const form = document.getElementById('incidentForm');
    
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    const reporterType = document.getElementById('reporterType').value;
    const incidentData = {
        id: generateIncidentNumber(),
        notificationDate: document.getElementById('notificationDate').value,
        reporterType: reporterType,
        serviceType: document.getElementById('serviceType').value,
        problemDescription: document.getElementById('problemDescription').value,
        priority: document.getElementById('priority').value,
        status: 'รับแจ้ง',
        assignedTo: [],
        createdAt: new Date().toISOString()
    };
    
    // Add reporter information based on type
    if (reporterType === 'ภายใน ก.ล.ต.') {
        const employeeId = document.getElementById('employeeId').value;
        const employee = employees.find(emp => emp.id === employeeId);
        incidentData.reporterName = employee ? employee.name : 'Unknown';
        incidentData.reporterEmail = employee ? employee.email : '';
        incidentData.employeeId = employeeId;
    } else {
        incidentData.reporterName = document.getElementById('externalName').value;
        incidentData.reporterEmail = document.getElementById('externalEmail').value;
        incidentData.companyName = document.getElementById('externalCompany').value;
        incidentData.phone = document.getElementById('externalPhone').value;
    }
    
    incidents.push(incidentData);
    
    showToast('success', 'บันทึกเหตุการณ์สำเร็จ', `เลขที่เหตุการณ์: ${incidentData.id}`);
    
    setTimeout(() => {
        showIncidentList();
    }, 1500);
}

function generateIncidentNumber() {
    const year = new Date().getFullYear();
    const month = String(new Date().getMonth() + 1).padStart(2, '0');
    const sequence = String(incidents.length + 1).padStart(3, '0');
    return `INC-${year}-${sequence}`;
}

function refreshIncidentList() {
    const tbody = document.getElementById('incidentTableBody');
    tbody.innerHTML = '';
    
    incidents.forEach(incident => {
        const row = createIncidentRow(incident);
        tbody.appendChild(row);
    });
}

function createIncidentRow(incident) {
    const row = document.createElement('tr');
    row.innerHTML = `
        <td><span class="badge bg-info">${incident.id}</span></td>
        <td>${formatDateTime(incident.notificationDate)}</td>
        <td>${incident.reporterName} (${incident.reporterType})</td>
        <td>${incident.serviceType}</td>
        <td>${truncateText(incident.problemDescription, 50)}</td>
        <td><span class="badge bg-${getStatusColor(incident.status)}">${incident.status}</span></td>
        <td><span class="badge bg-${getPriorityColor(incident.priority)}">${incident.priority}</span></td>
        <td>
            <button class="btn btn-sm btn-primary" onclick="viewIncident('${incident.id}')" title="ดูรายละเอียด">
                <i class="bi bi-eye"></i>
            </button>
            <button class="btn btn-sm btn-warning" onclick="editIncident('${incident.id}')" title="แก้ไข">
                <i class="bi bi-pencil"></i>
            </button>
            <button class="btn btn-sm btn-success" onclick="assignIncident('${incident.id}')" title="มอบหมาย">
                <i class="bi bi-person-plus"></i>
            </button>
            <button class="btn btn-sm btn-info" onclick="openResolutionModal('${incident.id}')" title="บันทึกการแก้ไข">
                <i class="bi bi-tools"></i>
            </button>
        </td>
    `;
    return row;
}

function viewIncident(incidentId) {
    const incident = incidents.find(inc => inc.id === incidentId);
    if (!incident) return;
    
    // Create modal content for viewing incident details
    const modalContent = `
        <div class="modal fade" id="viewIncidentModal" tabindex="-1">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">รายละเอียดเหตุการณ์ ${incident.id}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-6">
                                <strong>เลขที่เหตุการณ์:</strong> ${incident.id}<br>
                                <strong>วันที่รับแจ้ง:</strong> ${formatDateTime(incident.notificationDate)}<br>
                                <strong>ผู้แจ้ง:</strong> ${incident.reporterName}<br>
                                <strong>ประเภทผู้แจ้ง:</strong> ${incident.reporterType}
                            </div>
                            <div class="col-md-6">
                                <strong>ประเภทบริการ:</strong> ${incident.serviceType}<br>
                                <strong>สถานะ:</strong> <span class="badge bg-${getStatusColor(incident.status)}">${incident.status}</span><br>
                                <strong>ความสำคัญ:</strong> <span class="badge bg-${getPriorityColor(incident.priority)}">${incident.priority}</span><br>
                                <strong>วันที่สร้าง:</strong> ${formatDateTime(incident.createdAt)}
                            </div>
                        </div>
                        <hr>
                        <div class="row">
                            <div class="col-12">
                                <strong>รายละเอียดปัญหา:</strong><br>
                                <p>${incident.problemDescription}</p>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">ปิด</button>
                        <button type="button" class="btn btn-warning" onclick="editIncident('${incident.id}')">แก้ไข</button>
                        <button type="button" class="btn btn-success" onclick="assignIncident('${incident.id}')">มอบหมาย</button>
                    </div>
                </div>
            </div>
        </div>
    `;
    
    // Remove existing modal if any
    const existingModal = document.getElementById('viewIncidentModal');
    if (existingModal) {
        existingModal.remove();
    }
    
    // Add new modal to body and show it
    document.body.insertAdjacentHTML('beforeend', modalContent);
    const modal = new bootstrap.Modal(document.getElementById('viewIncidentModal'));
    modal.show();
}

function editIncident(incidentId) {
    const incident = incidents.find(inc => inc.id === incidentId);
    if (!incident) return;
    
    // Populate form with incident data
    document.getElementById('reporterType').value = incident.reporterType;
    document.getElementById('notificationDate').value = incident.notificationDate;
    document.getElementById('serviceType').value = incident.serviceType;
    document.getElementById('problemDescription').value = incident.problemDescription;
    document.getElementById('priority').value = incident.priority;
    
    // Populate reporter fields based on type
    toggleReporterFields();
    if (incident.reporterType === 'ภายใน ก.ล.ต.') {
        document.getElementById('employeeId').value = incident.employeeId || '';
        lookupEmployee();
    } else {
        document.getElementById('externalName').value = incident.reporterName;
        document.getElementById('externalEmail').value = incident.reporterEmail;
        document.getElementById('externalCompany').value = incident.companyName || '';
        document.getElementById('externalPhone').value = incident.phone || '';
    }
    
    currentIncidentId = incidentId;
    showNewIncident();
    
    // Change form title and submit button
    document.querySelector('#newIncidentView .card-header h5').innerHTML = 
        '<i class="bi bi-pencil"></i> แก้ไขเหตุการณ์ ' + incidentId;
}

function assignIncident(incidentId) {
    currentIncidentId = incidentId;
    showAssignments();
    
    showToast('info', 'เลือกเจ้าหน้าที่ที่ต้องการมอบหมาย', `เหตุการณ์: ${incidentId}`);
}

function assignTo1stLevel() {
    const selectedStaff = [];
    document.querySelectorAll('#assignmentView .card:first-child input[type="checkbox"]:checked').forEach(checkbox => {
        selectedStaff.push(checkbox.value);
    });
    
    if (selectedStaff.length === 0) {
        showToast('warning', 'กรุณาเลือกเจ้าหน้าที่อย่างน้อย 1 คน');
        return;
    }
    
    const incident = incidents.find(inc => inc.id === currentIncidentId);
    if (incident) {
        incident.assignedTo = selectedStaff;
        incident.status = 'ดำเนินการ';
        showToast('success', 'มอบหมายเรียบร้อย', `เจ้าหน้าที่: ${selectedStaff.join(', ')}`);
        showIncidentList();
    }
}

function assignTo2ndLevel() {
    const selectedStaff = [];
    document.querySelectorAll('#assignmentView .card:last-child input[type="checkbox"]:checked').forEach(checkbox => {
        selectedStaff.push(checkbox.value);
    });
    
    if (selectedStaff.length === 0) {
        showToast('warning', 'กรุณาเลือกเจ้าหน้าที่อย่างน้อย 1 คน');
        return;
    }
    
    const incident = incidents.find(inc => inc.id === currentIncidentId);
    if (incident) {
        incident.assignedTo = selectedStaff;
        incident.status = 'ดำเนินการ';
        showToast('success', 'มอบหมายเรียบร้อย', `เจ้าหน้าที่: ${selectedStaff.join(', ')}`);
        showIncidentList();
    }
}

function openResolutionModal(incidentId) {
    currentIncidentId = incidentId;
    const modal = new bootstrap.Modal(document.getElementById('resolutionModal'));
    modal.show();
}

function saveResolution() {
    const form = document.getElementById('resolutionForm');
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    const resolutionData = {
        incidentId: currentIncidentId,
        componentName: document.getElementById('componentName').value,
        resolutionDetails: document.getElementById('resolutionDetails').value,
        hoursSpent: parseFloat(document.getElementById('hoursSpent').value),
        estimatedStartDate: document.getElementById('estimatedStartDate').value,
        estimatedEndDate: document.getElementById('estimatedEndDate').value,
        actualResolutionDate: document.getElementById('actualResolutionDate').value,
        resolvedAt: new Date().toISOString()
    };
    
    // Update incident status
    const incident = incidents.find(inc => inc.id === currentIncidentId);
    if (incident) {
        incident.status = 'แก้ไขแล้ว';
        incident.resolution = resolutionData;
        showToast('success', 'บันทึกการแก้ไขเรียบร้อย', `เหตุการณ์: ${currentIncidentId}`);
        
        // Close modal and refresh list
        bootstrap.Modal.getInstance(document.getElementById('resolutionModal')).hide();
        showIncidentList();
    }
}

// Filter and Search
function filterIncidents() {
    const searchTerm = document.getElementById('searchIncident').value.toLowerCase();
    const statusFilter = document.getElementById('filterStatus').value;
    const priorityFilter = document.getElementById('filterPriority').value;
    const dateFilter = document.getElementById('filterDate').value;
    
    const filteredIncidents = incidents.filter(incident => {
        const matchesSearch = !searchTerm || 
            incident.id.toLowerCase().includes(searchTerm) ||
            incident.reporterName.toLowerCase().includes(searchTerm) ||
            incident.problemDescription.toLowerCase().includes(searchTerm);
        
        const matchesStatus = !statusFilter || incident.status === statusFilter;
        const matchesPriority = !priorityFilter || incident.priority === priorityFilter;
        const matchesDate = !dateFilter || incident.notificationDate.startsWith(dateFilter);
        
        return matchesSearch && matchesStatus && matchesPriority && matchesDate;
    });
    
    // Update table with filtered results
    const tbody = document.getElementById('incidentTableBody');
    tbody.innerHTML = '';
    
    filteredIncidents.forEach(incident => {
        const row = createIncidentRow(incident);
        tbody.appendChild(row);
    });
}

function applyFilters() {
    filterIncidents();
    showToast('info', 'กรองข้อมูลเรียบร้อย', `พบ ${incidents.length} รายการ`);
}

// Utility Functions
function formatDateTime(dateTimeString) {
    const date = new Date(dateTimeString);
    return date.toLocaleString('th-TH', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function truncateText(text, maxLength) {
    if (text.length <= maxLength) return text;
    return text.substring(0, maxLength) + '...';
}

function getStatusColor(status) {
    const colors = {
        'รับแจ้ง': 'secondary',
        'ดำเนินการ': 'info',
        'รอดำเนินการ': 'warning',
        'แก้ไขแล้ว': 'success',
        'ปิดเรื่อง': 'dark'
    };
    return colors[status] || 'secondary';
}

function getPriorityColor(priority) {
    const colors = {
        'ต่ำ': 'success',
        'ปานกลาง': 'warning',
        'สูง': 'orange',
        'วิกฤต': 'danger'
    };
    return colors[priority] || 'secondary';
}

function resetForm() {
    document.getElementById('incidentForm').reset();
    document.getElementById('incidentForm').classList.remove('was-validated');
    document.getElementById('notificationDate').value = new Date().toISOString().slice(0, 16);
    toggleReporterFields();
    currentIncidentId = null;
    
    // Reset form title
    document.querySelector('#newIncidentView .card-header h5').innerHTML = 
        '<i class="bi bi-plus-circle"></i> บันทึกเหตุการณ์ใหม่';
}

function saveDraft() {
    showToast('info', 'บันทึกฉบับร่าง', 'ข้อมูลถูกบันทึกชั่วคราว');
    // In real implementation, this would save to localStorage or server
}

// Toast Notifications
function showToast(type, title, message = '') {
    const toastContainer = document.getElementById('toastContainer') || createToastContainer();
    
    const toastId = 'toast-' + Date.now();
    const toastHtml = `
        <div id="${toastId}" class="toast align-items-center text-white bg-${type === 'success' ? 'success' : type === 'warning' ? 'warning' : type === 'danger' ? 'danger' : 'info'} border-0" role="alert">
            <div class="d-flex">
                <div class="toast-body">
                    <strong>${title}</strong>${message ? '<br>' + message : ''}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;
    
    toastContainer.insertAdjacentHTML('beforeend', toastHtml);
    const toastElement = document.getElementById(toastId);
    const toast = new bootstrap.Toast(toastElement);
    toast.show();
    
    // Remove toast element after it's hidden
    toastElement.addEventListener('hidden.bs.toast', () => {
        toastElement.remove();
    });
}

function createToastContainer() {
    const container = document.createElement('div');
    container.id = 'toastContainer';
    container.className = 'toast-container';
    document.body.appendChild(container);
    return container;
}

// Auto-save functionality
function setupAutoSave() {
    setInterval(() => {
        const form = document.getElementById('incidentForm');
        if (form.checkValidity()) {
            // Save form data to localStorage
            const formData = new FormData(form);
            const data = {};
            formData.forEach((value, key) => {
                data[key] = value;
            });
            localStorage.setItem('incidentDraft', JSON.stringify(data));
        }
    }, 30000); // Auto-save every 30 seconds
}

// Load draft on page load
function loadDraft() {
    const draft = localStorage.getItem('incidentDraft');
    if (draft) {
        const data = JSON.parse(draft);
        Object.keys(data).forEach(key => {
            const element = document.getElementById(key);
            if (element) {
                element.value = data[key];
            }
        });
        showToast('info', 'โหลดข้อมูลฉบับร่าง', 'พบข้อมูลที่บันทึกไว้');
    }
}

// Initialize auto-save and draft loading
setupAutoSave();
loadDraft();
