import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import { BookMarked, Calendar, Trash2, Save, Edit } from 'lucide-react';
import { notesService } from '../services';

export default function Notes() {
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

  // State
  const [notes, setNotes] = useState([]);
  const [noteTitle, setNoteTitle] = useState('');
  const [noteContent, setNoteContent] = useState('');
  const [editingId, setEditingId] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Fetch notes on mount
  useEffect(() => {
    fetchNotes();
  }, []);

  // Fetch all notes
  const fetchNotes = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await notesService.getAllNotes();

      if (response.success) {
        setNotes(response.data || []);
      }
    } catch (err) {
      console.error('Error fetching notes:', err);
      setError(err.response?.data?.message || 'Failed to load notes');
    } finally {
      setLoading(false);
    }
  };

  // Add or update note
  const saveNote = async () => {
    if (!noteTitle.trim() || !noteContent.trim()) {
      setError('Title and content are required');
      return;
    }

    try {
      setLoading(true);
      setError(null);

      const noteData = {
        title: noteTitle.trim(),
        content: noteContent.trim()
      };

      let response;
      if (editingId) {
        // Update existing note
        response = await notesService.updateNote(editingId, noteData);
        if (response.success) {
          setNotes(notes.map(n => n.id === editingId ? response.data : n));
        }
      } else {
        // Create new note
        response = await notesService.createNote(noteData);
        if (response.success) {
          setNotes([response.data, ...notes]);
        }
      }

      // Clear form
      setNoteTitle('');
      setNoteContent('');
      setEditingId(null);
    } catch (err) {
      console.error('Error saving note:', err);
      setError(err.response?.data?.message || 'Failed to save note');
    } finally {
      setLoading(false);
    }
  };

  // Edit note (load into form)
  const editNote = (note) => {
    setNoteTitle(note.title);
    setNoteContent(note.content);
    setEditingId(note.id);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  };

  // Cancel editing
  const cancelEdit = () => {
    setNoteTitle('');
    setNoteContent('');
    setEditingId(null);
    setError(null);
  };

  // Delete note
  const deleteNote = async (id) => {
    if (!window.confirm('Are you sure you want to delete this note?')) return;

    try {
      const response = await notesService.deleteNote(id);

      if (response.success) {
        setNotes(notes.filter(n => n.id !== id));
        
        // Clear form if deleting currently editing note
        if (editingId === id) {
          cancelEdit();
        }
      }
    } catch (err) {
      console.error('Error deleting note:', err);
      setError(err.response?.data?.message || 'Failed to delete note');
    }
  };

  // Format date for display
  const formatDate = (isoString) => {
    const date = new Date(isoString);
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  const Header = () => (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <BookMarked className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">Mentora - Notes</span>
      </div>

      <nav className="flex items-center gap-4">
        <button
          onClick={() => navigate('/study-planner')}
          className="text-white hover:bg-white/20 px-3 py-2 rounded-lg transition-colors"
        >
          Dashboard
        </button>
        <button
          onClick={() => navigate('/profile')}
          className="w-10 h-10 rounded-full border-2 border-white overflow-hidden hover:scale-110 transition-transform"
          title="Open profile"
        >
          <img src={user?.avatar || 'https://api.dicebear.com/7.x/avataaars/svg?seed=default'} alt="avatar" className="w-full h-full object-cover" />
        </button>
      </nav>
    </header>
  );

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <Header />
      <main className="container mx-auto px-4 mt-6">
        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-4">
            {error}
            <button onClick={() => setError(null)} className="float-right font-bold">×</button>
          </div>
        )}

        {/* Add/Edit Note Form */}
        <div className="bg-white rounded-2xl p-4 shadow-lg border mb-4" style={{ borderColor: M.bg3 }}>
          {editingId && (
            <div className="bg-blue-50 border border-blue-200 text-blue-700 px-3 py-2 rounded-lg mb-3 text-sm">
              Editing note - Click Cancel to create new note instead
            </div>
          )}
          <input
            placeholder="Note title..."
            value={noteTitle}
            onChange={(e) => setNoteTitle(e.target.value)}
            className="w-full px-3 py-2 rounded-lg border mb-3 focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
            style={{ borderColor: M.bg3 }}
            disabled={loading}
          />
          <textarea
            placeholder="Write your note... (supports Arabic text)"
            value={noteContent}
            onChange={(e) => setNoteContent(e.target.value)}
            rows={4}
            className="w-full px-3 py-2 rounded-lg border mb-3 focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
            style={{ borderColor: M.bg3 }}
            disabled={loading}
          />
          <div className="flex gap-2">
            <button
              onClick={saveNote}
              disabled={loading}
              className="px-6 py-2 rounded-lg text-white flex items-center gap-2 hover:shadow-lg transition-all font-medium disabled:opacity-50"
              style={{ background: M.primary }}
            >
              <Save className="w-4 h-4" />
              {loading ? 'Saving...' : editingId ? 'Update Note' : 'Save Note'}
            </button>
            {editingId && (
              <button
                onClick={cancelEdit}
                className="px-6 py-2 rounded-lg border font-medium hover:shadow-md transition-all"
                style={{ borderColor: M.bg3 }}
              >
                Cancel
              </button>
            )}
          </div>
        </div>

        {/* Notes List */}
        {loading && <p className="text-center py-8" style={{ color: M.muted }}>Loading...</p>}

        {!loading && notes.length === 0 && (
          <div className="bg-white rounded-xl p-8 shadow-md text-center">
            <BookMarked className="w-12 h-12 mx-auto mb-3 text-[#5A7A6B]" />
            <p className="text-[#5A7A6B]">No notes yet. Create your first note above!</p>
          </div>
        )}

        <div className="grid gap-4">
          {!loading && notes.map((note) => (
            <div
              key={note.id}
              className={`bg-white rounded-xl p-5 shadow-md border hover:shadow-lg transition-all ${
                editingId === note.id ? 'ring-2 ring-blue-400' : ''
              }`}
              style={{ borderColor: M.bg3 }}
            >
              <div className="flex items-start justify-between mb-3">
                <div className="flex-1">
                  <h4 className="font-bold text-[#2C3E3F] text-lg">{note.title}</h4>
                  <p className="text-xs text-[#5A7A6B] flex items-center gap-1 mt-1">
                    <Calendar className="w-3 h-3" />
                    {formatDate(note.createdAt)}
                    {note.updatedAt && note.updatedAt !== note.createdAt && (
                      <span className="ml-2">(updated: {formatDate(note.updatedAt)})</span>
                    )}
                  </p>
                </div>
                <div className="flex gap-2">
                  <button
                    onClick={() => editNote(note)}
                    className="text-blue-500 hover:bg-blue-50 p-2 rounded-lg transition-colors"
                    title="Edit note"
                  >
                    <Edit className="w-4 h-4" />
                  </button>
                  <button
                    onClick={() => deleteNote(note.id)}
                    className="text-red-500 hover:bg-red-50 p-2 rounded-lg transition-colors"
                    title="Delete note"
                  >
                    <Trash2 className="w-4 h-4" />
                  </button>
                </div>
              </div>
              <p className="text-[#5A7A6B] whitespace-pre-wrap">{note.content}</p>
            </div>
          ))}
        </div>
      </main>
    </div>
  );
}
