import { useState, useEffect } from 'react';
import { 
  PlusIcon, 
  TrashIcon, 
  PencilIcon, 
  StarIcon,
  ChartBarIcon,
  FunnelIcon,
  ArrowsUpDownIcon
} from '@heroicons/react/24/outline';
import { StarIcon as StarSolidIcon } from '@heroicons/react/24/solid';
import skillsService from '../services/skillsService';
import AddSkillModal from '../components/skills/AddSkillModal';
import SkillsAnalytics from '../components/skills/SkillsAnalytics';
import SharedHeader from '../components/SharedHeader';
import { useNavigate } from 'react-router-dom';

const SkillsManagement = () => {
  const navigate = useNavigate();

  // Theme colors matching the rest of the app
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const [skills, setSkills] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showAddModal, setShowAddModal] = useState(false);
  const [showAnalytics, setShowAnalytics] = useState(false);
  const [selectedSkill, setSelectedSkill] = useState(null);
  const [filters, setFilters] = useState({
    proficiencyLevel: null,
    isFeatured: null,
    sortBy: 'name'
  });

  useEffect(() => {
    loadSkills();
  }, [filters]);

  const loadSkills = async () => {
    try {
      setLoading(true);
      const response = await skillsService.getSkills(filters);
      setSkills(response.data || []);
    } catch (error) {
      console.error('Error loading skills:', error);
      
      // Show specific error message
      if (error.response?.status === 401) {
        alert('Please login to view your skills');
        navigate('/login');
      } else if (error.response?.status === 400) {
        console.error('Bad request:', error.response?.data);
        // Set empty skills on bad request
        setSkills([]);
      } else {
        // Just log other errors and show empty state
        setSkills([]);
      }
    } finally {
      setLoading(false);
    }
  };

  const handleAddSkill = async (skillData) => {
    try {
      await skillsService.addSkill(skillData);
      setShowAddModal(false);
      loadSkills();
    } catch (error) {
      if (error.response?.status === 409) {
        alert(error.response.data.message || 'Skill already exists or limit reached');
      } else {
        alert('Error adding skill');
      }
    }
  };

  const handleUpdateSkill = async (skillData) => {
    try {
      await skillsService.updateSkill(selectedSkill.id, skillData);
      setSelectedSkill(null);
      setShowAddModal(false);
      loadSkills();
    } catch (error) {
      alert('Error updating skill');
    }
  };

  const handleDeleteSkill = async (skillId) => {
    if (!confirm('Are you sure you want to delete this skill?')) return;

    try {
      await skillsService.deleteSkill(skillId);
      loadSkills();
    } catch (error) {
      alert('Error deleting skill');
    }
  };

  const handleToggleFeatured = async (skillId) => {
    try {
      await skillsService.toggleFeatured(skillId);
      loadSkills();
    } catch (error) {
      if (error.response?.status === 409) {
        alert('Maximum 10 featured skills allowed');
      } else {
        alert('Error toggling featured status');
      }
    }
  };

  const getProficiencyBadge = (level) => {
    const colors = {
      0: { bg: '#f3f4f6', text: '#374151' },
      1: { bg: '#dbeafe', text: '#1e40af' },
      2: { bg: '#fae8ff', text: '#7e22ce' },
      3: { bg: '#d1fae5', text: '#065f46' }
    };
    const color = colors[level] || colors[0];
    return { backgroundColor: color.bg, color: color.text };
  };

  const getProficiencyWidth = (level) => {
    return `${(level + 1) * 25}%`;
  };

  if (loading) {
    return (
      <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 mx-auto mb-4" style={{ borderColor: M.primary }}></div>
          <p style={{ color: M.text }}>Loading skills...</p>
        </div>
      </div>
    );
  }

  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
      <SharedHeader />

      <main className="container mx-auto px-4 mt-6">
        {/* Header */}
        <div className="bg-white rounded-3xl p-6 shadow-lg border mb-6" style={{ borderColor: M.bg3 }}>
          <div className="flex items-center justify-between mb-4">
            <div>
              <h1 className="text-3xl font-bold" style={{ color: M.text }}>My Skills Portfolio</h1>
              <p className="mt-1" style={{ color: M.muted }}>
                {skills.length} / 100 skills • {skills.filter(s => s.isFeatured).length} / 10 featured
              </p>
            </div>
            <div className="flex gap-3">
              <button
                onClick={() => setShowAnalytics(!showAnalytics)}
                className="px-4 py-2 bg-white border rounded-lg hover:shadow transition flex items-center gap-2"
                style={{ borderColor: M.bg3, color: M.text }}
              >
                <ChartBarIcon className="w-5 h-5" />
                Analytics
              </button>
              <button
                onClick={() => {
                  setSelectedSkill(null);
                  setShowAddModal(true);
                }}
                className="px-4 py-2 rounded-lg text-white font-medium hover:shadow-lg transition flex items-center gap-2"
                style={{ background: M.primary }}
              >
                <PlusIcon className="w-5 h-5" />
                Add Skill
              </button>
            </div>
          </div>

          {/* Filters */}
          <div className="flex flex-wrap gap-4 pt-4 border-t" style={{ borderColor: M.bg3 }}>
            <div className="flex items-center gap-2">
              <FunnelIcon className="w-5 h-5" style={{ color: M.muted }} />
              <span className="text-sm font-medium" style={{ color: M.text }}>Filters:</span>
            </div>
            
            <select
              value={filters.proficiencyLevel || ''}
              onChange={(e) => setFilters({ ...filters, proficiencyLevel: e.target.value ? parseInt(e.target.value) : null })}
              className="px-3 py-1.5 border rounded-lg text-sm"
              style={{ borderColor: M.bg3 }}
            >
              <option value="">All Levels</option>
              <option value="0">Beginner</option>
              <option value="1">Intermediate</option>
              <option value="2">Advanced</option>
              <option value="3">Expert</option>
            </select>

            <select
              value={filters.isFeatured === null ? '' : filters.isFeatured.toString()}
              onChange={(e) => setFilters({ ...filters, isFeatured: e.target.value === '' ? null : e.target.value === 'true' })}
              className="px-3 py-1.5 border rounded-lg text-sm"
              style={{ borderColor: M.bg3 }}
            >
              <option value="">All Skills</option>
              <option value="true">Featured Only</option>
              <option value="false">Non-Featured</option>
            </select>

            <div className="flex items-center gap-2 ml-auto">
              <ArrowsUpDownIcon className="w-5 h-5" style={{ color: M.muted }} />
              <select
                value={filters.sortBy}
                onChange={(e) => setFilters({ ...filters, sortBy: e.target.value })}
                className="px-3 py-1.5 border rounded-lg text-sm"
                style={{ borderColor: M.bg3 }}
              >
                <option value="name">Name</option>
                <option value="proficiency">Proficiency</option>
                <option value="experience">Experience</option>
                <option value="date">Date Added</option>
              </select>
            </div>
          </div>
        </div>

        {/* Analytics Panel */}
        {showAnalytics && (
          <div className="mb-6 animate-fade-in">
            <SkillsAnalytics />
          </div>
        )}

        {/* Skills Grid */}
        {skills.length === 0 ? (
          <div className="bg-white rounded-3xl p-12 shadow-lg border text-center" style={{ borderColor: M.bg3 }}>
            <div className="text-6xl mb-4">??</div>
            <h3 className="text-xl font-semibold mb-2" style={{ color: M.text }}>No skills yet</h3>
            <p className="mb-4" style={{ color: M.muted }}>Start building your skills portfolio</p>
            <button
              onClick={() => setShowAddModal(true)}
              className="px-6 py-2 rounded-lg text-white font-medium hover:shadow-lg transition"
              style={{ background: M.primary }}
            >
              Add Your First Skill
            </button>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
            {skills.map((skill) => (
              <div
                key={skill.id}
                className="bg-white rounded-3xl shadow-lg hover:shadow-xl transition p-6 relative border"
                style={{ borderColor: M.bg3 }}
              >
                {/* Featured Star */}
                <button
                  onClick={() => handleToggleFeatured(skill.id)}
                  className="absolute top-4 right-4"
                >
                  {skill.isFeatured ? (
                    <StarSolidIcon className="w-6 h-6 text-yellow-500" />
                  ) : (
                    <StarIcon className="w-6 h-6 text-gray-300 hover:text-yellow-500 transition" />
                  )}
                </button>

                {/* Skill Name */}
                <div className="mb-4">
                  <h3 className="text-lg font-semibold mb-1" style={{ color: M.text }}>
                    {skill.skillName}
                  </h3>
                  <span className="text-xs" style={{ color: M.muted }}>{skill.skillCategory}</span>
                </div>

                {/* Proficiency */}
                <div className="mb-4">
                  <div className="flex items-center justify-between mb-2">
                    <span className="text-sm" style={{ color: M.muted }}>Proficiency</span>
                    <span
                      className="px-2 py-0.5 rounded text-xs font-medium"
                      style={getProficiencyBadge(skill.proficiencyLevel)}
                    >
                      {skill.proficiencyLevelName}
                    </span>
                  </div>
                  <div className="w-full bg-gray-200 rounded-full h-2">
                    <div
                      className="h-2 rounded-full transition-all"
                      style={{
                        background: `linear-gradient(90deg, ${M.primary}, ${M.secondary})`,
                        width: getProficiencyWidth(skill.proficiencyLevel)
                      }}
                    ></div>
                  </div>
                </div>

                {/* Details */}
                <div className="space-y-2 text-sm mb-4" style={{ color: M.muted }}>
                  {skill.acquisitionMethod && (
                    <div className="flex items-center gap-2">
                      <span>??</span>
                      <span>{skill.acquisitionMethod}</span>
                    </div>
                  )}
                  {skill.yearsOfExperience && (
                    <div className="flex items-center gap-2">
                      <span>??</span>
                      <span>{skill.yearsOfExperience} years experience</span>
                    </div>
                  )}
                  {skill.startedDate && (
                    <div className="flex items-center gap-2">
                      <span>??</span>
                      <span>Since {new Date(skill.startedDate).getFullYear()}</span>
                    </div>
                  )}
                </div>

                {/* Notes */}
                {skill.notes && (
                  <div className="text-sm mb-4 italic pl-3" style={{ 
                    color: M.muted, 
                    borderLeft: `2px solid ${M.secondary}` 
                  }}>
                    "{skill.notes}"
                  </div>
                )}

                {/* Actions */}
                <div className="flex gap-2 pt-4 border-t" style={{ borderColor: M.bg3 }}>
                  <button
                    onClick={() => {
                      setSelectedSkill(skill);
                      setShowAddModal(true);
                    }}
                    className="flex-1 px-3 py-1.5 rounded hover:shadow transition text-sm flex items-center justify-center gap-2"
                    style={{ background: M.bg3, color: M.text }}
                  >
                    <PencilIcon className="w-4 h-4" />
                    Edit
                  </button>
                  <button
                    onClick={() => handleDeleteSkill(skill.id)}
                    className="flex-1 px-3 py-1.5 bg-red-50 text-red-600 rounded hover:bg-red-100 transition text-sm flex items-center justify-center gap-2"
                  >
                    <TrashIcon className="w-4 h-4" />
                    Delete
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </main>

      {/* Add/Edit Modal */}
      {showAddModal && (
        <AddSkillModal
          skill={selectedSkill}
          onClose={() => {
            setShowAddModal(false);
            setSelectedSkill(null);
          }}
          onSave={selectedSkill ? handleUpdateSkill : handleAddSkill}
        />
      )}
    </div>
  );
};

export default SkillsManagement;
