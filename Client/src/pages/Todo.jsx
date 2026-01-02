import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import { CheckSquare, Plus, Trash2 } from 'lucide-react';
import { todoService } from '../services';

export default function Todo() {
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
  const [todos, setTodos] = useState([]);
  const [newTodo, setNewTodo] = useState('');
  const [todoFilter, setTodoFilter] = useState('all');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [summary, setSummary] = useState(null);

  // Fetch todos on mount and when filter changes
  useEffect(() => {
    fetchTodos();
    fetchSummary();
  }, [todoFilter]);

  // Fetch todos from API
  const fetchTodos = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await todoService.getAllTodos(todoFilter);
      
      if (response.success) {
        setTodos(response.data || []);
      }
    } catch (err) {
      console.error('Error fetching todos:', err);
      setError(err.response?.data?.message || 'Failed to load todos');
    } finally {
      setLoading(false);
    }
  };

  // Fetch summary
  const fetchSummary = async () => {
    try {
      const response = await todoService.getSummary();
      if (response.success) {
        setSummary(response.data);
      }
    } catch (err) {
      console.error('Error fetching summary:', err);
    }
  };

  // Add todo
  const addTodo = async () => {
    if (!newTodo.trim()) return;

    try {
      setLoading(true);
      setError(null);
      const response = await todoService.createTodo(newTodo.trim());
      
      if (response.success) {
        setTodos([response.data, ...todos]);
        setNewTodo('');
        fetchSummary(); // Refresh summary
      }
    } catch (err) {
      console.error('Error creating todo:', err);
      setError(err.response?.data?.message || 'Failed to create todo');
    } finally {
      setLoading(false);
    }
  };

  // Toggle todo completion
  const toggleTodo = async (id) => {
    try {
      const response = await todoService.toggleTodo(id);
      
      if (response.success) {
        setTodos(todos.map(t => t.id === id ? response.data : t));
        fetchSummary(); // Refresh summary
      }
    } catch (err) {
      console.error('Error toggling todo:', err);
      setError(err.response?.data?.message || 'Failed to update todo');
    }
  };

  // Delete todo
  const deleteTodo = async (id) => {
    if (!window.confirm('Are you sure you want to delete this task?')) return;

    try {
      const response = await todoService.deleteTodo(id);
      
      if (response.success) {
        setTodos(todos.filter(t => t.id !== id));
        fetchSummary(); // Refresh summary
      }
    } catch (err) {
      console.error('Error deleting todo:', err);
      setError(err.response?.data?.message || 'Failed to delete todo');
    }
  };

  const Header = () => (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <CheckSquare className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">Mentora - Todo</span>
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
        {/* Summary Card */}
        {summary && (
          <div className="bg-white rounded-2xl p-4 shadow-lg border mb-4" style={{ borderColor: M.bg3 }}>
            <h3 className="font-semibold text-lg mb-3" style={{ color: M.text }}>Summary</h3>
            <div className="grid grid-cols-4 gap-4 text-center">
              <div>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{summary.totalTasks}</p>
                <p className="text-sm" style={{ color: M.muted }}>Total</p>
              </div>
              <div>
                <p className="text-2xl font-bold text-green-600">{summary.completedTasks}</p>
                <p className="text-sm" style={{ color: M.muted }}>Completed</p>
              </div>
              <div>
                <p className="text-2xl font-bold text-orange-600">{summary.pendingTasks}</p>
                <p className="text-sm" style={{ color: M.muted }}>Pending</p>
              </div>
              <div>
                <p className="text-2xl font-bold" style={{ color: M.primary }}>{summary.completionRate}%</p>
                <p className="text-sm" style={{ color: M.muted }}>Rate</p>
              </div>
            </div>
          </div>
        )}

        {/* Error Message */}
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded-lg mb-4">
            {error}
            <button onClick={() => setError(null)} className="float-right font-bold">×</button>
          </div>
        )}

        {/* Add Todo */}
        <div className="bg-white rounded-2xl p-4 shadow-lg border mb-4" style={{ borderColor: M.bg3 }}>
          <div className="flex gap-2">
            <input
              value={newTodo}
              onChange={(e) => setNewTodo(e.target.value)}
              placeholder="Add a new task... (e.g., Complete assignment)"
              className="flex-1 px-3 py-2 rounded-lg border focus:outline-none focus:ring-2 focus:ring-[#6B9080]"
              style={{ borderColor: M.bg3 }}
              onKeyDown={(e) => e.key === 'Enter' && !loading && addTodo()}
              disabled={loading}
            />
            <button
              onClick={addTodo}
              disabled={loading}
              className="px-4 py-2 rounded-lg text-white hover:shadow-lg transition-all disabled:opacity-50"
              style={{ background: M.primary }}
            >
              {loading ? '...' : <Plus className="w-5 h-5" />}
            </button>
          </div>
        </div>

        {/* Filter Buttons */}
        <div className="flex gap-2 mb-4">
          {['all', 'active', 'completed'].map((f) => (
            <button
              key={f}
              onClick={() => setTodoFilter(f)}
              className={`px-4 py-2 rounded-lg font-medium capitalize transition-all ${
                todoFilter === f ? 'text-white shadow-md' : 'text-[#5A7A6B]'
              }`}
              style={todoFilter === f ? { background: M.primary } : { border: `1px solid ${M.bg3}` }}
            >
              {f}
            </button>
          ))}
        </div>

        {/* Todo List */}
        {loading && <p className="text-center py-8" style={{ color: M.muted }}>Loading...</p>}

        {!loading && todos.length === 0 && (
          <div className="bg-white rounded-xl p-8 shadow-md text-center" style={{ borderColor: M.bg3 }}>
            <CheckSquare className="w-12 h-12 mx-auto mb-3 text-[#5A7A6B]" />
            <p className="text-[#5A7A6B]">No tasks {todoFilter !== 'all' ? `in ${todoFilter}` : 'yet'}</p>
            <p className="text-sm text-gray-400 mt-2">Create your first task above!</p>
          </div>
        )}

        <div className="space-y-3">
          {!loading && todos.map((todo) => (
            <div
              key={todo.id}
              className="bg-white rounded-xl p-4 shadow-md flex items-center gap-3 hover:shadow-lg transition-all"
            >
              <button
                onClick={() => toggleTodo(todo.id)}
                className={`w-6 h-6 rounded-full border-2 flex items-center justify-center transition-all ${
                  todo.isCompleted ? 'bg-[#6B9080] border-[#6B9080]' : 'border-gray-300 hover:border-[#6B9080]'
                }`}
              >
                {todo.isCompleted && <CheckSquare className="w-4 h-4 text-white" />}
              </button>
              <div className={`flex-1 ${todo.isCompleted ? 'line-through text-[#5A7A6B]' : 'text-[#2C3E3F] font-medium'}`}>
                {todo.title}
              </div>
              <button
                onClick={() => deleteTodo(todo.id)}
                className="text-red-500 hover:bg-red-50 p-2 rounded-lg transition-colors"
              >
                <Trash2 className="w-4 h-4" />
              </button>
            </div>
          ))}
        </div>
      </main>
    </div>
  );
}
