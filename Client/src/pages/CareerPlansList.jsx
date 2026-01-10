import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import api from '../services/api';
import SharedHeader from '../components/SharedHeader';
import {
  FileText,
  ChevronRight,
  Clock,
  CheckCircle,
  Loader2,
  AlertCircle,
  Plus,
  TrendingUp,
  Calendar
} from 'lucide-react';

export default function CareerPlansList() {
  const navigate = useNavigate();
  const { user } = useUser();
  const [plans, setPlans] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  useEffect(() => {
    fetchPlans();
  }, []);

  const fetchPlans = async () => {
    try {
      setLoading(true);
      const response = await api.get('/career-plans');
      setPlans(response.data);
      setError(null);
    } catch (err) {
      console.error('Error fetching plans:', err);
      setError('Failed to load career plans');
    } finally {
      setLoading(false);
    }
  };

  const getStatusBadge = (status) => {
    const statusConfig = {
      Generated: { icon: CheckCircle, bg: 'bg-green-100', text: 'text-green-700', label: 'Generated' },
      InProgress: { icon: Clock, bg: 'bg-blue-100', text: 'text-blue-700', label: 'In Progress' },
      Completed: { icon: CheckCircle, bg: 'bg-green-100', text: 'text-green-700', label: 'Completed' },
      Outdated: { icon: AlertCircle, bg: 'bg-gray-100', text: 'text-gray-700', label: 'Outdated' }
    };

    const config = statusConfig[status] || statusConfig.Generated;
    const Icon = config.icon;

    return (
      <div className={`flex items-center gap-2 px-3 py-1 ${config.bg} ${config.text} rounded-full`}>
        <Icon className="w-4 h-4" />
        <span className="text-sm font-medium">{config.label}</span>
      </div>
    );
  };

  if (loading) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <SharedHeader title="Mentora - Career Plans" />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-3xl p-8 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <Loader2 className="w-12 h-12 animate-spin mx-auto mb-4" style={{ color: M.primary }} />
            <p className="text-lg" style={{ color: M.muted }}>Loading career plans...</p>
          </div>
        </main>
      </div>
    );
  }

  if (error) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <SharedHeader title="Mentora - Career Plans" />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-3xl p-8 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <AlertCircle className="w-12 h-12 mx-auto mb-4 text-red-500" />
            <h1 className="text-3xl font-bold mb-4" style={{ color: M.text }}>Error Loading Plans</h1>
            <p className="text-lg mb-6" style={{ color: M.muted }}>{error}</p>
            <button
              onClick={fetchPlans}
              className="px-8 py-4 rounded-lg text-white font-medium hover:shadow-lg transition-all text-lg"
              style={{ background: M.primary }}
            >
              Try Again
            </button>
          </div>
        </main>
      </div>
    );
  }

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <SharedHeader title="Mentora - Career Plans" />
      <main className="container mx-auto px-4 mt-6">
        {/* Header */}
        <div className="bg-white rounded-3xl p-8 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-4">
            <div>
              <h1 className="text-3xl font-bold mb-2" style={{ color: M.text }}>My Career Plans</h1>
              <p className="text-lg" style={{ color: M.muted }}>
                View and manage your AI-generated career development plans
              </p>
            </div>
            <button
              onClick={() => navigate('/create-career-builder')}
              className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2"
              style={{ background: M.primary }}
            >
              <Plus className="w-5 h-5" />
              New Assessment
            </button>
          </div>
        </div>

        {/* Plans List */}
        {plans.length === 0 ? (
          <div className="bg-white rounded-3xl p-12 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <FileText className="w-16 h-16 mx-auto mb-4" style={{ color: M.muted, opacity: 0.5 }} />
            <h2 className="text-2xl font-bold mb-2" style={{ color: M.text }}>No Career Plans Yet</h2>
            <p className="text-lg mb-6" style={{ color: M.muted }}>
              Take the career assessment to generate your first personalized career plan
            </p>
            <button
              onClick={() => navigate('/create-career-builder')}
              className="px-8 py-4 rounded-lg text-white font-medium hover:shadow-lg transition-all text-lg inline-flex items-center gap-2"
              style={{ background: M.primary }}
            >
              <Plus className="w-5 h-5" />
              Start Career Assessment
            </button>
          </div>
        ) : (
          <div className="grid grid-cols-1 gap-6">
            {plans.map((plan) => (
              <div
                key={plan.id}
                onClick={() => navigate(`/career-plan/${plan.id}`)}
                className="bg-white rounded-3xl p-6 shadow-lg border hover:shadow-xl transition-all cursor-pointer"
                style={{ borderColor: M.bg3 }}
              >
                <div className="flex items-start justify-between mb-4">
                  <div className="flex-1">
                    <h3 className="text-xl font-bold mb-2" style={{ color: M.text }}>{plan.title}</h3>
                    <div className="flex items-center gap-4 text-sm" style={{ color: M.muted }}>
                      <div className="flex items-center gap-2">
                        <Calendar className="w-4 h-4" />
                        <span>{new Date(plan.createdAt).toLocaleDateString()}</span>
                      </div>
                      <div className="flex items-center gap-2">
                        <TrendingUp className="w-4 h-4" />
                        <span>{plan.progressPercentage}% Complete</span>
                      </div>
                    </div>
                  </div>
                  <div className="flex items-center gap-3">
                    {getStatusBadge(plan.status)}
                    <ChevronRight className="w-5 h-5" style={{ color: M.muted }} />
                  </div>
                </div>

                {/* Progress Bar */}
                <div className="w-full bg-gray-200 rounded-full h-2">
                  <div
                    className="h-2 rounded-full transition-all duration-300"
                    style={{
                      width: `${plan.progressPercentage}%`,
                      backgroundColor: M.primary
                    }}
                  />
                </div>
              </div>
            ))}
          </div>
        )}

        {/* Back Button */}
        <div className="flex justify-center mt-6">
          <button
            onClick={() => navigate('/career-builder')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.secondary }}
          >
            Back to Career Builder
          </button>
        </div>
      </main>
    </div>
  );
}
