import React from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useUser } from '../contexts/UserContext';
import { BookOpen,Home as HomeIcon,User as UserIcon,LayoutDashboard,Target,Users,Award,TrendingUp,CheckCircle,Star,Heart,Lightbulb,
  Zap,Calendar,Clock,BookMarked,ClipboardCheck,HelpCircle,GraduationCap,} from 'lucide-react';

export default function AboutUs() {
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


  return (
    <div style={{ background: `linear-gradient(180deg, ${M.bg1}, ${M.bg2})` }} className="min-h-screen pb-24">
        <nav className="container mx-auto px-4 py-6 flex items-center justify-between backdrop-blur-md bg-white/40 border-b border-[#A4C3B2]/30">
        <div className="flex items-center gap-3">
            <div className="w-10 h-10 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-lg flex items-center justify-center shadow-md">
            <GraduationCap className="w-6 h-6 text-white" />
            </div>
              <span className="text-[#2C3E3F] text-2xl font-semibold select-none">Mentora</span>
           </div>
        <div className="flex items-center gap-4">
              <Link
              to="/about-us"
               className="border-2 border-[#6B9080] text-[#6B9080] hover:bg-[#6B9080] hover:text-white px-6 py-3 shadow-lg rounded-full text-sm transition-all">
              About Us
            </Link>
             <Link
          to="/Login"
      className="bg-[#6B9080] hover:bg-[#5A7A6B] text-white px-6 py-3 shadow-lg rounded-full text-sm transition-all">
              Login  </Link>

        </div>
      </nav>
        <main className="container mx-auto px-4 mt-6">
            {/* Hero Section */}
            <div className="bg-white rounded-3xl p-8 shadow-lg border mb-8 text-center" style={{ borderColor: M.bg3 }}>
               <div className="flex justify-center mb-6">
                <img src="/src/assets/Mentora.jpg" alt="Mentora Logo" className="w-24 h-24 rounded-full object-cover shadow-lg" />
             </div>
             <h1 className="text-4xl font-bold mb-4" style={{ color: M.text }}>About Mentora</h1>
              <p className="text-xl mb-6" style={{ color: M.muted }}>
                Your AI-powered companion for academic excellence and career success
             </p>
              <div className="flex justify-center gap-4">
                <div className="flex items-center gap-2 px-4 py-2 rounded-lg bg-green-100">
                  <Star className="w-5 h-5 text-green-600" />
                  <span className="font-semibold text-green-700">4.8/5 Rating</span>
                 </div>
            <div className="flex items-center gap-2 px-4 py-2 rounded-lg bg-blue-100">
                 <Users className="w-5 h-5 text-blue-600" />
              <span className="font-semibold text-blue-700">10,000+ Users</span>
     </div>
          </div>
             </div>

            {/* Mission Section */}
             <div className="bg-white rounded-3xl p-8 shadow-lg border mb-8" style={{ borderColor: M.bg3 }}>
              <div className="text-center mb-8">
            <Target className="w-16 h-16 mx-auto mb-4" style={{ color: M.primary }} />
                <h2 className="text-3xl font-bold mb-4" style={{ color: M.text }}>Our Mission</h2>
            <p className="text-lg leading-relaxed" style={{ color: M.muted }}>
                  To empower students and young professionals with intelligent tools that enhance learning,
              boost productivity, and guide career development. We believe that with the right guidance
                 and tools, everyone can achieve their academic and professional goals.
            </p>
               </div>
        </div>

        {/* Features Section */}
        <div className="bg-white rounded-3xl p-8 shadow-lg border mb-8" style={{ borderColor: M.bg3 }}>
          <h2 className="text-3xl font-bold text-center mb-8" style={{ color: M.text }}>What We Offer</h2>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
               {/* Study Tools */}
                <div className="text-center p-6 rounded-2xl" style={{ background: M.bg1 }}>
                   <BookMarked className="w-12 h-12 mx-auto mb-4" style={{ color: M.primary }} />
                    <h3 className="text-xl font-bold mb-3" style={{ color: M.text }}>Smart Study Tools</h3>
                      <ul className="text-left space-y-2" style={{ color: M.muted }}>
                        <li className="flex items-center gap-2">
                         <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                  <span>Pomodoro Timer for focused study sessions</span>
                     </li>
                <li className="flex items-center gap-2">
                      <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                  <span>To-Do lists with progress tracking</span>
                     </li>
                <li className="flex items-center gap-2">
                       <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                  <span>Digital notes with organization features</span>
                     </li>
                <li className="flex items-center gap-2">
                      <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                  <span>Attendance tracking and analytics</span>
                    </li>
              </ul>
               </div>

                {/* Career Guidance */}
            <div className="text-center p-6 rounded-2xl" style={{ background: M.bg1 }}>
                  <TrendingUp className="w-12 h-12 mx-auto mb-4" style={{ color: M.primary }} />
              <h3 className="text-xl font-bold mb-3" style={{ color: M.text }}>Career Builder</h3>
              <ul className="text-left space-y-2" style={{ color: M.muted }}>
                   <li className="flex items-center gap-2">
                  <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                       <span>Personalized career assessments</span>
                     </li>
                <li className="flex items-center gap-2">
                  <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                       <span>Resume and cover letter guidance</span>
                   </li>
                  <li className="flex items-center gap-2">
                    <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                   <span>Interview preparation tips</span>
                   </li>
                  <li  className="flex items-center gap-2">
                     <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                      <span>Industry insights and trends</span>
                    </li>
                    </ul>
               </div>

              {/* AI Features */}
               <div className="text-center p-6 rounded-2xl" style={{ background: M.bg1 }}>
                  <Zap className="w-12 h-12 mx-auto mb-4" style={{ color: M.primary }} />
                      <h3 className="text-xl font-bold mb-3" style={{ color: M.text }}>AI-Powered Features</h3>
                   <ul className="text-left space-y-2" style={{ color: M.muted }}>
                    <li className="flex items-center gap-2">
                  <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                      <span>Intelligent study plan generation</span>
                   </li>
                      <li className="flex items-center gap-2">
                     <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                  <span>Smart quiz creation and assessment</span>
                     </li>
                <li className="flex items-center gap-2">
                     <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                  <span>Personalized learning recommendations</span>
                   </li>
                     <li className="flex items-center gap-2">
                  <CheckCircle className="w-4 h-4 flex-shrink-0" style={{ color: M.primary }} />
                      <span>Progress analytics and insights</span>
                  </li>
                  </ul>
             </div>
                  </div>
        </div>
  
              {/* How It Works */}
            <div className="bg-white rounded-3xl p-8 shadow-lg border mb-8" style={{ borderColor: M.bg3 }}>
                 <h2 className="text-3xl font-bold text-center mb-8" style={{ color: M.text }}>How Mentora Works</h2>

                   <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                      <div className="text-center">
                   <div className="w-16 h-16 mx-auto mb-4 rounded-full flex items-center justify-center" style={{ background: M.primary }}>
                    <span className="text-2xl font-bold text-white">1</span>
                           </div>
                     <h3 className="text-xl font-bold mb-3" style={{ color: M.text }}>Assess Your Goals</h3>
                      <p style={{ color: M.muted }}>
                        Take our comprehensive assessments to understand your strengths, interests, and career aspirations.
                       </p>
              </div>

                      <div className="text-center">
                    <div className="w-16 h-16 mx-auto mb-4 rounded-full flex items-center justify-center" style={{ background: M.primary }}>
                       <span className="text-2xl font-bold text-white">2</span>
              </div>
                       <h3 className="text-xl font-bold mb-3" style={{ color: M.text }}>Get Personalized Plans</h3>
                    <p style={{ color: M.muted }}>
                         Receive customized study plans and career roadmaps tailored to your unique profile and goals.
                     </p>
                        </div>

            <div className="text-center">
                 <div className="w-16 h-16 mx-auto mb-4 rounded-full flex items-center justify-center" style={{ background: M.primary }}>
                 <span className="text-2xl font-bold text-white">3</span>
              </div>
                   <h3 className="text-xl font-bold mb-3" style={{ color: M.text }}>Track & Achieve</h3>
                <p style={{ color: M.muted }}>
                    Monitor your progress, stay motivated, and achieve your academic and professional objectives.
                    </p>
            </div>
           </div>
        </div>

          {/* Testimonials */}
              <div className="bg-white rounded-3xl p-8 shadow-lg border mb-8" style={{ borderColor: M.bg3 }}>
           <h2 className="text-3xl font-bold text-center mb-8" style={{ color: M.text }}>What Our Users Say</h2>

                 <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
               <div className="p-6 rounded-2xl border" style={{ borderColor: M.bg3, background: M.bg1 }}>
                  <div className="flex items-center gap-1 mb-3">
                    {[...Array(5)].map((_, i) => (
                      <Star key={i} className="w-5 h-5 fill-current" style={{ color: M.primary }} />
                   ))}
              </div>
                 <p className="mb-4 italic" style={{ color: M.muted }}>
                   "Mentora completely transformed my study habits. The Pomodoro timer and personalized study plans helped me improve my grades significantly!"
                  </p>
              <div className="flex items-center gap-3">
                                       <img src="https://i.pinimg.com/736x/17/d5/2f/17d52f442053514ba3c60c40e927b88b.jpg" alt="Mike" className="w-10 h-10 rounded-full" />
  
                      <div>
                          <p className="font-semibold" style={{ color: M.text }}>Sarah Johnson</p>
                      <p className="text-sm" style={{ color: M.muted }}>Computer Science Student</p>
 
                   </div>
                    </div>
              </div>

               <div className="p-6 rounded-2xl border" style={{ borderColor: M.bg3, background: M.bg1 }}>
                 <div className="flex items-center gap-1 mb-3">
                  {[...Array(5)].map((_, i) => (
                  <Star 
                  key={i} className="w-5 h-5 fill-current" style={{ color: M.primary }} />
                ))}
              </div>
              <p className="mb-4 italic" style={{ color: M.muted }}>
                "The career assessment helped me discover my true passion. Now I'm pursuing a career in data science and loving every moment!"
              </p>
                   <div className="flex items-center gap-3">
                      <img src="https://i.pinimg.com/736x/17/d5/2f/17d52f442053514ba3c60c40e927b88b.jpg" alt="Mike" className="w-10 h-10 rounded-full" />
                <div>
                      <p className="font-semibold" style={{ color: M.text }}>Roa'a Badran</p>
                        <p className="text-sm" style={{ color: M.muted }}>Recent Graduate</p>
                    </div>
                   </div>
               </div>
                   </div>
                 </div>

              {/* Stats Section */}
                  <div className="bg-white rounded-3xl p-8 shadow-lg border mb-8" style={{ borderColor: M.bg3 }}>
                    <h2 className="text-3xl font-bold text-center mb-8" style={{ color: M.text }}>Our Impact</h2>

          <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
                     <div className="text-center">
              <div className="text-4xl font-bold mb-2" style={{ color: M.primary }}>10K+</div>
                       <p className="text-sm" style={{ color: M.muted }}>Active Users</p>
                 </div>
                   <div className="text-center">
                       <div className="text-4xl font-bold mb-2" style={{ color: M.primary }}>50K+</div>
                 <p className="text-sm" style={{ color: M.muted }}>Study Sessions</p>
                 </div>
            <div className="text-center">
                <div className="text-4xl font-bold mb-2" style={{ color: M.primary }}>25K+</div>
               <p className="text-sm" style={{ color: M.muted }}>Career Assessments</p>
               </div>
            <div className="text-center">
                 <div className="text-4xl font-bold mb-2" style={{ color: M.primary }}>95%</div>
              <p className="text-sm" style={{ color: M.muted }}>User Satisfaction</p>
              </div>
            </div>
        </div>

        {/* Call to Action */}
        <div className="bg-gradient-to-r from-[#6B9080] to-[#A4C3B2] rounded-3xl p-8 shadow-lg text-center text-white mb-8">
              <Heart className="w-16 h-16 mx-auto mb-4" />
            <h2 className="text-3xl font-bold mb-4">Ready to Start Your Journey?</h2>
          <p className="text-xl mb-6 opacity-90">
             Join thousands of students who are already achieving their goals with Mentora
            </p>
          <div className="flex flex-col sm:flex-row gap-4 justify-center">
            <button
                onClick={() => navigate('/Login')}
                 className="px-8 py-4 bg-white text-[#6B9080] rounded-lg font-bold hover:shadow-lg transition-all"
              >
              Start Studying
               </button>
            <button
               onClick={() => navigate('/Login')}
              className="px-8 py-4 border-2 border-white text-white rounded-lg font-bold hover:bg-white hover:text-[#6B9080] transition-all"
            >
              Build Your Career
            </button>
          </div>
        </div>
      </main>
    </div>
  );
}
