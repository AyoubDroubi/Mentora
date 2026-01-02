import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import axios from 'axios';
import {
  Target,
  ChevronRight,
  Award,
  CheckSquare,
  BookOpen,
  TrendingUp,
  Clock,
  BookMarked,
  RotateCcw,
  ArrowLeft,
  ArrowRight,
  Brain,
  Home as HomeIcon,
  User as UserIcon,
  LayoutDashboard,
  FileText,
  Download,
  Lightbulb,
  ListChecks,
  Zap,
  FileText as FileTextIcon,
  ExternalLink,
  BarChart3,
  CheckCircle,
  AlertCircle,
  Clock as ClockIcon,
  Star,
  Code,
  Users,
  Briefcase,
  GraduationCap,
  TrendingUp as TrendingUpIcon,
  Loader2,
} from 'lucide-react';

const API_URL = import.meta.env.VITE_API_URL || 'https://localhost:7000/api';

export default function CareerPlan() {
  const navigate = useNavigate();
  const { id } = useParams();
  const { user } = useUser();
  const [careerPlan, setCareerPlan] = useState(null);
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
    fetchCareerPlan();
  }, [id]);

  const fetchCareerPlan = async () => {
    try {
      setLoading(true);
      const token = localStorage.getItem('accessToken');
      const response = await axios.get(`${API_URL}/career-plans/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
      });
      setCareerPlan(response.data);
      setError(null);
    } catch (err) {
      console.error('Error fetching career plan:', err);
      setError(err.response?.data?.message || 'Failed to load career plan');
    } finally {
      setLoading(false);
    }
  };

  const Header = () => (
    <header
      className="px-6 py-4 flex items-center justify-between shadow-lg"
      style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
    >
      <div className="flex items-center gap-3">
        <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
          <BookOpen className="w-6 h-6" style={{ color: M.primary }} />
        </div>
        <span className="text-white text-xl font-bold">Mentora - Career Plan</span>
      </div>

      <div className="flex items-center gap-4">
        <img
          onClick={() => navigate('/profile')}
          src={user?.avatar || '/default-avatar.png'}
          alt="Profile"
          className="w-10 h-10 rounded-full border-2 border-white hover:scale-110 hover:opacity-90 transition-all cursor-pointer"
        />
      </div>
    </header>
  );

  const getStatusBadge = (status) => {
    const statusConfig = {
      Generated: {
        icon: CheckCircle,
        bg: 'bg-green-100',
        text: 'text-green-700',
        label: 'Generated'
      },
      InProgress: {
        icon: ClockIcon,
        bg: 'bg-blue-100',
        text: 'text-blue-700',
        label: 'In Progress'
      },
      Completed: {
        icon: CheckCircle,
        bg: 'bg-green-100',
        text: 'text-green-700',
        label: 'Completed'
      },
      Archived: {
        icon: AlertCircle,
        bg: 'bg-gray-100',
        text: 'text-gray-700',
        label: 'Archived'
      }
    };

    const config = statusConfig[status] || statusConfig.Generated;
    const Icon = config.icon;

    return (
      <div className={`flex items-center gap-2 px-4 py-2 ${config.bg} ${config.text} rounded-full`}>
        <Icon className="w-4 h-4" />
        <span className="text-sm font-medium">{config.label}</span>
      </div>
    );
  };

  const getStepIcon = (index) => {
    const icons = [Lightbulb, Code, TrendingUp, Briefcase];
    return icons[index] || Target;
  };

  if (loading) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <Header />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-3xl p-8 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <Loader2 className="w-12 h-12 animate-spin mx-auto mb-4" style={{ color: M.primary }} />
            <p className="text-lg" style={{ color: M.muted }}>Loading career plan...</p>
          </div>
        </main>
      </div>
    );
  }

  if (error || !careerPlan) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <Header />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-3xl p-8 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <AlertCircle className="w-12 h-12 mx-auto mb-4 text-red-500" />
            <h1 className="text-3xl font-bold mb-4" style={{ color: M.text }}>Career Plan Not Found</h1>
            <p className="text-lg mb-6" style={{ color: M.muted }}>
              {error || 'The career plan you are looking for does not exist or you do not have access to it.'}
            </p>
            <button
              onClick={() => navigate('/career-builder')}
              className="px-8 py-4 rounded-lg text-white font-medium hover:shadow-lg transition-all text-lg"
              style={{ background: M.primary }}
            >
              Back to Career Builder
            </button>
          </div>
        </main>
      </div>
    );
  }

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <Header />
      <main className="container mx-auto px-4 mt-6">
        {/* Header Section */}
        <div className="bg-white rounded-3xl p-8 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-4">
            <h1 className="text-3xl font-bold" style={{ color: M.text }}>{careerPlan.title}</h1>
            {getStatusBadge(careerPlan.status)}
          </div>
          <p className="text-lg mb-4" style={{ color: M.muted }}>{careerPlan.summary}</p>
          <div className="flex items-center gap-4 text-sm" style={{ color: M.muted }}>
            <div className="flex items-center gap-2">
              <ClockIcon className="w-4 h-4" />
              <span>Created: {new Date(careerPlan.createdAt).toLocaleDateString()}</span>
            </div>
            <div className="flex items-center gap-2">
              <BarChart3 className="w-4 h-4" />
              <span>Progress: {careerPlan.progressPercentage}%</span>
            </div>
          </div>
        </div>

        {/* Progress Bar */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-2">
            <span className="text-sm font-medium" style={{ color: M.text }}>Overall Progress</span>
            <span className="text-sm font-bold" style={{ color: M.primary }}>{careerPlan.progressPercentage}%</span>
          </div>
          <div className="w-full bg-gray-200 rounded-full h-3">
            <div
              className="h-3 rounded-full transition-all duration-300"
              style={{
                width: `${careerPlan.progressPercentage}%`,
                background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})`
              }}
            />
          </div>
        </div>

        {/* Career Steps Timeline */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold mb-6" style={{ color: M.text }}>Career Roadmap</h2>
          <div className="space-y-6">
            {careerPlan.steps.map((step, index) => {
              const IconComponent = getStepIcon(index);
              const isCompleted = step.progressPercentage === 100;
              const isInProgress = step.progressPercentage > 0 && step.progressPercentage < 100;
              
              return (
                <div key={step.id} className="flex items-start gap-4">
                  <div className={`flex-shrink-0 w-12 h-12 rounded-full flex items-center justify-center ${
                    isCompleted ? 'bg-green-100' : isInProgress ? 'bg-blue-100' : 'bg-gray-100'
                  }`}>
                    <IconComponent className={`w-6 h-6 ${
                      isCompleted ? 'text-green-600' : isInProgress ? 'text-blue-600' : 'text-gray-400'
                    }`} />
                  </div>
                  <div className="flex-1">
                    <div className="flex items-center gap-2 mb-2">
                      <h3 className="text-lg font-semibold" style={{ color: M.text }}>
                        Step {index + 1}: {step.name}
                      </h3>
                      {isCompleted && <CheckCircle className="w-5 h-5 text-green-600" />}
                    </div>
                    <p className="text-sm mb-3" style={{ color: M.muted }}>{step.description}</p>
                    
                    {/* Step Progress */}
                    <div className="mb-3">
                      <div className="flex items-center justify-between mb-1">
                        <span className="text-xs font-medium" style={{ color: M.muted }}>Progress</span>
                        <span className="text-xs font-bold" style={{ color: M.primary }}>
                          {step.progressPercentage}%
                        </span>
                      </div>
                      <div className="w-full bg-gray-200 rounded-full h-2">
                        <div
                          className="h-2 rounded-full transition-all"
                          style={{
                            width: `${step.progressPercentage}%`,
                            backgroundColor: M.primary
                          }}
                        />
                      </div>
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex flex-wrap justify-center gap-4">
          <button
            onClick={() => navigate('/career-builder')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2"
            style={{ background: M.secondary }}
          >
            <ArrowLeft className="w-4 h-4" />
            Back to Career Builder
          </button>
          <button
            onClick={() => navigate('/career-progress')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2"
            style={{ background: M.primary }}
          >
            <TrendingUpIcon className="w-4 h-4" />
            Track Progress
          </button>
          <button
            onClick={() => navigate('/career-skills')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all flex items-center gap-2"
            style={{ background: M.primary }}
          >
            <Code className="w-4 h-4" />
            View Skills
          </button>
        </div>
      </main>
    </div>
  );
}
