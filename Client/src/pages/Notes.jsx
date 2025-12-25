import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import { BookMarked, Calendar, Trash2, Save, Home as HomeIcon, LayoutDashboard, User as UserIcon,} from 'lucide-react';

export default function Notes() {
  const navigate = useNavigate();
  const { user } = useUser();
  const [isLoggedIn, setIsLoggedIn] = useState(true);
  

  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
     text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const [notes, setNotes] = useState([
    { id: 1, title: 'web eaxm', content: 'Html,css', date: '2024-12-15' },
  ]);
    const [noteTitle, setNoteTitle] = useState('');
     const [noteContent, setNoteContent] = useState('');

  const addNote = () => {
    if (!noteTitle.trim() || !noteContent.trim()) return;
       setNotes(n => [{ id: Date.now(), title: noteTitle.trim(), content: noteContent.trim(), date: new Date().toISOString().split('T')[0] }, ...n]);
        setNoteTitle('');
      setNoteContent('');
  };
  const deleteNote = (id) => setNotes(n => n.filter(x => x.id !== id));



  const Header = () => (
    <header className="px-6 py-4 flex items-center justify-between shadow-lg"
       style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}>
         <div className="flex items-center gap-3">
           <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
             <BookMarked className="w-6 h-6" style={{ color: M.primary }} />
           </div>
           <span className="text-white text-xl font-bold">Mentora - Notes</span>
         </div>
         <button
             onClick={() => navigate('/profile')}
             className="ml-4 w-10 h-10 rounded-full border-2 border-white overflow-hidden hover:scale-110 transition-transform"
             title="Open profile"
              >
               <img src={user.avatar} alt="avatar" className="w-full h-full object-cover" />
            </button>
      </header>
    );

    return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <Header />
      <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-2xl p-4 shadow-lg border mb-4" style={{ borderColor: M.bg3 }}>
          <input
               placeholder="Note title..."
              value={noteTitle}
             onChange={e => setNoteTitle(e.target.value)}
             className="w-full px-3 py-2 rounded-lg border mb-3 focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
                style={{ borderColor: M.bg3 }}
 />
            <textarea
             placeholder="Write your note..."
               value={noteContent}
             onChange={e => setNoteContent(e.target.value)}
               rows={4}
             className="w-full px-3 py-2 rounded-lg border mb-3 focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
            style={{ borderColor: M.bg3 }}
            />
           <div className="flex gap-2">
               <button
                onClick={addNote}
               className="px-6 py-2 rounded-lg text-white flex items-center gap-2 hover:shadow-lg transition-all font-medium"
              style={{ background: M.primary }}
                 >
              <Save className="w-4 h-4" /> Save Note
                 </button>
             <button
 onClick={() => { setNoteTitle(''); setNoteContent(''); }}
                className="px-6 py-2 rounded-lg border font-medium hover:shadow-md transition-all"
                 style={{ borderColor: M.bg3 }}
            >
             </button>
          </div>
        </div>

          <div className="grid gap-4">
   {notes.length === 0 ? (
 <div className="bg-white rounded-xl p-8 shadow-md text-center">
                <BookMarked className="w-12 h-12 mx-auto mb-3 text-[#5A7A6B]" />
                <p className="text-[#5A7A6B]">No notes yet. Create your first note above!</p>
              </div>
            ) : (
               notes.map(n => (
                 <div key={n.id} className="bg-white rounded-xl p-5 shadow-md border hover:shadow-lg transition-all" style={{ borderColor: M.bg3 }}>
                   <div className="flex items-start justify-between mb-3">
                     <div className="flex-1">
                       <h4 className="font-bold text-[#2C3E3F] text-lg">{n.title}</h4>
                       <p className="text-xs text-[#5A7A6B] flex items-center gap-1 mt-1">
                         <Calendar className="w-3 h-3" />
                         {n.date}
                       </p>
                     </div>
                     <button
                       onClick={() => deleteNote(n.id)}
                        className="text-red-500 hover:bg-red-50 p-2 rounded-lg transition-colors"
                     >
                       <Trash2 className="w-4 h-4" />
                      </button>
                   </div>
                  <p className="text-[#5A7A6B]">{n.content}</p>
                </div>
              ))
           )}
            </div>
         </main>
    </div>
        );
}
