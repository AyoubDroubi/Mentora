
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import {
  Clock,
  Play,
  Pause,
  RotateCcw,
  Home as HomeIcon,
  User as UserIcon,
  LayoutDashboard,
  User,CheckSquare,
} from 'lucide-react';

export default function Timer() {
  const navigate = useNavigate();

   
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  // Timer state
  const [timerMinutes, setTimerMinutes] = useState(25);
  const [timerSeconds, setTimerSeconds] = useState(0);
  const [isTimerRunning, setIsTimerRunning] = useState(false);
  const [timerMode, setTimerMode] = useState('focus');

    const [user, setUser] = useState({
     name: 'Bijaya Kumar Behera',
     email: 'kumarbijayebehera07@gmail.com',
     avatar: 'https://api.dicebear.com/7.x/avataaars/svg?seed=Bijaya',
  });
  const [isLoggedIn, setIsLoggedIn] = useState(true);

  

  const Header = () => (
    <header className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}>
      <div className="flex items-center gap-3">
          <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
           <CheckSquare className="w-6 h-6" style={{ color: M.primary }} />
         </div>
          <span className="text-white text-xl font-bold">Mentora - Todo</span>
         </div>

         <nav className="flex items-center gap-4">
           {isLoggedIn ? (
 <button onClick={() => navigate('/profile')}
               className="ml-4 w-10 h-10 rounded-full border-2 border-white overflow-hidden hover:scale-110 transition-transform"
            title="Open profile"
      >
               <img src={user.avatar} alt="avatar" className="w-full h-full object-cover" />
            </button>
           ) : (
             <button onClick={() => navigate('/login')} className="ml-4 bg-white text-[#6B9080] px-4 py-2 rounded-lg font-medium hover:shadow-lg transition-shadow">Login</button>
           )}
          </nav>
        </header>
     );

  // Timer effect
  useEffect(() => {
    let interval = null;
    if (isTimerRunning) {
      interval = setInterval(() => {
        setTimerSeconds((s) => {
          if (s === 0) {
             if (timerMinutes === 0) {
                setIsTimerRunning(false);
              alert('Timer completed! 🎉');
                return 0;
              } else {
                setTimerMinutes((m) => m - 1);
              return 59;
              }
            }
            return s - 1;
        });
      }, 1000);
     }
     return () => clearInterval(interval);
    }, [isTimerRunning, timerMinutes]);

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <Header />
      <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-2xl p-8 shadow-lg border" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold text-center text-[#2C3E3F] mb-6">Pomodoro Timer</h2>
 
          <div className="flex justify-center gap-3 mb-8">
              {[
              {mode:'focus',label:'Focus (25m)', time: 25},
                {mode:'short',label:'Short (5m)', time: 5},
              {mode:'long',label:'Long (15m)', time: 15}
              ].map(it => (
              <button
                  key={it.mode}
                onClick={() => {
                    setTimerMode(it.mode);
                  setIsTimerRunning(false);
                    setTimerMinutes(it.time);
                  setTimerSeconds(0);
                   }}
                className={`px-6 py-3 rounded-lg font-medium transition-all ${timerMode===it.mode ? 'text-white shadow-md' : 'text-[#5A7A6B] hover:bg-[#F6FFF8]'}`}
                  style={timerMode===it.mode ? { background: M.primary } : { border: `1px solid ${M.bg3}` }}
              >
                   {it.label}
              </button>
            ))}
          </div>

          <div className="text-center mb-8">
              <div className="text-7xl font-bold text-[#2C3E3F] mb-4 font-mono">
              {String(timerMinutes).padStart(2,'0')}:{String(timerSeconds).padStart(2,'0')}
            </div>
               <p className="text-lg text-[#5A7A6B]">
              {timerMode === 'focus' ? '🎯 Time to focus!' : '☕ Take a break!'}
              </p>
          </div>

           <div className="flex justify-center gap-4 mb-6">
            {!isTimerRunning ? (
              <button
                onClick={() => setIsTimerRunning(true)}
                className="px-8 py-4 rounded-xl text-white flex items-center gap-2 text-lg font-medium hover:shadow-lg transition-all"
                   style={{ background: M.primary }}
              >
                  <Play className="w-5 h-5" /> Start
              </button>
              ) : (
              <button
                  onClick={() => setIsTimerRunning(false)}
                className="px-8 py-4 rounded-xl text-white flex items-center gap-2 text-lg font-medium hover:shadow-lg transition-all"
                  style={{ background: M.secondary }}
              >
                <Pause className="w-5 h-5" /> Pause
                 </button>
            )}
            <button
              onClick={() => {
                setIsTimerRunning(false);
                   setTimerMinutes(timerMode==='focus'?25:timerMode==='short'?5:15);
                setTimerSeconds(0);
              }}
               className="px-8 py-4 rounded-xl border font-medium hover:shadow-md transition-all flex items-center gap-2"
              style={{ borderColor: M.bg3 }}
            >
               <RotateCcw className="w-5 h-5" /> Reset
            </button>
          </div>
  
          <div className="border-t pt-6" style={{ borderColor: M.bg3 }}>
            <p className="text-center text-sm text-[#5A7A6B] mb-3">Custom Time</p>
             <div className="flex justify-center items-center gap-3">
              <button
                onClick={() => setTimerMinutes(m => Math.max(0,m-5))}
                 className="px-4 py-2 rounded-lg text-white font-medium hover:shadow-lg transition-all"
                style={{ background: M.secondary }}
              >
                 -5m
              </button>
              <input
                 type="number"
                min="0"
                value={timerMinutes}
                  onChange={(e) => {
                  setTimerMinutes(Math.max(0, Number(e.target.value)));
                  setTimerSeconds(0);
                  }}
 className="w-24 px-3 py-2 text-center rounded-lg border text-lg font-medium focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                 style={{ borderColor: M.bg3 }}
                />
                <button
                 onClick={() => setTimerMinutes(m => m + 5)}
                 className="px-4 py-2 rounded-lg text-white font-medium hover:shadow-lg transition-all"
                style={{ background: M.primary }}
              >
                 +5m
               </button>
                 </div>
          </div>
          </div>
      </main>
       
    </div>
  );
}
