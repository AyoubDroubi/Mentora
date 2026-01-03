import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import api from '../services/api';
import SharedHeader from '../components/SharedHeader';
import {
  Target,
  ChevronRight,
  Award,
  CheckSquare,
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
  Activity,
  PieChart,
  Calendar,
  Trophy,
  X,
  Eye,
} from 'lucide-react';

export default function CareerSkills() {
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

  const [skillsData, setSkillsData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [selectedPlanId, setSelectedPlanId] = useState(null);
  const [plans, setPlans] = useState([]);

  useEffect(() => {
    fetchCareerPlans();
  }, []);

  const fetchCareerPlans = async () => {
    try {
      const response = await api.get('/careerplans');
      if (response.data.success && response.data.data.length > 0) {
        setPlans(response.data.data);
        // Auto-select first plan
        const firstPlan = response.data.data[0];
        setSelectedPlanId(firstPlan.id);
        fetchSkillsForPlan(firstPlan.id);
      } else {
        setLoading(false);
      }
    } catch (error) {
      console.error('Error fetching career plans:', error);
      setLoading(false);
    }
  };

  const fetchSkillsForPlan = async (planId) => {
    try {
      setLoading(true);
      const response = await api.get(`/careerplans/${planId}/skills`);
      
      if (response.data.success) {
        const skills = response.data.data;
        
        // Transform skills into the required format
        const transformedSkills = skills.map((skill, index) => ({
          id: skill.id,
          name: skill.skillName,
          type: skill.skillCategory || 'Technical',
          status: skill.status, // 'Missing', 'InProgress', 'Achieved'
          priority: index + 1,
          targetLevel: skill.targetLevelName,
          careerStepId: skill.careerStepId
        }));

        // Get top 5 by priority
        const topSkills = transformedSkills.slice(0, 5);

        setSkillsData({
          topSkills,
          allSkills: transformedSkills
        });
      }
      setLoading(false);
    } catch (error) {
      console.error('Error fetching skills:', error);
      setLoading(false);
    }
  };

  const [showAllSkills, setShowAllSkills] = useState(false);

  const getStatusBadge = (status) => {
    switch (status) {
      case 'Achieved':
        return (
          <span className="inline-flex items-center gap-1 px-2 py-1 rounded-full text-xs font-medium bg-green-100 text-green-700">
            <CheckCircle className="w-3 h-3" />
            Achieved
          </span>
        );
      case 'InProgress':
        return (
          <span className="inline-flex items-center gap-1 px-2 py-1 rounded-full text-xs font-medium bg-yellow-100 text-yellow-700">
            <Clock className="w-3 h-3" />
            In Progress
          </span>
        );
      case 'Missing':
        return (
          <span className="inline-flex items-center gap-1 px-2 py-1 rounded-full text-xs font-medium bg-red-100 text-red-700">
            <AlertCircle className="w-3 h-3" />
            Missing
          </span>
        );
      default:
        return null;
    }
  };

  const handleUpdateSkillStatus = async (skillId, newStatus) => {
    try {
      await api.patch(`/careerplans/${selectedPlanId}/skills/${skillId}`, {
        status: newStatus
      });
      // Refresh skills
      fetchSkillsForPlan(selectedPlanId);
    } catch (error) {
      console.error('Error updating skill status:', error);
    }
  };

  if (loading) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24 flex items-center justify-center">
        <SharedHeader title="Mentora - Career Skills" />
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 mx-auto mb-4" style={{ borderColor: M.primary }}></div>
          <p style={{ color: M.text }}>Loading career skills...</p>
        </div>
      </div>
    );
  }

  if (!skillsData || skillsData.allSkills.length === 0) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <SharedHeader title="Mentora - Career Skills" />
        <main className="container mx-auto px-4 mt-6">
          <div className="bg-white rounded-3xl p-12 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <Trophy className="w-16 h-16 mx-auto mb-4" style={{ color: M.primary }} />
            <h2 className="text-2xl font-bold mb-4" style={{ color: M.text }}>No Career Plan Yet</h2>
            <p className="mb-6" style={{ color: M.muted }}>
              Create your first career plan to see your skill gaps and development path.
            </p>
            <button
              onClick={() => navigate('/create-career-builder')}
              className="px-8 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
              style={{ background: M.primary }}
            >
              Create Career Plan
            </button>
          </div>
        </main>
      </div>
    );
  }

  const displayedSkills = showAllSkills ? skillsData.allSkills : skillsData.topSkills;

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <SharedHeader title="Mentora - Career Skills" />
      <main className="container mx-auto px-4 mt-6">
        {/* Plan Selector */}
        {plans.length > 1 && (
          <div className="bg-white rounded-3xl p-4 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
            <label className="text-sm font-medium mb-2 block" style={{ color: M.text }}>Select Career Plan:</label>
            <select
              value={selectedPlanId}
              onChange={(e) => {
                setSelectedPlanId(e.target.value);
                fetchSkillsForPlan(e.target.value);
              }}
              className="w-full p-3 border rounded-lg"
              style={{ borderColor: M.bg3 }}
            >
              {plans.map(plan => (
                <option key={plan.id} value={plan.id}>
                  {plan.title}
                </option>
              ))}
            </select>
          </div>
        )}

        {/* Skills Overview */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold mb-6" style={{ color: M.text }}>Career Skills Analysis</h2>
          <p className="text-sm mb-6" style={{ color: M.muted }}>
            Based on your career plan, here are the skills you need to develop for your target role.
          </p>

          {/* Top Skills */}
          <div className="mb-6">
            <h3 className="text-xl font-semibold mb-4 flex items-center gap-2" style={{ color: M.text }}>
              <Star className="w-5 h-5" style={{ color: M.primary }} />
              {showAllSkills ? 'All Skills' : 'Top 5 Skills'}
            </h3>
            <div className="space-y-3">
              {displayedSkills.map((skill) => (
                <div key={skill.id} className="flex items-center justify-between p-4 rounded-lg border hover:shadow-md transition-shadow" style={{ borderColor: M.bg3 }}>
                  <div className="flex items-center gap-3 flex-1">
                    <div className="w-8 h-8 rounded-full flex items-center justify-center text-sm font-bold" style={{ background: M.primary, color: 'white' }}>
                      {skill.priority}
                    </div>
                    <div className="flex-1">
                      <h4 className="font-semibold" style={{ color: M.text }}>{skill.name}</h4>
                      <div className="flex items-center gap-2 mt-1">
                        <span className="text-xs" style={{ color: M.muted }}>{skill.type}</span>
                        {skill.targetLevel && (
                          <span className="text-xs px-2 py-0.5 rounded" style={{ background: M.bg3, color: M.muted }}>
                            Target: {skill.targetLevel}
                          </span>
                        )}
                      </div>
                    </div>
                  </div>
                  <div className="flex items-center gap-2">
                    {getStatusBadge(skill.status)}
                    {skill.status !== 'Achieved' && (
                      <button
                        onClick={() => handleUpdateSkillStatus(skill.id, skill.status === 'Missing' ? 'InProgress' : 'Achieved')}
                        className="px-3 py-1 rounded text-xs font-medium hover:shadow transition-all"
                        style={{ background: M.secondary, color: 'white' }}
                      >
                        {skill.status === 'Missing' ? 'Start Learning' : 'Mark Complete'}
                      </button>
                    )}
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* View All Skills Button */}
          <div className="flex justify-center">
            <button
              onClick={() => setShowAllSkills(!showAllSkills)}
              className="flex items-center gap-2 px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
              style={{ background: M.primary }}
            >
              <Eye className="w-4 h-4" />
              {showAllSkills ? 'Show Top 5 Skills' : `View All ${skillsData.allSkills.length} Skills`}
            </button>
          </div>
        </div>

        {/* Skills Statistics */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
          <div className="bg-white rounded-3xl p-6 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <CheckCircle className="w-8 h-8 mx-auto mb-2" style={{ color: '#22c55e' }} />
            <div className="text-3xl font-bold mb-2" style={{ color: M.primary }}>
              {skillsData.allSkills.filter(skill => skill.status === 'Achieved').length}
            </div>
            <p className="text-sm" style={{ color: M.muted }}>Skills Achieved</p>
          </div>
          <div className="bg-white rounded-3xl p-6 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <Clock className="w-8 h-8 mx-auto mb-2" style={{ color: '#eab308' }} />
            <div className="text-3xl font-bold mb-2" style={{ color: M.primary }}>
              {skillsData.allSkills.filter(skill => skill.status === 'InProgress').length}
            </div>
            <p className="text-sm" style={{ color: M.muted }}>In Progress</p>
          </div>
          <div className="bg-white rounded-3xl p-6 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <AlertCircle className="w-8 h-8 mx-auto mb-2" style={{ color: '#ef4444' }} />
            <div className="text-3xl font-bold mb-2" style={{ color: M.primary }}>
              {skillsData.allSkills.filter(skill => skill.status === 'Missing').length}
            </div>
            <p className="text-sm" style={{ color: M.muted }}>Skills Missing</p>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex justify-center gap-4">
          <button
            onClick={() => navigate('/career-builder')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.primary }}
          >
            Back to Career Builder
          </button>
        </div>
      </main>
    </div>
  );
}
