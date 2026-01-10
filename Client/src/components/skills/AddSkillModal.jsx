import { useState, useEffect } from 'react';
import { XMarkIcon, MagnifyingGlassIcon } from '@heroicons/react/24/outline';
import masterSkillsService from '../../services/masterSkillsService';

const AddSkillModal = ({ skill, onClose, onSave }) => {
  // Theme colors
  const M = {
    primary: '#6B9080',
    secondary: '#A4C3B2',
    bg1: '#F6FFF8',
    bg2: '#EAF4F4',
    bg3: '#E8F3E8',
    text: '#2C3E3F',
    muted: '#5A7A6B',
  };

  const [formData, setFormData] = useState({
    skillId: '',
    proficiencyLevel: 0,
    acquisitionMethod: '',
    startedDate: '',
    yearsOfExperience: '',
    isFeatured: false,
    notes: '',
    displayOrder: 0
  });

  // Master skills library state
  const [availableSkills, setAvailableSkills] = useState([]);
  const [filteredSkills, setFilteredSkills] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Load master skills and categories
  useEffect(() => {
    loadSkillsLibrary();
    loadCategories();
  }, []);

  // Update form data if editing existing skill
  useEffect(() => {
    if (skill) {
      setFormData({
        skillId: skill.skillId,
        proficiencyLevel: skill.proficiencyLevel,
        acquisitionMethod: skill.acquisitionMethod || '',
        startedDate: skill.startedDate ? skill.startedDate.split('T')[0] : '',
        yearsOfExperience: skill.yearsOfExperience || '',
        isFeatured: skill.isFeatured,
        notes: skill.notes || '',
        displayOrder: skill.displayOrder || 0
      });
    }
  }, [skill]);

  // Filter skills when search or category changes
  useEffect(() => {
    filterSkills();
  }, [searchTerm, selectedCategory, availableSkills]);

  const loadSkillsLibrary = async () => {
    try {
      setLoading(true);
      const response = await masterSkillsService.getAllSkills();
      if (response.success) {
        setAvailableSkills(response.data || []);
        setFilteredSkills(response.data || []);
      }
    } catch (err) {
      console.error('Error loading skills:', err);
      setError('Failed to load skills library');
      // Fallback to mock data if API fails
      const mockSkills = [
        { id: '1', name: 'JavaScript', category: 'Technical', description: 'Programming language' },
        { id: '2', name: 'React', category: 'Technical', description: 'Frontend framework' },
        { id: '3', name: 'Node.js', category: 'Technical', description: 'Backend runtime' },
        { id: '4', name: 'Python', category: 'Technical', description: 'Programming language' },
        { id: '5', name: 'Communication', category: 'Soft', description: 'Interpersonal skill' },
      ];
      setAvailableSkills(mockSkills);
      setFilteredSkills(mockSkills);
    } finally {
      setLoading(false);
    }
  };

  const loadCategories = async () => {
    try {
      const response = await masterSkillsService.getCategories();
      if (response.success) {
        setCategories(response.data || []);
      }
    } catch (err) {
      console.error('Error loading categories:', err);
    }
  };

  const filterSkills = () => {
    let filtered = availableSkills;

    // Filter by search term
    if (searchTerm) {
      filtered = filtered.filter(s =>
        s.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
        (s.description && s.description.toLowerCase().includes(searchTerm.toLowerCase()))
      );
    }

    // Filter by category
    if (selectedCategory) {
      filtered = filtered.filter(s => s.category === selectedCategory);
    }

    setFilteredSkills(filtered);
  };

  const handleSkillSelect = (skillId) => {
    setFormData({ ...formData, skillId });
    setError('');
  };

  const getSelectedSkillName = () => {
    const selected = availableSkills.find(s => s.id === formData.skillId);
    return selected ? selected.name : 'None selected';
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setError('');

    // Validation
    if (!formData.skillId) {
      setError('Please select a skill from the library');
      return;
    }

    const payload = {
      skillId: formData.skillId,
      proficiencyLevel: parseInt(formData.proficiencyLevel),
      acquisitionMethod: formData.acquisitionMethod || null,
      startedDate: formData.startedDate || null,
      yearsOfExperience: formData.yearsOfExperience ? parseInt(formData.yearsOfExperience) : null,
      isFeatured: formData.isFeatured,
      notes: formData.notes || null,
      displayOrder: parseInt(formData.displayOrder) || 0
    };

    onSave(payload);
  };

  const proficiencyLevels = ['Beginner', 'Intermediate', 'Advanced', 'Expert'];

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-3xl shadow-xl max-w-2xl w-full max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b" style={{ borderColor: M.bg3 }}>
          <h2 className="text-2xl font-bold" style={{ color: M.text }}>
            {skill ? 'Edit Skill' : 'Add New Skill'}
          </h2>
          <button
            onClick={onClose}
            className="p-2 hover:bg-gray-100 rounded-lg transition"
          >
            <XMarkIcon className="w-6 h-6" style={{ color: M.muted }} />
          </button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="p-6 space-y-6">
          {error && (
            <div className="p-4 bg-red-50 border border-red-200 rounded-lg text-red-700">
              {error}
            </div>
          )}

          {/* Skill Selection */}
          {!skill && (
            <div>
              <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
                Select Skill <span className="text-red-500">*</span>
              </label>
              
              {/* Selected Skill Display */}
              <div className="mb-3 p-3 bg-gray-50 rounded-lg border" style={{ borderColor: M.bg3 }}>
                <span className="text-sm" style={{ color: M.muted }}>Selected: </span>
                <span className="font-medium" style={{ color: M.text }}>{getSelectedSkillName()}</span>
              </div>

              {/* Search and Filter */}
              <div className="mb-3 flex gap-3">
                <div className="flex-1 relative">
                  <MagnifyingGlassIcon className="w-5 h-5 absolute left-3 top-1/2 transform -translate-y-1/2" style={{ color: M.muted }} />
                  <input
                    type="text"
                    placeholder="Search skills..."
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    className="w-full pl-10 pr-4 py-2 border rounded-lg"
                    style={{ borderColor: M.bg3 }}
                  />
                </div>
                {categories.length > 0 && (
                  <select
                    value={selectedCategory}
                    onChange={(e) => setSelectedCategory(e.target.value)}
                    className="px-4 py-2 border rounded-lg"
                    style={{ borderColor: M.bg3 }}
                  >
                    <option value="">All Categories</option>
                    {categories.map(cat => (
                      <option key={cat.value} value={cat.value}>{cat.name}</option>
                    ))}
                  </select>
                )}
              </div>

              {/* Skills List */}
              <div className="border rounded-lg max-h-64 overflow-y-auto" style={{ borderColor: M.bg3 }}>
                {loading ? (
                  <div className="p-4 text-center" style={{ color: M.muted }}>Loading skills...</div>
                ) : filteredSkills.length === 0 ? (
                  <div className="p-4 text-center" style={{ color: M.muted }}>No skills found</div>
                ) : (
                  <div className="divide-y" style={{ borderColor: M.bg3 }}>
                    {filteredSkills.map(skillOption => (
                      <button
                        key={skillOption.id}
                        type="button"
                        onClick={() => handleSkillSelect(skillOption.id)}
                        className={`w-full text-left p-3 hover:bg-gray-50 transition ${
                          formData.skillId === skillOption.id ? 'bg-blue-50' : ''
                        }`}
                      >
                        <div className="font-medium" style={{ color: M.text }}>{skillOption.name}</div>
                        <div className="text-sm flex items-center gap-2 mt-1">
                          <span className="px-2 py-0.5 bg-gray-100 rounded text-xs">{skillOption.category}</span>
                          {skillOption.description && (
                            <span style={{ color: M.muted }} className="text-xs">{skillOption.description}</span>
                          )}
                        </div>
                      </button>
                    ))}
                  </div>
                )}
              </div>
            </div>
          )}

          {/* Proficiency Level */}
          <div>
            <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
              Proficiency Level <span className="text-red-500">*</span>
            </label>
            <div className="grid grid-cols-4 gap-3">
              {proficiencyLevels.map((level, index) => (
                <button
                  key={level}
                  type="button"
                  onClick={() => setFormData({ ...formData, proficiencyLevel: index })}
                  className="py-3 rounded-lg font-medium transition"
                  style={{
                    background: formData.proficiencyLevel === index ? M.primary : M.bg3,
                    color: formData.proficiencyLevel === index ? 'white' : M.text
                  }}
                >
                  {level}
                </button>
              ))}
            </div>
          </div>

          {/* Acquisition Method */}
          <div>
            <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
              How did you acquire this skill?
            </label>
            <input
              type="text"
              placeholder="e.g., Online Course, University, Self-taught..."
              maxLength={200}
              value={formData.acquisitionMethod}
              onChange={(e) => setFormData({ ...formData, acquisitionMethod: e.target.value })}
              className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:border-transparent"
              style={{ borderColor: M.bg3 }}
            />
          </div>

          {/* Started Date & Years of Experience */}
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
                Started Date
              </label>
              <input
                type="date"
                value={formData.startedDate}
                onChange={(e) => setFormData({ ...formData, startedDate: e.target.value })}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:border-transparent"
                style={{ borderColor: M.bg3 }}
              />
            </div>
            <div>
              <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
                Years of Experience
              </label>
              <input
                type="number"
                min="0"
                max="50"
                placeholder="0"
                value={formData.yearsOfExperience}
                onChange={(e) => setFormData({ ...formData, yearsOfExperience: e.target.value })}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:border-transparent"
                style={{ borderColor: M.bg3 }}
              />
            </div>
          </div>

          {/* Featured Toggle */}
          <div className="flex items-center gap-3 p-4 rounded-lg border" style={{ borderColor: M.secondary, background: M.bg1 }}>
            <input
              type="checkbox"
              id="isFeatured"
              checked={formData.isFeatured}
              onChange={(e) => setFormData({ ...formData, isFeatured: e.target.checked })}
              className="w-5 h-5 rounded"
              style={{ accentColor: M.primary }}
            />
            <label htmlFor="isFeatured" className="flex-1">
              <div className="font-medium" style={{ color: M.text }}>? Featured Skill</div>
              <div className="text-sm" style={{ color: M.muted }}>
                Display this skill prominently on your profile (max 10)
              </div>
            </label>
          </div>

          {/* Display Order (only for featured) */}
          {formData.isFeatured && (
            <div>
              <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
                Display Order (for featured skills)
              </label>
              <input
                type="number"
                min="0"
                value={formData.displayOrder}
                onChange={(e) => setFormData({ ...formData, displayOrder: parseInt(e.target.value) || 0 })}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:border-transparent"
                style={{ borderColor: M.bg3 }}
              />
            </div>
          )}

          {/* Notes */}
          <div>
            <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
              Notes (Optional)
            </label>
            <textarea
              rows={4}
              maxLength={1000}
              placeholder="Any additional notes about this skill..."
              value={formData.notes}
              onChange={(e) => setFormData({ ...formData, notes: e.target.value })}
              className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:border-transparent resize-none"
              style={{ borderColor: M.bg3 }}
            />
            <div className="text-xs mt-1 text-right" style={{ color: M.muted }}>
              {formData.notes.length} / 1000
            </div>
          </div>

          {/* Actions */}
          <div className="flex gap-3 pt-4 border-t" style={{ borderColor: M.bg3 }}>
            <button
              type="button"
              onClick={onClose}
              className="flex-1 px-6 py-3 border rounded-lg hover:bg-gray-50 transition font-medium"
              style={{ borderColor: M.bg3, color: M.text }}
            >
              Cancel
            </button>
            <button
              type="submit"
              className="flex-1 px-6 py-3 rounded-lg text-white hover:shadow-lg transition font-medium"
              style={{ background: M.primary }}
            >
              {skill ? 'Update Skill' : 'Add Skill'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddSkillModal;
