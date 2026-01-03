import { useState, useEffect } from 'react';
import { XMarkIcon } from '@heroicons/react/24/outline';

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

  const [availableSkills] = useState([
    // Mock skills - ?? ?????? ??? ???? ???? ?? API
    { id: '1', name: 'JavaScript', category: 'Technical' },
    { id: '2', name: 'React', category: 'Technical' },
    { id: '3', name: 'Node.js', category: 'Technical' },
    { id: '4', name: 'Python', category: 'Technical' },
    { id: '5', name: 'Communication', category: 'Soft' },
    { id: '6', name: 'Leadership', category: 'Soft' },
    { id: '7', name: 'Project Management', category: 'Business' },
    { id: '8', name: 'Design Thinking', category: 'Creative' }
  ]);

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

  const handleSubmit = (e) => {
    e.preventDefault();
    
    const payload = {
      ...formData,
      yearsOfExperience: formData.yearsOfExperience ? parseInt(formData.yearsOfExperience) : null,
      startedDate: formData.startedDate || null
    };

    onSave(payload);
  };

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
          {/* Skill Selection */}
          {!skill && (
            <div>
              <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
                Select Skill *
              </label>
              <select
                required
                value={formData.skillId}
                onChange={(e) => setFormData({ ...formData, skillId: e.target.value })}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:border-transparent"
                style={{ borderColor: M.bg3, focusRing: M.primary }}
              >
                <option value="">Choose a skill...</option>
                {availableSkills.map((s) => (
                  <option key={s.id} value={s.id}>
                    {s.name} ({s.category})
                  </option>
                ))}
              </select>
            </div>
          )}

          {/* Proficiency Level */}
          <div>
            <label className="block text-sm font-medium mb-2" style={{ color: M.text }}>
              Proficiency Level *
            </label>
            <div className="grid grid-cols-4 gap-3">
              {['Beginner', 'Intermediate', 'Advanced', 'Expert'].map((level, index) => (
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
                onChange={(e) => setFormData({ ...formData, displayOrder: parseInt(e.target.value) })}
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
