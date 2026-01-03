import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import SharedHeader from '../components/SharedHeader';
import {
  CheckSquare,
  Clock,
  ChevronRight,
  Award,
  TrendingUp,
  Flame,
  Target,
  BookOpenCheck,
  CalendarIcon,
  User
} from 'lucide-react';

export default function Dashboard() {
  const navigate = useNavigate();
  const { user, setUser } = useUser();

  // Theme colors
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
     muted: '#5A7A6B',
   };

   const [isLoggedIn, setIsLoggedIn] = useState(true);

  // To-Do
  const [todos, setTodos] = useState([
    { id: 1, text: 'Complete Math Assignment', completed: false },
     { id: 2, text: 'Read Chapter 5 - Physics', completed: true },
      { id: 3, text: 'Prepare for Chemistry test', completed: false },
  ]);

   // Notes
  const [notes, setNotes] = useState([
    { id: 1, title: 'Important Formulas', content: 'Newton\'s laws, Kinematic equations, Energy conservation', date: '2024-12-15' },
  ]);

   // Planner
  const [events, setEvents] = useState([
     { id: 1, title: 'Math Exam', date: '2024-12-20', time: '10:00', duration: '2h' },
    { id: 2, title: 'Physics Lab', date: '2024-12-21', time: '14:00', duration: '3h' },
   ]);



  // Attendance state
  const [attendanceRecords, setAttendanceRecords] = useState([
     { id: 1, subject: 'Mathematics', date: '2024-12-15', status: 'present' },
    { id: 2, subject: 'Physics', date: '2024-12-15', status: 'present' },
    { id: 3, subject: 'Chemistry', date: '2024-12-14', status: 'absent' },
    { id: 4, subject: 'Computer Science', date: '2024-12-14', status: 'present' },
     { id: 5, subject: 'Mathematics', date: '2024-12-13', status: 'present' },
  ]);

   // Helpers
   const addTodo = () => {
      const newTodoText = prompt('Enter new task:');
      if (newTodoText?.trim()) {
      setTodos((t) => [...t, { id: Date.now(), text: newTodoText.trim(), completed: false }]);
     }
  };
   const toggleTodo = (id) => {
    setTodos((t) => t.map(x => x.id === id ? { ...x, completed: !x.completed } : x));
      if (!todos.find(x => x.id === id).completed) {
        setUser(u => ({ ...u, completedTasks: u.completedTasks + 1 }));
      }
   };
    const deleteTodo = (id) => setTodos((t) => t.filter(x => x.id !== id));

   const addNote = () => {
     const title = prompt('Note title:');
     const content = prompt('Note content:');
     if (title?.trim() && content?.trim()) {
       setNotes(n => [{ id: Date.now(), title: title.trim(), content: content.trim(), date: new Date().toISOString().split('T')[0] }, ...n]);
    }
    };
   const deleteNote = (id) => setNotes(n => n.filter(x => x.id !== id));

  const addEvent = (title, date, time) => {
    if (!title || !date || !time) return;
    setEvents(e => [...e, { id: Date.now(), title, date, time, duration: '1h' }]);
  };
  const deleteEvent = (id) => setEvents(e => e.filter(x => x.id !== id));



   const markAttendance = (subject, status) => {
     const today = new Date().toISOString().split('T')[0];
     setAttendanceRecords(a => [{ id: Date.now(), subject, date: today, status }, ...a]);
   };

  // Calculate attendance stats
  const attendanceStats = {
     total: attendanceRecords.length,
      present: attendanceRecords.filter(r => r.status === 'present').length,
       absent: attendanceRecords.filter(r => r.status === 'absent').length,
    percentage: Math.round((attendanceRecords.filter(r => r.status === 'present').length / attendanceRecords.length) * 100) || 0,
    };

    return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen text-[#2C3E3F]">
      <SharedHeader title="Mentora - Dashboard" />
      <main className="container mx-auto px-4 mt-6">
           {/* Stats Grid */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
                <div className="bg-white rounded-2xl p-4 shadow-lg border hover:scale-105 transition-transform" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center justify-between">
                   <div>
                <p className="text-sm text-[#5A7A6B]">Study Hours</p>
                   <p className="text-2xl font-bold" style={{ color: M.primary }}>{user.totalHours}h</p>
              </div>
                 <Clock className="w-8 h-8" style={{ color: M.primary }} />
            </div>
               </div>
          <div className="bg-white rounded-2xl p-4 shadow-lg border hover:scale-105 transition-transform" style={{ borderColor: M.bg3 }}>
                 <div className="flex items-center justify-between">
              <div>
                    <p className="text-sm text-[#5A7A6B]">Completed</p>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{user.completedTasks}</p>
                    </div>
                <Award className="w-8 h-8" style={{ color: M.primary }} />
                       </div>
                 </div>
                    <div className="bg-white rounded-2xl p-4 shadow-lg border hover:scale-105 transition-transform" style={{ borderColor: M.bg3 }}>
                 <div className="flex items-center justify-between">
               <div>
                      <p className="text-sm text-[#5A7A6B]">Streak</p>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{user.studyStreak} days</p>
                  </div>
              <Flame className="w-8 h-8 text-orange-500" />
                     </div>
          </div>
          <div className="bg-white rounded-2xl p-4 shadow-lg border hover:scale-105 transition-transform" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center justify-between">
                <div>
                  <p className="text-sm text-[#5A7A6B]">Attendance</p>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{attendanceStats.percentage}%</p>
               </div>
                <TrendingUp className="w-8 h-8" style={{ color: M.primary }} />
            </div>
            </div>
        </div>

        {/* Quick Actions */}
        <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-6">
          <button
            onClick={() => navigate('/study-planner')}
            className="bg-white rounded-2xl p-6 shadow-lg border hover:scale-105 transition-transform text-center"
            style={{ borderColor: M.bg3 }}
          >
            <Clock className="w-12 h-12 mx-auto mb-3" style={{ color: M.primary }} />
            <h3 className="font-bold text-[#2C3E3F] text-sm">Study Planner</h3>
            <p className="text-xs text-[#5A7A6B] mt-1">Manage your schedule</p>
          </button>

          <button
            onClick={() => navigate('/career-builder')}
            className="bg-white rounded-2xl p-6 shadow-lg border hover:scale-105 transition-transform text-center"
            style={{ borderColor: M.bg3 }}
          >
            <TrendingUp className="w-12 h-12 mx-auto mb-3" style={{ color: M.primary }} />
            <h3 className="font-bold text-[#2C3E3F] text-sm">Career Builder</h3>
            <p className="text-xs text-[#5A7A6B] mt-1">Plan your future</p>
          </button>

          <button
            onClick={() => navigate('/skills')}
            className="bg-white rounded-2xl p-6 shadow-lg border hover:scale-105 transition-transform text-center"
            style={{ borderColor: M.bg3 }}
          >
            <Award className="w-12 h-12 mx-auto mb-3" style={{ color: M.primary }} />
            <h3 className="font-bold text-[#2C3E3F] text-sm">My Skills</h3>
            <p className="text-xs text-[#5A7A6B] mt-1">Track your progress</p>
          </button>

          <button
            onClick={() => navigate('/profile')}
            className="bg-white rounded-2xl p-6 shadow-lg border hover:scale-105 transition-transform text-center"
            style={{ borderColor: M.bg3 }}
          >
            <User className="w-12 h-12 mx-auto mb-3" style={{ color: M.primary }} />
            <h3 className="font-bold text-[#2C3E3F] text-sm">Profile</h3>
            <p className="text-xs text-[#5A7A6B] mt-1">View & edit profile</p>
          </button>
        </div>


          {/* Upcoming Events & Tasks */}
          <div className="bg-white rounded-2xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
          <h3 className="font-bold text-[#2C3E3F] flex items-center gap-2 mb-4">
                <BookOpenCheck className="w-5 h-5" style={{ color: M.primary }} />
               Upcoming Events & Tasks
          </h3>
          <div className="space-y-4">

             
            <div>
              <h4 className="text-sm font-semibold text-[#5A7A6B] mb-2 flex items-center gap-2">
                <Clock className="w-4 h-4" />
                Upcoming Events
              </h4>
              <div className="space-y-2">
                {events.slice(0, 2).map(event => (
                  <div key={event.id} className="flex items-center justify-between p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                    <div className="flex items-center gap-3">
                      <div className="w-8 h-8 rounded-full flex items-center justify-center" style={{ background: M.bg3 }}>
                        <Clock className="w-4 h-4" style={{ color: M.primary }} />
                      </div>
                      <div>
                        <p className="font-medium text-[#2C3E3F] text-sm">{event.title}</p>
                        <p className="text-xs text-[#5A7A6B]">{event.date} at {event.time}</p>
                      </div>
                    </div>
                    <ChevronRight className="w-4 h-4 text-[#5A7A6B]" />
                  </div>
                ))}
                {events.length === 0 && (
                  <p className="text-sm text-[#5A7A6B] text-center py-2">No upcoming events</p>
                )}
              </div>
            </div>

            {/* Pending Tasks */}
            <div>
              <h4 className="text-sm font-semibold text-[#5A7A6B] mb-2 flex items-center gap-2">
                <CheckSquare className="w-4 h-4" />
                Pending Tasks
              </h4>
              <div className="space-y-2">
                {todos.filter(todo => !todo.completed).slice(0, 2).map(todo => (
                  <div key={todo.id} className="flex items-center justify-between p-3 rounded-lg hover:bg-[#F6FFF8] transition-colors">
                    <div className="flex items-center gap-3">
                      <div className="w-8 h-8 rounded-full flex items-center justify-center" style={{ background: M.bg3 }}>
                        <CheckSquare className="w-4 h-4" style={{ color: M.primary }} />
                      </div>
                      <div>
                        <p className="font-medium text-[#2C3E3F] text-sm">{todo.text}</p>
                        <p className="text-xs text-[#5A7A6B]">Pending</p>
                      </div>
                    </div>
                    <ChevronRight className="w-4 h-4 text-[#5A7A6B]" />
                  </div>
                ))}
                {todos.filter(todo => !todo.completed).length === 0 && (
                  <p className="text-sm text-[#5A7A6B] text-center py-2">No pending tasks</p>
                )}
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
}
