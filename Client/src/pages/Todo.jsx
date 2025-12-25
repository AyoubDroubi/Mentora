import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext'; 
 import { CheckSquare,Plus, Trash2, Home as HomeIcon,User as UserIcon, LayoutDashboard, User,} from 'lucide-react';

export default function Todo() {
  const navigate = useNavigate();
  const { user, setUser } = useUser();  

  
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  // To-Do state
  const [todos, setTodos] = useState([
    { id: 1, text: 'Complete web Assignment', completed: false },
    { id: 2, text: 'Read Chapter 5 - java', completed: true },
    { id: 3, text: 'exam database', completed: false },
  ]);
  const [newTodo, setNewTodo] = useState('');
  const [todoFilter, setTodoFilter] = useState('all');

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

   
    const addTodo = () => {
       if (!newTodo.trim()) return;
      setTodos((t) => [...t, { id: Date.now(), text: newTodo.trim(), completed: false }]);
      setNewTodo('');
     };
    const toggleTodo = (id) => {
      setTodos((t) => t.map(x => x.id === id ? { ...x, completed: !x.completed } : x));
     };
     const deleteTodo = (id) => setTodos((t) => t.filter(x => x.id !== id));

     const filtered = todos.filter(t => todoFilter === 'all' ? true : todoFilter === 'active' ? !t.completed : t.completed);

     return (
       <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
       <Header />
 <main className="container mx-auto px-4 mt-6">
           <div className="bg-white rounded-2xl p-4 shadow-lg border mb-4" style={{ borderColor: M.bg3 }}>
           <div className="flex gap-2">
              <input
               value={newTodo}
                 onChange={e => setNewTodo(e.target.value)}
                 placeholder="Add a new task..."
                className="flex-1 px-3 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                style={{ borderColor: M.bg3 }}
                onKeyDown={(e) => e.key === 'Enter' && addTodo()}
              />
              <button
                 onClick={addTodo}
                 className="px-4 py-2 rounded-lg text-white hover:shadow-lg transition-all"
              style={{ background: M.primary }}
               >
              <Plus className="w-5 h-5" />
               </button>
             </div>
            </div>

              <div className="flex gap-2 mb-4">
          {['all','active','completed'].map(f => (
               <button
              key={f}
              onClick={() => setTodoFilter(f)}
                 className={`px-4 py-2 rounded-lg font-medium capitalize transition-all ${todoFilter === f ? 'text-white shadow-md' : 'text-[#5A7A6B]'}`}
              style={todoFilter === f ? { background: M.primary } : { border: `1px solid ${M.bg3}` }}
              >
              {f}
               </button>
            ))}
           </div>

           <div className="space-y-3">
          {filtered.length === 0 ? (
               <div className="bg-white rounded-xl p-8 shadow-md text-center" style={{ borderColor: M.bg3 }}>
                 <CheckSquare className="w-12 h-12 mx-auto mb-3 text-[#5A7A6B]" />
                  <p className="text-[#5A7A6B]">No tasks {todoFilter !== 'all' ? `in ${todoFilter}` : 'yet'}</p>
               </div>
             ) : (
            filtered.map(t => (
                 <div key={t.id} className="bg-white rounded-xl p-4 shadow-md flex items-center gap-3 hover:shadow-lg transition-all">
                <button
                     onClick={() => toggleTodo(t.id)}
                  className={`w-6 h-6 rounded-full border-2 flex items-center justify-center transition-all ${t.completed ? 'bg-[#6B9080] border-[#6B9080]' : 'border-gray-300 hover:border-[#6B9080]'}`}
                   >
                  {t.completed && <CheckSquare className="w-4 h-4 text-white" />}
                     </button>
                <div className={`flex-1 ${t.completed ? 'line-through text-[#5A7A6B]' : 'text-[#2C3E3F] font-medium'}`}>{t.text}</div>
                   <button
                      onClick={() => deleteTodo(t.id)}
                     className="text-red-500 hover:bg-red-50 p-2 rounded-lg transition-colors"
                   >
                     <Trash2 className="w-4 h-4" />
                     </button>
  </div>
               ))
            )}
          </div>
         </main>
       </div>
  );
}
