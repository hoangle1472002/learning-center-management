import React, { useState, useEffect } from 'react';
import { Card, CardContent, CardHeader, CardTitle } from '../components/ui/Card';
import { Button } from '../components/ui/Button';
import { Input } from '../components/ui/Input';

interface Class {
    id: number;
    name: string;
    subjectName: string;
    teacherName: string;
    maxStudents: number;
    currentStudents: number;
    startDate?: string;
    endDate?: string;
    price?: number;
    status?: string;
    room?: string;
    enrollmentRate: number;
}

interface Student {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    studentCode: string;
}

const Enrollment: React.FC = () => {
    const [classes, setClasses] = useState<Class[]>([]);
    const [students, setStudents] = useState<Student[]>([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [selectedClass, setSelectedClass] = useState<number | null>(null);
    const [selectedStudent, setSelectedStudent] = useState<number | null>(null);
    const [showEnrollModal, setShowEnrollModal] = useState(false);

    useEffect(() => {
        fetchClasses();
        fetchStudents();
    }, []);

    const fetchClasses = async () => {
        try {
            const response = await fetch('/api/Class');
            if (response.ok) {
                const data = await response.json();
                setClasses(data);
            }
        } catch (error) {
            console.error('Error fetching classes:', error);
        }
    };

    const fetchStudents = async () => {
        try {
            const response = await fetch('/api/Student');
            if (response.ok) {
                const data = await response.json();
                setStudents(data);
            }
        } catch (error) {
            console.error('Error fetching students:', error);
        } finally {
            setLoading(false);
        }
    };

    const handleEnroll = async () => {
        if (!selectedClass || !selectedStudent) return;

        try {
            const response = await fetch('/api/Class/enroll', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    studentId: selectedStudent,
                    classId: selectedClass,
                }),
            });

            if (response.ok) {
                alert('Student enrolled successfully!');
                setShowEnrollModal(false);
                setSelectedClass(null);
                setSelectedStudent(null);
                fetchClasses(); // Refresh classes to update enrollment count
            } else {
                const error = await response.json();
                alert(`Error: ${error.message}`);
            }
        } catch (error) {
            console.error('Error enrolling student:', error);
            alert('An error occurred while enrolling student');
        }
    };

    const handleUnenroll = async (studentId: number, classId: number) => {
        if (!window.confirm('Are you sure you want to unenroll this student?')) return;

        try {
            const response = await fetch('/api/Class/unenroll', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    studentId,
                    classId,
                }),
            });

            if (response.ok) {
                alert('Student unenrolled successfully!');
                fetchClasses(); // Refresh classes
            } else {
                const error = await response.json();
                alert(`Error: ${error.message}`);
            }
        } catch (error) {
            console.error('Error unenrolling student:', error);
            alert('An error occurred while unenrolling student');
        }
    };

    const filteredClasses = classes.filter(cls => {
        const matchesSearch = cls.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
            cls.subjectName.toLowerCase().includes(searchTerm.toLowerCase()) ||
            cls.teacherName.toLowerCase().includes(searchTerm.toLowerCase());
        return matchesSearch;
    });

    const getStatusColor = (status?: string) => {
        switch (status) {
            case 'Active': return 'bg-green-100 text-green-800';
            case 'Draft': return 'bg-gray-100 text-gray-800';
            case 'Completed': return 'bg-blue-100 text-blue-800';
            case 'Cancelled': return 'bg-red-100 text-red-800';
            default: return 'bg-gray-100 text-gray-800';
        }
    };

    if (loading) {
        return (
            <div className="flex items-center justify-center h-64">
                <div className="text-lg">Loading...</div>
            </div>
        );
    }

    return (
        <div className="space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-3xl font-bold">Student Enrollment</h1>
                <Button onClick={() => setShowEnrollModal(true)}>
                    Enroll Student
                </Button>
            </div>

            {/* Search */}
            <Input
                placeholder="Search classes..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="max-w-sm"
            />

            {/* Classes Grid */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {filteredClasses.map((cls) => (
                    <Card key={cls.id} className="hover:shadow-lg transition-shadow">
                        <CardHeader>
                            <div className="flex justify-between items-start">
                                <CardTitle className="text-lg">{cls.name}</CardTitle>
                                <span className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(cls.status)}`}>
                                    {cls.status || 'Draft'}
                                </span>
                            </div>
                            <p className="text-sm text-gray-600">{cls.subjectName}</p>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-2">
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Teacher:</span>
                                    <span>{cls.teacherName}</span>
                                </div>
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Students:</span>
                                    <span>{cls.currentStudents}/{cls.maxStudents}</span>
                                </div>
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-600">Enrollment:</span>
                                    <span>{cls.enrollmentRate.toFixed(1)}%</span>
                                </div>
                                {cls.room && (
                                    <div className="flex justify-between text-sm">
                                        <span className="text-gray-600">Room:</span>
                                        <span>{cls.room}</span>
                                    </div>
                                )}
                                {cls.price && (
                                    <div className="flex justify-between text-sm">
                                        <span className="text-gray-600">Price:</span>
                                        <span>${cls.price}</span>
                                    </div>
                                )}
                                <div className="w-full bg-gray-200 rounded-full h-2 mt-2">
                                    <div
                                        className="bg-blue-600 h-2 rounded-full"
                                        style={{ width: `${cls.enrollmentRate}%` }}
                                    ></div>
                                </div>
                            </div>
                            <div className="flex gap-2 mt-4">
                                <Button
                                    size="sm"
                                    onClick={() => {
                                        setSelectedClass(cls.id);
                                        setShowEnrollModal(true);
                                    }}
                                    disabled={cls.currentStudents >= cls.maxStudents || cls.status !== 'Active'}
                                >
                                    Enroll
                                </Button>
                                <Button size="sm" variant="outline">
                                    View Details
                                </Button>
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>

            {/* Enroll Modal */}
            {showEnrollModal && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
                    <Card className="w-full max-w-md">
                        <CardHeader>
                            <CardTitle>Enroll Student</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-4">
                                <div>
                                    <label className="block text-sm font-medium mb-2">Select Class</label>
                                    <select
                                        value={selectedClass || ''}
                                        onChange={(e) => setSelectedClass(parseInt(e.target.value))}
                                        className="w-full px-3 py-2 border border-gray-300 rounded-md"
                                    >
                                        <option value="">Choose a class</option>
                                        {classes
                                            .filter(cls => cls.status === 'Active' && cls.currentStudents < cls.maxStudents)
                                            .map(cls => (
                                                <option key={cls.id} value={cls.id}>
                                                    {cls.name} - {cls.subjectName} ({cls.currentStudents}/{cls.maxStudents})
                                                </option>
                                            ))}
                                    </select>
                                </div>
                                <div>
                                    <label className="block text-sm font-medium mb-2">Select Student</label>
                                    <select
                                        value={selectedStudent || ''}
                                        onChange={(e) => setSelectedStudent(parseInt(e.target.value))}
                                        className="w-full px-3 py-2 border border-gray-300 rounded-md"
                                    >
                                        <option value="">Choose a student</option>
                                        {students.map(student => (
                                            <option key={student.id} value={student.id}>
                                                {student.firstName} {student.lastName} ({student.studentCode})
                                            </option>
                                        ))}
                                    </select>
                                </div>
                                <div className="flex gap-2">
                                    <Button onClick={handleEnroll} disabled={!selectedClass || !selectedStudent}>
                                        Enroll
                                    </Button>
                                    <Button
                                        variant="outline"
                                        onClick={() => {
                                            setShowEnrollModal(false);
                                            setSelectedClass(null);
                                            setSelectedStudent(null);
                                        }}
                                    >
                                        Cancel
                                    </Button>
                                </div>
                            </div>
                        </CardContent>
                    </Card>
                </div>
            )}

            {filteredClasses.length === 0 && (
                <div className="text-center py-12">
                    <p className="text-gray-500 text-lg">No classes found</p>
                </div>
            )}
        </div>
    );
};

export default Enrollment;
