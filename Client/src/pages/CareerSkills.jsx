import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
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

  useEffect(() => {
    const fetchSkills = async () => {
      try {
        const response = await fetch('/api/ai/career-skills', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            careerGoal: user.careerGoal,
            skills: user.skills,
            level: user.level,
          }),
        });
        if (response.ok) {
          const data = await response.json();
          setSkillsData(data);
        } else {
          console.error('Failed to fetch skills');
          // Provide mock data when API fails
          setSkillsData({
            topSkills: [
              { name: 'JavaScript/TypeScript', type: 'Technical', status: 'Achieved', priority: 1 },
              { name: 'React/Vue.js', type: 'Technical', status: 'In Progress', priority: 2 },
              { name: 'Node.js', type: 'Technical', status: 'In Progress', priority: 3 },
              { name: 'Database Design', type: 'Technical', status: 'Missing', priority: 4 },
              { name: 'API Development', type: 'Technical', status: 'Missing', priority: 5 }
            ],
            allSkills: [
              { name: 'JavaScript/TypeScript', type: 'Technical', status: 'Achieved', priority: 1 },
              { name: 'React/Vue.js', type: 'Technical', status: 'In Progress', priority: 2 },
              { name: 'Node.js', type: 'Technical', status: 'In Progress', priority: 3 },
              { name: 'Database Design', type: 'Technical', status: 'Missing', priority: 4 },
              { name: 'API Development', type: 'Technical', status: 'Missing', priority: 5 },
              { name: 'Communication', type: 'Soft', status: 'Achieved', priority: 6 },
              { name: 'Problem Solving', type: 'Soft', status: 'In Progress', priority: 7 },
              { name: 'Team Work', type: 'Soft', status: 'Achieved', priority: 8 },
              { name: 'Time Management', type: 'Soft', status: 'In Progress', priority: 9 },
              { name: 'Adaptability', type: 'Soft', status: 'Missing', priority: 10 }
            ]
          });
        }
      } catch (error) {
        console.error('Error fetching skills:', error);
        // Provide mock data when API fails
        setSkillsData({
          topSkills: [
            { name: 'JavaScript/TypeScript', type: 'Technical', status: 'Achieved', priority: 1 },
            { name: 'React/Vue.js', type: 'Technical', status: 'In Progress', priority: 2 },
            { name: 'Node.js', type: 'Technical', status: 'In Progress', priority: 3 },
            { name: 'Database Design', type: 'Technical', status: 'Missing', priority: 4 },
            { name: 'API Development', type: 'Technical', status: 'Missing', priority: 5 }
          ],
          allSkills: [
            { name: 'JavaScript/TypeScript', type: 'Technical', status: 'Achieved', priority: 1 },
            { name: 'React/Vue.js', type: 'Technical', status: 'In Progress', priority: 2 },
            { name: 'Node.js', type: 'Technical', status: 'In Progress', priority: 3 },
            { name: 'Database Design', type: 'Technical', status: 'Missing', priority: 4 },
            { name: 'API Development', type: 'Technical', status: 'Missing', priority: 5 },
            { name: 'Communication', type: 'Soft', status: 'Achieved', priority: 6 },
            { name: 'Problem Solving', type: 'Soft', status: 'In Progress', priority: 7 },
            { name: 'Team Work', type: 'Soft', status: 'Achieved', priority: 8 },
            { name: 'Time Management', type: 'Soft', status: 'In Progress', priority: 9 },
            { name: 'Adaptability', type: 'Soft', status: 'Missing', priority: 10 }
          ]
        });
      }
    };

    fetchSkills();
  }, [user]);

  const [showAllSkills, setShowAllSkills] = useState(false);

  
    const Header = () => (
         <header
           className="px-6 py-4 flex items-center justify-between shadow-lg"
           style={{ background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})` }}
         >
           <div className="flex items-center gap-3">
             <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-md border-2 border-gray-300">
               <BookOpen className="w-6 h-6" style={{ color: M.primary }} />
             </div>
             <span className="text-white text-xl font-bold">Mentora - skill</span>
           </div>
     
           <div className="flex items-center gap-4">
             <img
             onClick={() => navigate('/profile')}
               src={user.avatar}
               alt="Profile"
               className="w-10 h-10 rounded-full border-2 border-white hover:scale-110 hover:opacity-90 transition-all cursor-pointer"
             />
           </div>
         </header>
       );
     


  const getStatusBadge = (status) => {
    switch (status) {
      case 'Achieved':
        return (
          <span className="inline-flex items-center gap-1 px-2 py-1 rounded-full text-xs font-medium bg-green-100 text-green-700">
            <CheckCircle className="w-3 h-3" />
            Achieved
          </span>
        );
      case 'In Progress':
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

  if (skillsData === null) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900 mx-auto mb-4"></div>
          <p style={{ color: M.text }}>Loading skills...</p>
        </div>
      </div>
    );
  }

  const displayedSkills = showAllSkills ? skillsData.allSkills : skillsData.topSkills;

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <Header />
      <main className="container mx-auto px-4 mt-6">
        {/* Skills Overview */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold mb-6" style={{ color: M.text }}>Career Skills Analysis</h2>
          <p className="text-sm mb-6" style={{ color: M.muted }}>
            Based on AI analysis of your career path, here are the most important skills you need to develop.
          </p>

          {/* Top 5 Skills */}
          <div className="mb-6">
            <h3 className="text-xl font-semibold mb-4 flex items-center gap-2" style={{ color: M.text }}>
              <Star className="w-5 h-5" style={{ color: M.primary }} />
              Top 5 Skills
            </h3>
            <div className="space-y-3">
              {displayedSkills.map((skill, index) => (
                <div key={skill.name} className="flex items-center justify-between p-4 rounded-lg border" style={{ borderColor: M.bg3 }}>
                  <div className="flex items-center gap-3">
                    <div className="w-8 h-8 rounded-full flex items-center justify-center text-sm font-bold" style={{ background: M.primary, color: 'white' }}>
                      {skill.priority}
                    </div>
                    <div>
                      <h4 className="font-semibold" style={{ color: M.text }}>{skill.name}</h4>
                      <span className="text-xs" style={{ color: M.muted }}>{skill.type}</span>
                    </div>
                  </div>
                  {getStatusBadge(skill.status)}
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
              {showAllSkills ? 'Show Top 5 Skills' : 'View All Skills'}
            </button>
          </div>
        </div>

        {/* Skills Statistics */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
          <div className="bg-white rounded-3xl p-6 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <div className="text-3xl font-bold mb-2" style={{ color: M.primary }}>
              {skillsData.allSkills.filter(skill => skill.status === 'Achieved').length}
            </div>
            <p className="text-sm" style={{ color: M.muted }}>Skills Achieved</p>
          </div>
          <div className="bg-white rounded-3xl p-6 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <div className="text-3xl font-bold mb-2" style={{ color: M.primary }}>
              {skillsData.allSkills.filter(skill => skill.status === 'In Progress').length}
            </div>
            <p className="text-sm" style={{ color: M.muted }}>In Progress</p>
          </div>
          <div className="bg-white rounded-3xl p-6 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <div className="text-3xl font-bold mb-2" style={{ color: M.primary }}>
              {skillsData.allSkills.filter(skill => skill.status === 'Missing').length}
            </div>
            <p className="text-sm" style={{ color: M.muted }}>Skills Missing</p>
          </div>
        </div>

        {/* Action Buttons */}
        <div className="flex justify-center gap-4">
          <button
            onClick={() => navigate('/create-career-builder')}
            className="px-6 py-3 rounded-lg text-white font-medium hover:shadow-lg transition-all"
            style={{ background: M.primary }}
          >
            Update Skills Assessment
          </button>
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
