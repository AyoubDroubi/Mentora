import React, { useState } from "react";
import { Link } from "react-router-dom";
import { Award, BriefcaseBusiness, GraduationCap, BookOpenText, Mail, MapPin, Phone, Linkedin, Twitter, Github, Star, Quote, Notebook, Target, Calendar, Users, ArrowRight } from "lucide-react";






/* hero section */
function Hero() {
  return (
    <div className="relative overflow-hidden bg-gradient-to-br from-[#F6FFF8] via-[#EAF4F4] to-[#E8F3E8]">
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

      <div className="container mx-auto px-4 py-20 lg:py-24">
        <div className="grid lg:grid-cols-2 gap-12 items-center max-w-7xl mx-auto">
          <div className="text-center lg:text-left">
            <h1 className="text-[#2C3E3F] text-5xl lg:text-6xl font-semibold mb-6 leading-tight">
              Build Your Career,<br />
              <span className="text-[#6B9080]">Plan Your Success</span>
            </h1>
            <p className="text-[#5A7A6B] text-xl mb-10 max-w-2xl mx-auto lg:mx-0">
              Mentora helps you navigate your academic journey and career path with intelligent planning tools and personalized guidance.
            </p>
            <div className="flex flex-col sm:flex-row gap-4 justify-center lg:justify-start">
              <Link
                to="/login"
                className="flex items-center justify-center gap-2 bg-[#6B9080] hover:bg-[#5A7A6B] text-white px-8 py-4 text-lg rounded-2xl shadow-lg transition-all"
              >
                Get Started
                <ArrowRight className="w-5 h-5" />
              </Link>
              <button
                onClick={() => document.getElementById("why-mentora")?.scrollIntoView({ behavior: "smooth" })}
                className="border-2 border-[#6B9080] text-[#6B9080] hover:bg-[#6B9080]/10 px-8 py-4 text-lg rounded-2xl transition-all"
              >
                Why Mentora?
              </button>
            </div>
          </div>
          <div className="relative">
            <div className="relative rounded-2xl overflow-hidden shadow-2xl border-2 border-[#A4C3B2]">
              <img 
                src="https://images.unsplash.com/photo-1522202176988-66273c2fd55f?w=800&h=600&fit=crop" 
                alt="Students collaborating" 
                className="w-full h-full object-cover" 
                draggable="false"
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

/* Project Summary */
function ProjectSummary() {
  return (
    <section className="bg-[#F6FFF8] py-20">
      <div className="container mx-auto px-4 max-w-7xl">
        <div className="grid lg:grid-cols-2 gap-12 items-center">
          {/* النص على اليمين */}
          <div className="order-2 lg:order-1">
            <h2 className="text-4xl lg:text-5xl font-semibold text-[#2C3E3F] mb-6">
              What is <span className="text-[#6B9080]">Mentora</span>?
            </h2>
            <p className="text-[#5A7A6B] text-lg mb-6 leading-relaxed">
             Mentora was developed to be the personal guidance platform every student and recent graduate needs. In a world full of scattered and unstructured information, we wanted to create one place that helps you plan your future with confidence.
            </p>
            <p className="text-[#5A7A6B] text-lg mb-8 leading-relaxed">
             We built this platform because students need a tool that combines study planning, skill development, and career building — all in a simple, easy-to-use interface.
            </p>

            {/* البوكسات مع الصور على الشمال */}
            <div className="space-y-4">
              <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-xl p-5 hover:shadow-lg transition-all duration-300 flex items-start gap-4">
                <img 
                  src="https://images.unsplash.com/photo-1516321318423-f06f85e504b3?w=100&h=100&fit=crop" 
                  alt="Career planning" 
                  className="w-20 h-20 rounded-lg object-cover flex-shrink-0"
                />
                <div>
                  <h4 className="text-[#2C3E3F] font-semibold text-lg mb-1">Build a personalized career plan</h4>
                  <p className="text-[#5A7A6B] text-sm">Based on your skills and goals</p>
                </div>
              </div>

              <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-xl p-5 hover:shadow-lg transition-all duration-300 flex items-start gap-4">
                <img 
                  src="https://images.unsplash.com/photo-1434030216411-0b793f4b4173?w=100&h=100&fit=crop" 
                  alt="Study planning" 
                  className="w-20 h-20 rounded-lg object-cover flex-shrink-0"
                />
                <div>
                  <h4 className="text-[#2C3E3F] font-semibold text-lg mb-1">Use the study planner</h4>
                  <p className="text-[#5A7A6B] text-sm">Organize courses, exams, and daily focus</p>
                </div>
              </div>

              <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-xl p-5 hover:shadow-lg transition-all duration-300 flex items-start gap-4">
                <img 
                  src="https://images.unsplash.com/photo-1586281380349-632531db7ed4?w=100&h=100&fit=crop" 
                  alt="CV building" 
                  className="w-20 h-20 rounded-lg object-cover flex-shrink-0"
                />
                <div>
                  <h4 className="text-[#2C3E3F] font-semibold text-lg mb-1">Prepare a professional CV</h4>
                  <p className="text-[#5A7A6B] text-sm">Step by step, tailored to each job</p>
                </div>
              </div>
            </div>
          </div>

          {/* الصورة على الشمال */}
          <div className="order-1 lg:order-2">
            <div className="relative rounded-2xl overflow-hidden shadow-2xl border-2 border-[#A4C3B2]">
              <img 
                src="https://images.unsplash.com/photo-1523240795612-9a054b0db644?w=800&h=900&fit=crop" 
                alt="Student planning" 
                className="w-full h-full object-cover"
              />
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}


const benefits = [
  {
    icon: Target,
    title: "Personalized Insights",
    description: "Get tailored recommendations based on your goals, interests, and academic performance to make informed decisions."
  },
  {
    icon: Calendar,
    title: "Progress Tracking",
    description: "Monitor your academic progress and career milestones with intuitive dashboards and detailed analytics."
  },
  {
    icon: Users,
    title: "Expert Guidance",
    description: "Access resources and strategies from education and career experts to accelerate your success."
  },
];

function CheckCircleMini({ color = "#6B9080" }) {
  return (
    <svg viewBox="0 0 24 24" className="w-5 h-5" aria-hidden>
      <circle cx="12" cy="12" r="12" fill={color} />
      <path
        d="M7 12.5l2.5 2.5L17 8.5"
        fill="none"
        stroke="white"
        strokeWidth="2"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  );
}

/* Why Mentora */
function WhyMentora() {
  return (
    <section id="why-mentora" className="bg-gradient-to-b from-[#F6FFF8] to-[#EAF4F4] py-20">
      <div className="container mx-auto px-4 max-w-6xl text-center">
        <h2 className="text-[#2C3E3F] text-4xl lg:text-5xl mb-4 font-semibold">
          Why Choose <span className="bg-gradient-to-r from-[#6B9080] to-[#A4C3B2] bg-clip-text text-transparent">Mentora</span>?
        </h2>
        <p className="text-[#5A7A6B] text-xl mb-12">
          Empowering students and professionals to achieve their goals with smart planning and career guidance
        </p>

          <div className="container mx-auto px-4 max-w-7xl grid lg:grid-cols-2 gap-16">
        {/* Career Builder */}
        <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-2xl p-8 shadow-xl backdrop-blur-sm hover:shadow-2xl hover:border-[#6B9080] transition-all duration-300">
          <div className="w-16 h-16 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-xl flex items-center justify-center mb-6 shadow-lg">
            <BriefcaseBusiness className="w-8 h-8 text-white" />
          </div>
          <h3 className="text-[#2C3E3F] text-3xl font-semibold mb-4">Career Builder</h3>
          <p className="text-[#5A7A6B] mb-6 text-lg">Navigate your professional path with confidence. Set career goals, discover opportunities, and build a roadmap that aligns with your aspirations and strengths.</p>
          <ul className="space-y-3">
            <li className="flex items-start gap-3 text-[#5A7A6B]"><CheckCircleMini /> Personalized career path recommendations</li>
            <li className="flex items-start gap-3 text-[#5A7A6B]"><CheckCircleMini /> Industry insights and market trends</li>
            <li className="flex items-start gap-3 text-[#5A7A6B]"><CheckCircleMini /> Skills gap analysis and development plans</li>
          </ul>
        </div>

        {/* Study Planner */}
        <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-2xl p-8 shadow-xl backdrop-blur-sm hover:shadow-2xl hover:border-[#A4C3B2] transition-all duration-300">
          <div className="w-16 h-16 bg-gradient-to-br from-[#A4C3B2] to-[#6B9080] rounded-xl flex items-center justify-center mb-6 shadow-lg">
            <BookOpenText className="w-8 h-8 text-white" />
          </div>
          <h3 className="text-[#2C3E3F] text-3xl font-semibold mb-4">Study Planner</h3>
          <p className="text-[#5A7A6B] mb-6 text-lg">Organize your learning journey efficiently. Create study schedules, track assignments, and maintain a perfect balance between academics and personal growth.</p>
          <ul className="space-y-3">
            <li className="flex items-start gap-3 text-[#5A7A6B]"><CheckCircleMini /> Smart scheduling with deadline tracking</li>
            <li className="flex items-start gap-3 text-[#5A7A6B]"><CheckCircleMini /> Progress monitoring and performance analytics</li>
            <li className="flex items-start gap-3 text-[#5A7A6B]"><CheckCircleMini /> Resource organization and study materials hub</li>
          </ul>
        </div>
      </div>
      </div>
    </section>
  );
}







/* Testimonials */
 function Testimonials() {
  const [formData, setFormData] = useState({ name: "", role: "", comment: "" });

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("New testimonial:", formData);
    setFormData({ name: "", role: "", comment: "" });
  };

  return (
    <section className="bg-[#F6FFF8] py-20">
      <div className="container mx-auto px-4 max-w-6xl text-center">

        <h2 className="text-[#2C3E3F] text-4xl lg:text-5xl mb-8 font-semibold">
          What Our <span className="bg-gradient-to-r from-[#6B9080] to-[#A4C3B2] bg-clip-text text-transparent">Users Say</span>
        </h2>

        <p className="text-[#5A7A6B] text-xl mb-12 max-w-2xl mx-auto">
          Real experiences from students and professionals who transformed their journey with Mentora
        </p>

        <div className="grid md:grid-cols-3 gap-8 mb-16">

          {/* Testimonial 1 */}
          <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-2xl p-8 relative backdrop-blur-sm hover:shadow-xl hover:border-[#6B9080] transition-all duration-300">
          

            <div className="flex items-center gap-4 mb-4">
              <img src="pic1.jpg" alt="Sarah Ahmed" className="w-16 h-16 rounded-full border-2 border-[#6B9080]" />
              <div className="text-left">
                <h4 className="text-[#2C3E3F] font-semibold">Sarah Ahmed</h4>
                <p className="text-[#5A7A6B] text-sm">Software Student</p>
                <div className="flex gap-1 mt-1">
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                </div>
              </div>
            </div>

            <p className="text-[#5A7A6B] leading-relaxed text-left">
              Mentora helped me organize my studying in a way I never thought possible!
            </p>
          </div>

          {/* Testimonial 2 */}
          <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-2xl p-8 relative backdrop-blur-sm hover:shadow-xl hover:border-[#6B9080] transition-all duration-300">     

            <div className="flex items-center gap-4 mb-4">
              <img src="pic2.jpg" alt="Mohammed Ali" className="w-16 h-16 rounded-full border-2 border-[#6B9080]" />
              <div className="text-left">
                <h4 className="text-[#2C3E3F] font-semibold">Mohammed Ali</h4>
                <p className="text-[#5A7A6B] text-sm">Junior Developer</p>
                <div className="flex gap-1 mt-1">
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                </div>
              </div>
            </div>

            <p className="text-[#5A7A6B] leading-relaxed text-left">
              The career builder feature made planning my next steps so much easier.
            </p>
          </div>

          {/* Testimonial 3 */}
          <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-2xl p-8 relative backdrop-blur-sm hover:shadow-xl hover:border-[#6B9080] transition-all duration-300">
            

            <div className="flex items-center gap-4 mb-4">
              <img src="pic3.jpg" alt="Lina Majed" className="w-16 h-16 rounded-full border-2 border-[#6B9080]" />
              <div className="text-left">
                <h4 className="text-[#2C3E3F] font-semibold">Lina Majed</h4>
                <p className="text-[#5A7A6B] text-sm">Business Student</p>
                <div className="flex gap-1 mt-1">
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                  <Star className="w-4 h-4 text-[#6B9080] fill-[#6B9080]" />
                </div>
              </div>
            </div>

            <p className="text-[#5A7A6B] leading-relaxed text-left">
              I love how organized and simple Mentora makes everything feel.
            </p>
          </div>

        </div>

        {/* نموذج التقييم */}
        <div className="max-w-2xl mx-auto">
          <div className="bg-white/80 border border-[#A4C3B2]/40 rounded-2xl p-8 backdrop-blur-sm">
            <h3 className="text-[#2C3E3F] text-2xl mb-2 font-semibold text-center">Share Your Experience</h3>
            <p className="text-[#5A7A6B] text-center mb-8">
              Tell us how Mentora helped you achieve your goals
            </p>

            <form onSubmit={handleSubmit} className="space-y-4">
              <input
                type="text"
                placeholder="Your Name"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                className="w-full px-4 py-3 rounded-lg bg-white border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#5A7A6B] focus:border-[#6B9080] focus:outline-none"
              />
              <input
                type="text"
                placeholder="Your Role"
                value={formData.role}
                onChange={(e) => setFormData({ ...formData, role: e.target.value })}
                className="w-full px-4 py-3 rounded-lg bg-white border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#5A7A6B] focus:border-[#6B9080] focus:outline-none"
              />
              <textarea
                placeholder="Your Experience"
                value={formData.comment}
                onChange={(e) => setFormData({ ...formData, comment: e.target.value })}
                className="w-full px-4 py-3 rounded-lg bg-white border border-[#A4C3B2] text-[#2C3E3F] placeholder:text-[#5A7A6B] focus:border-[#6B9080] focus:outline-none min-h-[120px]"
              />
              <button
                type="submit"
                className="w-full bg-[#6B9080] hover:bg-[#5A7A6B] text-white py-4 rounded-xl transition-all shadow-lg"
              >
                Submit Your Testimonial
              </button>
            </form>
          </div>
        </div>

      </div>
    </section>
  );
}
/* Footer */
function Footer() {
  return (
    <footer className="bg-[#F6FFF8] border-t border-[#A4C3B2]/30 py-12">
      <div className="container mx-auto px-4">
        <div className="grid md:grid-cols-4 gap-8 max-w-6xl mx-auto">
          <div className="md:col-span-1">
            <div className="flex items-center gap-2 mb-4">
              <div className="w-10 h-10 bg-gradient-to-br from-[#6B9080] to-[#A4C3B2] rounded-lg flex items-center justify-center shadow-md">
                <GraduationCap className="w-6 h-6 text-white" />
              </div>
              <span className="text-[#2C3E3F] text-2xl font-semibold">Mentora</span>
            </div>
            <p className="text-[#5A7A6B] text-sm leading-relaxed mb-6">
              Empowering students and professionals to achieve their dreams.
            </p>
            <div className="flex items-center gap-4">
              <a href="#" aria-label="LinkedIn" className="text-[#5A7A6B] hover:text-[#6B9080] transition-colors">
                <Linkedin className="w-5 h-5" />
              </a>
              <a href="#" aria-label="Twitter" className="text-[#5A7A6B] hover:text-[#6B9080] transition-colors">
                <Twitter className="w-5 h-5" />
              </a>
              <a href="#" aria-label="Github" className="text-[#5A7A6B] hover:text-[#6B9080] transition-colors">
                <Github className="w-5 h-5" />
              </a>
            </div>
          </div>

          <div>
            <h4 className="text-[#2C3E3F] mb-4 font-semibold">Quick Links</h4>
            <ul className="space-y-2">
              <li><a href="/" className="text-[#5A7A6B] hover:text-[#6B9080] transition-colors">Home</a></li>
              <li><a href="#why-mentora" className="text-[#5A7A6B] hover:text-[#6B9080] transition-colors">Why Mentora</a></li>
              <li><a href="#features" className="text-[#5A7A6B] hover:text-[#6B9080] transition-colors">Features</a></li>
            </ul>
          </div>

          <div>
            <h4 className="text-[#2C3E3F] mb-4 font-semibold">Resources</h4>
            <ul className="space-y-2">
              <li><a href="/study-tips" className="text-[#5A7A6B] hover:text-[#A4C3B2] transition-colors">Study Tips</a></li>
              <li><a href="/career-advice" className="text-[#5A7A6B] hover:text-[#A4C3B2] transition-colors">Career Advice</a></li>
              <li><a href="/blog" className="text-[#5A7A6B] hover:text-[#A4C3B2] transition-colors">Blog</a></li>
            </ul>
          </div>

          <div>
            <h4 className="text-[#2C3E3F] mb-4 font-semibold">Contact</h4>
            <ul className="space-y-3">
              <li className="flex items-center gap-2 text-[#5A7A6B]">
                <Mail className="w-4 h-4 text-[#6B9080]" />
                <a href="mailto:support@mentora.com" className="hover:text-[#2C3E3F] transition-colors">support@mentora.com</a>
              </li>
              <li className="flex items-center gap-2 text-[#5A7A6B]">
                <Phone className="w-4 h-4 text-[#6B9080]" />
                <a href="tel:+15551234567" className="hover:text-[#2C3E3F] transition-colors">+962 7 7245 6842</a>
              </li>
              <li className="flex items-center gap-2 text-[#5A7A6B]">
                <MapPin className="w-4 h-4 text-[#6B9080]" />
                <span>Global Platform</span>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </footer>
  );
}

/* الصفحة الرئيسية Home */
export default function Home() {
  return (
    <div className="bg-[#F6FFF8] text-[#2C3E3F]">
      <Hero />
      <ProjectSummary />
      <WhyMentora />
    
      <Testimonials />
      <Footer />
    </div>
  );
}