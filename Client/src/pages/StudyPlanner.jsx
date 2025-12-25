// StudyPlanner.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import {BookOpen, CheckSquare, BookMarked,   Clock, Calendar, HelpCircle,ClipboardCheck, Flame,} from 'lucide-react';

export default function StudyPlanner() {
   const navigate = useNavigate();
 const { user } = useUser();

 
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  // Header  
  const Header = () => (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="flex items-center gap-3">
   
          <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
           <BookOpen className="w-6 h-6" style={{ color: M.primary }} />
         </div>
          <span className="text-white text-xl font-bold">Mentora</span>
        </div>

      <div className="flex items-center gap-4">
           <nav className="flex items-center gap-4">
            <button
             onClick={() => navigate('/study-planner')}
               className="text-white font-medium hover:bg-white/20 hover:text-white transition-all duration-300 px-3 py-2 rounded-lg hidden md:block"
           >
             Study Planner
 </button>
           <button
              onClick={() => navigate('/career-builder')}
             className="text-white font-medium hover:bg-white/20 hover:text-white transition-all duration-300 px-3 py-2 rounded-lg hidden md:block"
           >
             Career Builder
           </button>
           <button
             onClick={() => navigate('/dashboard')}
             className="text-white font-medium hover:bg-white/20 hover:text-white transition-all duration-300 px-3 py-2 rounded-lg hidden md:block"
            >
              Dashboard
           </button>
         </nav>
         <img
         onClick={() => navigate('/profile')}
           src={user.avatar}
           alt="Profile"
  className="w-10 h-10 rounded-full border-2 border-white hover:scale-110 hover:opacity-90 transition-all cursor-pointer"
    />
       </div>
     </header>
  );

  return (
      <div
  style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }}
       className="min-h-screen pb-24 text-[#2C3E3F]"
     >
      <Header />
      <main className="container mx-auto px-4 mt-6">
        {/* Welcome Section */}
        <section
          className="bg-white rounded-3xl p-6 shadow-lg border mb-6"
          style={{ borderColor: M.bg3, background: `linear-gradient(135deg, ${M.bg1}, white)` }}
        >  
          <h1 className="text-3xl font-bold text-[#2C3E3F] mb-2">Welcome to Mentora</h1>
         <p className="text-[#5A7A6B] mb-4">Your personal study companion for academic excellence</p>
          <div className="flex gap-3">
               <div className="flex items-center gap-2 px-4 py-2 rounded-lg bg-white shadow-sm">
              <Flame className="w-5 h-5 text-orange-500" />
              <span className="font-semibold">{user.studyStreak} day streak</span>
             </div>
             <div className="flex items-center gap-2 px-4 py-2 rounded-lg bg-white shadow-sm">
                <Clock className="w-5 h-5" style={{ color: M.primary }} />
               <span className="font-semibold">{user.totalHours}h studied</span>
             </div>
           </div>
           </section>

          {/* Study Tools */}
        <section className="bg-white rounded-3xl p-6 shadow-lg border" style={{ borderColor: M.bg3 }}>
           <h2 className="text-xl font-bold text-[#2C3E3F] mb-4">Study Tools</h2>
          <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
              <ToolButton
              icon={<CheckSquare className="w-8 h-8" style={{ color: M.primary }} />}
               title="To-Do"
              subtitle={`${user.todosPending} pending`}
                  onClick={() => navigate('/todo')}
              borderColor={M.bg3}
             />
            <ToolButton
 icon={<BookMarked className="w-8 h-8" style={{ color: M.primary }} />}
              title="Notes"
                  subtitle={`${user.notesCount} saved`}
              onClick={() => navigate('/notes')}
               borderColor={M.bg3}
            />
             <ToolButton
              icon={<Clock className="w-8 h-8" style={{ color: M.primary }} />}
                title="Timer"
                 subtitle="Pomodoro"
                 onClick={() => navigate('/timer')}
                 borderColor={M.bg3}
            />
              <ToolButton
                icon={<Calendar className="w-8 h-8" style={{ color: M.primary }} />}
                title="Planner"
                 subtitle={`${user.eventsCount} events`}
                onClick={() => navigate('/planner')}
                borderColor={M.bg3}
              />
             <ToolButton
               icon={<ClipboardCheck className="w-8 h-8" style={{ color: M.primary }} />}
               title="Attendance"
                subtitle={`${user.attendanceRate}% present`}
               onClick={() => navigate('/attendance')}
                borderColor={M.bg3}
             />
            <ToolButton
                icon={<HelpCircle className="w-8 h-8" style={{ color: M.primary }} />}
                  title="Quiz"
                subtitle="AI Assessment"
               onClick={() => navigate('/quiz')}
              borderColor={M.bg3}
            />
             </div>
        </section>
      </main>
    </div>
  );
}

function ToolButton({ icon, title, subtitle, onClick, borderColor }) {
  return (
    <button
       onClick={onClick}
        className="flex flex-col items-center gap-2 p-4 rounded-2xl border hover:shadow-md hover:scale-105 transition-all"
        style={{ borderColor }}
    >
        {icon}
        <span className="text-sm font-medium">{title}</span>
         <span className="text-xs text-[#5A7A6B]">{subtitle}</span>
        </button>
    );
  }
