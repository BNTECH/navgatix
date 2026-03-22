import Navbar from '../components/Navbar';
import { Target, Shield, Zap, Users } from 'lucide-react';

const AboutUsPage = () => {
    return (
        <div className="min-h-screen flex flex-col font-sans bg-slate-50">
            <Navbar />
            <main className="flex-1">
                {/* Hero Section */}
                <section className="relative py-24 bg-slate-900 overflow-hidden text-center text-white">
                    <div className="absolute inset-0 bg-[radial-gradient(circle_at_30%_50%,rgba(34,197,94,0.1),transparent)]" />
                    <div className="max-w-4xl mx-auto px-4 sm:px-6 relative z-10 text-center">
                        <img src="/logo.png" alt="Navgatix Logo" className="h-20 mx-auto mb-8 drop-shadow-xl" />
                        <h1 className="text-4xl md:text-6xl font-black mb-6 tracking-tight">
                            Redefining <span className="text-primary-400">Logistics</span> for a Connected World
                        </h1>
                        <p className="text-xl text-slate-300 font-medium">
                            Navgatix is India’s smartest driver and logistics network, built to seamlessly connect businesses with trusted transporters.
                        </p>
                    </div>
                </section>

                {/* Our Mission */}
                <section className="py-20 bg-white">
                    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-16 items-center">
                            <div>
                                <h2 className="text-3xl font-black text-slate-900 mb-6 flex items-center gap-3">
                                    <Target className="h-8 w-8 text-primary-600" />
                                    Our Mission
                                </h2>
                                <p className="text-lg text-slate-600 leading-relaxed mb-6 font-medium">
                                    At Navgatix, we aim to eliminate the friction in modern supply chains. Whether you're a single driver looking for fair, consistent work, or an enterprise scaling operations, we provide the digital infrastructure to make it happen.
                                </p>
                                <p className="text-lg text-slate-600 leading-relaxed font-medium">
                                    By leveraging state-of-the-art mapping, real-time communication, and intelligent matching algorithms, we dramatically reduce empty miles and environmental impact.
                                </p>
                            </div>
                            <div className="grid grid-cols-2 gap-6">
                                <div className="bg-slate-50 p-8 rounded-3xl border border-slate-100 text-center shadow-sm">
                                    <h3 className="text-4xl font-black text-primary-600 mb-2">50k+</h3>
                                    <p className="text-sm font-bold text-slate-500 uppercase tracking-wider">Active Drivers</p>
                                </div>
                                <div className="bg-slate-50 p-8 rounded-3xl border border-slate-100 text-center shadow-sm -mt-8 mb-8">
                                    <h3 className="text-4xl font-black text-blue-600 mb-2">2M+</h3>
                                    <p className="text-sm font-bold text-slate-500 uppercase tracking-wider">Deliveries</p>
                                </div>
                                <div className="bg-slate-50 p-8 rounded-3xl border border-slate-100 text-center shadow-sm">
                                    <h3 className="text-4xl font-black text-purple-600 mb-2">300+</h3>
                                    <p className="text-sm font-bold text-slate-500 uppercase tracking-wider">Cities</p>
                                </div>
                                <div className="bg-slate-50 p-8 rounded-3xl border border-slate-100 text-center shadow-sm -mt-8 mb-8">
                                    <h3 className="text-4xl font-black text-amber-500 mb-2">99%</h3>
                                    <p className="text-sm font-bold text-slate-500 uppercase tracking-wider">On-time</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                {/* Core Values */}
                <section className="py-20 bg-slate-50 relative overflow-hidden">
                    <div className="absolute right-0 top-0 w-1/3 h-full bg-primary-50 rounded-l-[100px] opacity-50"></div>
                    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 relative z-10">
                        <div className="text-center mb-16">
                            <h2 className="text-3xl font-black text-slate-900 mb-4">Our Core Values</h2>
                            <p className="text-lg text-slate-600 max-w-2xl mx-auto font-medium">We are driven by technology, but we are defined by our values.</p>
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
                            <div className="bg-white p-8 rounded-3xl shadow-sm border border-slate-200">
                                <div className="w-14 h-14 bg-primary-100 rounded-2xl flex items-center justify-center mb-6">
                                    <Shield className="h-7 w-7 text-primary-600" />
                                </div>
                                <h3 className="text-xl font-bold text-slate-900 mb-3">Trust & Transparency</h3>
                                <p className="text-slate-600 font-medium">Clear pricing, verified users, and secure tracking mean you always know what's happening and who you're working with.</p>
                            </div>
                            <div className="bg-white p-8 rounded-3xl shadow-sm border border-slate-200">
                                <div className="w-14 h-14 bg-blue-100 rounded-2xl flex items-center justify-center mb-6">
                                    <Zap className="h-7 w-7 text-blue-600" />
                                </div>
                                <h3 className="text-xl font-bold text-slate-900 mb-3">Speed & Efficiency</h3>
                                <p className="text-slate-600 font-medium">Our intelligent routing systems mean less time waiting and more time moving. We optimize every route for maximum yield.</p>
                            </div>
                            <div className="bg-white p-8 rounded-3xl shadow-sm border border-slate-200">
                                <div className="w-14 h-14 bg-purple-100 rounded-2xl flex items-center justify-center mb-6">
                                    <Users className="h-7 w-7 text-purple-600" />
                                </div>
                                <h3 className="text-xl font-bold text-slate-900 mb-3">Driver First</h3>
                                <p className="text-slate-600 font-medium">We believe the backbone of logistics is the driver. We ensure fair pay, respectful treatment, and the tools they need to succeed.</p>
                            </div>
                        </div>
                    </div>
                </section>
            </main>
        </div>
    );
};

export default AboutUsPage;
