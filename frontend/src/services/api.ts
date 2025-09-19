const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:7210';

export interface LoginRequest {
    email: string;
    password: string;
}

export interface LoginResponse {
    token: string;
    accessToken: string;
    refreshToken: string;
    expiresAt: string;
    user: {
        id: number;
        firstName: string;
        lastName: string;
        email: string;
        phoneNumber: string;
        address?: string;
        dateOfBirth: string;
        isActive: boolean;
        createdAt: string;
        updatedAt?: string;
        profileImageUrl?: string;
        roles: string[];
    };
}

export interface RegisterRequest {
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmPassword: string;
    phoneNumber: string;
    address?: string;
    dateOfBirth: string;
}

class ApiService {
    private baseURL: string;
    private token: string | null = null;

    constructor(baseURL: string = API_BASE_URL) {
        this.baseURL = baseURL;
        this.token = localStorage.getItem('accessToken');
    }

    private async request<T>(
        endpoint: string,
        options: RequestInit = {}
    ): Promise<T> {
        const url = `${this.baseURL}${endpoint}`;

        const config: RequestInit = {
            headers: {
                'Content-Type': 'application/json',
                ...(this.token && { Authorization: `Bearer ${this.token}` }),
                ...options.headers,
            },
            ...options,
        };

        try {
            const response = await fetch(url, config);

            if (!response.ok) {
                const errorData = await response.json().catch(() => ({}));
                throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
            }

            return await response.json();
        } catch (error) {
            console.error('API request failed:', error);
            throw error;
        }
    }

    // Auth endpoints
    async login(credentials: LoginRequest): Promise<LoginResponse> {
        const response = await this.request<LoginResponse>('/api/Auth/login', {
            method: 'POST',
            body: JSON.stringify(credentials),
        });

        // Store token
        this.token = response.accessToken;
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
        localStorage.setItem('user', JSON.stringify(response.user));

        return response;
    }

    async register(userData: RegisterRequest): Promise<LoginResponse> {
        const response = await this.request<LoginResponse>('/api/Auth/register', {
            method: 'POST',
            body: JSON.stringify(userData),
        });

        // Store token
        this.token = response.accessToken;
        localStorage.setItem('accessToken', response.accessToken);
        localStorage.setItem('refreshToken', response.refreshToken);
        localStorage.setItem('user', JSON.stringify(response.user));

        return response;
    }

    async logout(): Promise<void> {
        this.token = null;
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('user');
    }

    // User endpoints
    async getUsers(): Promise<any[]> {
        return this.request<any[]>('/api/Users');
    }

    async getUserById(id: number): Promise<any> {
        return this.request<any>(`/api/Users/${id}`);
    }

    async updateUser(id: number, userData: any): Promise<any> {
        return this.request<any>(`/api/Users/${id}`, {
            method: 'PUT',
            body: JSON.stringify(userData),
        });
    }

    async deleteUser(id: number): Promise<void> {
        return this.request<void>(`/api/Users/${id}`, {
            method: 'DELETE',
        });
    }

    // Student endpoints
    async getStudents(): Promise<any[]> {
        return this.request<any[]>('/api/Students');
    }

    async getStudentById(id: number): Promise<any> {
        return this.request<any>(`/api/Students/${id}`);
    }

    async createStudent(studentData: any): Promise<any> {
        return this.request<any>('/api/Students', {
            method: 'POST',
            body: JSON.stringify(studentData),
        });
    }

    async updateStudent(id: number, studentData: any): Promise<any> {
        return this.request<any>(`/api/Students/${id}`, {
            method: 'PUT',
            body: JSON.stringify(studentData),
        });
    }

    async deleteStudent(id: number): Promise<void> {
        return this.request<void>(`/api/Students/${id}`, {
            method: 'DELETE',
        });
    }

    // Teacher endpoints
    async getTeachers(): Promise<any[]> {
        return this.request<any[]>('/api/Teachers');
    }

    async getTeacherById(id: number): Promise<any> {
        return this.request<any>(`/api/Teachers/${id}`);
    }

    async createTeacher(teacherData: any): Promise<any> {
        return this.request<any>('/api/Teachers', {
            method: 'POST',
            body: JSON.stringify(teacherData),
        });
    }

    async updateTeacher(id: number, teacherData: any): Promise<any> {
        return this.request<any>(`/api/Teachers/${id}`, {
            method: 'PUT',
            body: JSON.stringify(teacherData),
        });
    }

    async deleteTeacher(id: number): Promise<void> {
        return this.request<void>(`/api/Teachers/${id}`, {
            method: 'DELETE',
        });
    }

    // Class endpoints
    async getClasses(): Promise<any[]> {
        return this.request<any[]>('/api/Classes');
    }

    async getClassById(id: number): Promise<any> {
        return this.request<any>(`/api/Classes/${id}`);
    }

    async createClass(classData: any): Promise<any> {
        return this.request<any>('/api/Classes', {
            method: 'POST',
            body: JSON.stringify(classData),
        });
    }

    async updateClass(id: number, classData: any): Promise<any> {
        return this.request<any>(`/api/Classes/${id}`, {
            method: 'PUT',
            body: JSON.stringify(classData),
        });
    }

    async deleteClass(id: number): Promise<void> {
        return this.request<void>(`/api/Classes/${id}`, {
            method: 'DELETE',
        });
    }

    async enrollStudent(classId: number, studentId: number): Promise<any> {
        return this.request<any>(`/api/Classes/${classId}/enroll`, {
            method: 'POST',
            body: JSON.stringify({ studentId }),
        });
    }

    // Subject endpoints
    async getSubjects(): Promise<any[]> {
        return this.request<any[]>('/api/Subjects');
    }

    async getSubjectById(id: number): Promise<any> {
        return this.request<any>(`/api/Subjects/${id}`);
    }

    async createSubject(subjectData: any): Promise<any> {
        return this.request<any>('/api/Subjects', {
            method: 'POST',
            body: JSON.stringify(subjectData),
        });
    }

    async updateSubject(id: number, subjectData: any): Promise<any> {
        return this.request<any>(`/api/Subjects/${id}`, {
            method: 'PUT',
            body: JSON.stringify(subjectData),
        });
    }

    async deleteSubject(id: number): Promise<void> {
        return this.request<void>(`/api/Subjects/${id}`, {
            method: 'DELETE',
        });
    }

    // Check if user is authenticated
    isAuthenticated(): boolean {
        return !!this.token && !!localStorage.getItem('accessToken');
    }

    // Get current user from localStorage
    getCurrentUser(): any | null {
        const user = localStorage.getItem('user');
        return user ? JSON.parse(user) : null;
    }
}

export const apiService = new ApiService();
export default apiService;
