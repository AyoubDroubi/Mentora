import React, { useState, useEffect } from 'react';
import { todoService } from '../services';

/**
 * Example Todo Component
 * Demonstrates how to use todoService with the new DDD architecture
 */
const TodoExample = () => {
  const [todos, setTodos] = useState([]);
  const [summary, setSummary] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [newTodoTitle, setNewTodoTitle] = useState('');

  // Fetch todos on component mount
  useEffect(() => {
    fetchTodos();
    fetchSummary();
  }, []);

  // Fetch all todos
  const fetchTodos = async (filter = 'all') => {
    try {
      setLoading(true);
      setError(null);
      const response = await todoService.getAllTodos(filter);
      
      if (response.success) {
        setTodos(response.data);
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to fetch todos');
      console.error('Error fetching todos:', err);
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

  // Create new todo
  const handleCreateTodo = async (e) => {
    e.preventDefault();
    
    if (!newTodoTitle.trim()) {
      setError('Todo title is required');
      return;
    }

    try {
      setLoading(true);
      setError(null);
      const response = await todoService.createTodo(newTodoTitle);
      
      if (response.success) {
        setTodos([response.data, ...todos]);
        setNewTodoTitle('');
        fetchSummary(); // Refresh summary
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to create todo');
      console.error('Error creating todo:', err);
    } finally {
      setLoading(false);
    }
  };

  // Toggle todo completion
  const handleToggleTodo = async (id) => {
    try {
      const response = await todoService.toggleTodo(id);
      
      if (response.success) {
        setTodos(todos.map(todo => 
          todo.id === id ? response.data : todo
        ));
        fetchSummary(); // Refresh summary
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to toggle todo');
      console.error('Error toggling todo:', err);
    }
  };

  // Delete todo
  const handleDeleteTodo = async (id) => {
    if (!window.confirm('Are you sure you want to delete this todo?')) {
      return;
    }

    try {
      const response = await todoService.deleteTodo(id);
      
      if (response.success) {
        setTodos(todos.filter(todo => todo.id !== id));
        fetchSummary(); // Refresh summary
      }
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to delete todo');
      console.error('Error deleting todo:', err);
    }
  };

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Todo List</h1>

      {/* Summary */}
      {summary && (
        <div className="bg-blue-100 p-4 rounded mb-4">
          <h2 className="font-semibold mb-2">Summary</h2>
          <div className="grid grid-cols-3 gap-4">
            <div>
              <p className="text-sm text-gray-600">Total Tasks</p>
              <p className="text-xl font-bold">{summary.totalTasks}</p>
            </div>
            <div>
              <p className="text-sm text-gray-600">Completed</p>
              <p className="text-xl font-bold">{summary.completedTasks}</p>
            </div>
            <div>
              <p className="text-sm text-gray-600">Completion Rate</p>
              <p className="text-xl font-bold">{summary.completionRate}%</p>
            </div>
          </div>
        </div>
      )}

      {/* Error Message */}
      {error && (
        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
          {error}
        </div>
      )}

      {/* Create Todo Form */}
      <form onSubmit={handleCreateTodo} className="mb-4 flex gap-2">
        <input
          type="text"
          value={newTodoTitle}
          onChange={(e) => setNewTodoTitle(e.target.value)}
          placeholder="Add a new todo..."
          className="flex-1 px-4 py-2 border rounded"
          disabled={loading}
        />
        <button
          type="submit"
          disabled={loading}
          className="bg-blue-500 text-white px-6 py-2 rounded hover:bg-blue-600 disabled:opacity-50"
        >
          {loading ? 'Adding...' : 'Add'}
        </button>
      </form>

      {/* Filter Buttons */}
      <div className="mb-4 flex gap-2">
        <button
          onClick={() => fetchTodos('all')}
          className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
        >
          All
        </button>
        <button
          onClick={() => fetchTodos('active')}
          className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
        >
          Active
        </button>
        <button
          onClick={() => fetchTodos('completed')}
          className="px-4 py-2 bg-gray-200 rounded hover:bg-gray-300"
        >
          Completed
        </button>
      </div>

      {/* Todo List */}
      {loading && <p>Loading...</p>}
      
      {!loading && todos.length === 0 && (
        <p className="text-gray-500 text-center py-8">No todos yet. Create one!</p>
      )}

      <div className="space-y-2">
        {todos.map((todo) => (
          <div
            key={todo.id}
            className="flex items-center gap-3 p-4 bg-white border rounded hover:shadow"
          >
            <input
              type="checkbox"
              checked={todo.isCompleted}
              onChange={() => handleToggleTodo(todo.id)}
              className="w-5 h-5"
            />
            <span
              className={`flex-1 ${
                todo.isCompleted ? 'line-through text-gray-400' : ''
              }`}
            >
              {todo.title}
            </span>
            <button
              onClick={() => handleDeleteTodo(todo.id)}
              className="text-red-500 hover:text-red-700"
            >
              Delete
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default TodoExample;
